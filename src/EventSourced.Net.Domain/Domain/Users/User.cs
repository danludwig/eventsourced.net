using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using JetBrains.Annotations;
using PhoneNumbers;

namespace EventSourced.Net.Domain.Users
{
  public class User : CommonDomain.Core.AggregateBase
  {
    #region Construction

    private User() {
      ContactChallenges = new Dictionary<Guid, ContactChallenge>();
      ConfirmedLogins = new List<string>();
      PasswordHashes = new List<string>();
    }

    public User(Guid id) : this() {
      RaiseEvent(new Created(id, DateTime.UtcNow));
    }

    [UsedImplicitly]
    private void Apply(Created e) {
      Id = e.AggregateId;
    }

    #endregion
    #region Registration

    private IDictionary<Guid, ContactChallenge> ContactChallenges { get; }
    private IList<string> ConfirmedLogins { get; }
    private IList<string> PasswordHashes { get; }
    private string CurrentPasswordHash => PasswordHashes.Count > 0 ? PasswordHashes.Last() : null;

    #region Prepare Challenge

    public void PrepareContactChallenge(Guid correlationId, string emailOrPhone) {
      if (ContactChallenges.ContainsKey(correlationId))
        throw new CommandRejectedException(nameof(correlationId), correlationId, CommandRejectionReason.AlreadyApplied,
          $"Contact challenge for correlationId '{correlationId}' has already been prepared.");

      MailAddress mailAddress = ContactIdParser.AsMailAddress(emailOrPhone);
      if (mailAddress != null) {
        PrepareContactEmailChallenge(correlationId, mailAddress, Guid.NewGuid().ToString(),
          ContactChallengePurpose.CreateUserFromEmail);
        return;
      }

      PhoneNumber phoneNumber = ContactIdParser.AsPhoneNumber(emailOrPhone);
      if (phoneNumber != null) {
        PrepareContactSmsChallenge(correlationId, phoneNumber, Guid.NewGuid().ToString(),
          ContactChallengePurpose.CreateUserFromPhone);
        return;
      }

      throw new CommandRejectedException(nameof(emailOrPhone), emailOrPhone,
        CommandRejectionReason.InvalidFormat);
    }

    private void PrepareContactEmailChallenge(Guid correlationId, MailAddress mailAddress, string stamp,
      ContactChallengePurpose purpose) {
      string code = ContactChallengers.TotpCodeProvider.Generate(Id, mailAddress, purpose, stamp);
      string token = ContactChallengers.DataProtectionTokenProvider.Generate(Id, purpose, stamp);
      var assembly = Assembly.GetExecutingAssembly();
      string body = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Body.txt"))
        .Replace("{Code}", code);
      string subject = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Subject.txt"));
      RaiseEvent(new ContactEmailChallengePrepared(Id, DateTime.UtcNow, correlationId,
        mailAddress.Address, purpose, stamp, token, subject, body));
    }

    private void PrepareContactSmsChallenge(Guid correlationId, PhoneNumber phoneNumber, string stamp,
      ContactChallengePurpose purpose) {
      string code = ContactChallengers.TotpCodeProvider.Generate(Id, phoneNumber, purpose, stamp);
      string token = ContactChallengers.DataProtectionTokenProvider.Generate(Id, purpose, stamp);
      var assembly = Assembly.GetExecutingAssembly();
      string message = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Message.txt"))
        .Replace("{Code}", code);
      RaiseEvent(new ContactSmsChallengePrepared(Id, DateTime.UtcNow, correlationId,
        phoneNumber.NationalNumber, ContactIdParser.DefaultRegionCode, purpose, stamp, token, message));
    }

    [UsedImplicitly]
    private void Apply(ContactEmailChallengePrepared e) {
      var challenge = new ContactEmailChallenge(e);
      ContactChallenges[e.CorrelationId] = challenge;
    }

    [UsedImplicitly]
    private void Apply(ContactSmsChallengePrepared e) {
      var challenge = new ContactSmsChallenge(e);
      ContactChallenges[e.CorrelationId] = challenge;
    }

    #endregion
    #region Verify Code

    public void VerifyContactChallengeResponse(Guid correlationId, string code, out CommandRejectedException exceptionToThrowAfterSave) {
      RejectIfNullContactChallengeCorrelation(correlationId);
      ContactChallenge challenge = ContactChallenges[correlationId];
      exceptionToThrowAfterSave = null;

      if (challenge.IsCodeVerified)
        throw new CommandRejectedException(nameof(code), code, CommandRejectionReason.AlreadyApplied);

      if (challenge.IsMaxCodeAttemptsExhausted)
        throw new CommandRejectedException(nameof(code), code, CommandRejectionReason.MaxAttempts);

      bool isValid = ContactChallengers.TotpCodeProvider.Validate(code, Id, challenge.ContactValue, challenge.Purpose, challenge.Stamp);
      DateTime happenedOn = DateTime.UtcNow;

      if (isValid) {
        RaiseEvent(new ContactChallengeResponseVerified(Id, happenedOn, correlationId, challenge.NextCodeAttemptNumber));
      } else {
        RaiseEvent(new ContactChallengeResponseInvalidCodeAttempted(Id, happenedOn, correlationId, code, challenge.NextCodeAttemptNumber));
        if (challenge.CodeAttemptsRemainingCount <= 0) {
          RaiseEvent(new ContactChallengeResponseMaxInvalidCodesAttempted(Id, happenedOn, correlationId));
          exceptionToThrowAfterSave = new CommandRejectedException(nameof(code), code, CommandRejectionReason.MaxAttempts);
          return;
        }
        exceptionToThrowAfterSave = new CommandRejectedException(nameof(code), code, CommandRejectionReason.Unverified,
          new { challenge.CodeAttemptsRemainingCount, });
      }
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeResponseVerified e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsCodeVerified = true;
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeResponseInvalidCodeAttempted e) {
      var challenge = ContactChallenges[e.CorrelationId];
      ++challenge.InvalidCodeAttemptCount;
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeResponseMaxInvalidCodesAttempted e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsMaxCodeAttemptsExhausted = true;
    }

    #endregion
    #region Redeem to Create Password

    public void RedeemContactChallenge(Guid correlationId, string token, string password) {
      RejectIfNullContactChallengeCorrelation(correlationId);
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsTokenRedeemed)
        throw new CommandRejectedException(nameof(token), token, CommandRejectionReason.AlreadyApplied);

      // todo: create a better password policy
      const int minCharacters = 8;
      if (password.Length < minCharacters)
        throw new CommandRejectedException(nameof(password), password, CommandRejectionReason.InvalidFormat, new {
          MinCharacters = minCharacters,
        });

      bool isValid = ContactChallengers.DataProtectionTokenProvider.Validate(token, Id, challenge.Purpose, challenge.Stamp);
      if (!isValid) throw new CommandRejectedException(nameof(token), token, CommandRejectionReason.Unverified);

      string hashedPassword = ContactChallengers.PasswordHasher.Instance.HashPassword(password);
      RaiseEvent(new ContactChallengeRedeemed(Id, DateTime.UtcNow, correlationId, hashedPassword));
    }

    public void ReverseContactChallengeRedemption(Guid correlationId) {
      RejectIfNullContactChallengeCorrelation(correlationId);
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (!challenge.IsTokenRedeemed)
        throw new CommandRejectedException(nameof(correlationId), correlationId, CommandRejectionReason.AlreadyApplied);
      RaiseEvent(new ContactChallengeRedemptionReversed(Id, DateTime.UtcNow, correlationId));
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeRedeemed e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsTokenRedeemed = true;
      ConfirmedLogins.Add(challenge.NormalizedContactValue);
      PasswordHashes.Add(e.PasswordHash);
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeRedemptionReversed e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsTokenRedeemed = false;
      ConfirmedLogins.Remove(challenge.NormalizedContactValue);
      PasswordHashes.RemoveAt(PasswordHashes.Count - 1);
    }

    #endregion
    #region Private Classes & Methods

    private void RejectIfNullContactChallengeCorrelation(Guid correlationId) {
      if (!ContactChallenges.ContainsKey(correlationId))
        throw new CommandRejectedException(nameof(correlationId), correlationId, CommandRejectionReason.NotFound,
          $"{GetType().Name} '{Id}' has no prepared contact challenge for correlation id '{correlationId}'.");
    }

    private abstract class ContactChallenge
    {
      private const int MaxInvalidCodeAttempts = 3;

      protected ContactChallenge(ContactChallengePrepared e) {
        Stamp = e.Stamp;
        Purpose = e.Purpose;
      }

      internal string Stamp { get; }
      internal ContactChallengePurpose Purpose { get; }
      internal abstract string ContactValue { get; }
      internal string NormalizedContactValue => ContactIdParser.Normalize(ContactValue);
      internal bool IsCodeVerified { get; set; }
      internal int InvalidCodeAttemptCount { get; set; }
      internal int NextCodeAttemptNumber => InvalidCodeAttemptCount + 1;
      internal int CodeAttemptsRemainingCount => MaxInvalidCodeAttempts - InvalidCodeAttemptCount;
      internal bool IsMaxCodeAttemptsExhausted { get; set; }
      internal bool IsTokenRedeemed { get; set; }
    }

    private class ContactEmailChallenge : ContactChallenge
    {
      internal ContactEmailChallenge(ContactEmailChallengePrepared e) : base(e) {
        MailAddress = new MailAddress(e.EmailAddress);
      }

      internal MailAddress MailAddress { get; }
      internal override string ContactValue => MailAddress.Address;
    }

    private class ContactSmsChallenge : ContactChallenge
    {
      internal ContactSmsChallenge(ContactSmsChallengePrepared e) : base(e) {
        PhoneNumber = ContactIdParser.AsPhoneNumber(e.PhoneNumber, e.RegionCode);
      }

      internal PhoneNumber PhoneNumber { get; }
      internal override string ContactValue => PhoneNumber.NationalNumber.ToString();
    }

    #endregion

    #endregion
    #region Login

    public void VerifyLogin(string login, string password, out CommandRejectedException exceptionToThrowAfterSave) {
      var unverifiedException = new CommandRejectedException(nameof(login), null, CommandRejectionReason.Unverified);
      if (!ConfirmedLogins.Any(x => string.Equals(x, ContactIdParser.Normalize(login))))
        throw unverifiedException;

      exceptionToThrowAfterSave = null;
      bool isRehashRequired; // = false;
      bool isVerified = ContactChallengers.PasswordHasher.Instance.VerifyHashedPassword(CurrentPasswordHash, password, out isRehashRequired);

      DateTime happenedOn = DateTime.UtcNow;
      if (isVerified) {
        string passwordRehash = null;
        if (isRehashRequired) {
          passwordRehash = ContactChallengers.PasswordHasher.Instance.HashPassword(password);
        }
        RaiseEvent(new LoginVerified(Id, happenedOn, login, passwordRehash));
      } else {
        exceptionToThrowAfterSave = unverifiedException;
        RaiseEvent(new LoginInvalidPasswordAttempted(Id, happenedOn, login));
      }
    }


    [UsedImplicitly]
    private void Apply(LoginVerified e) {
      if (e.PasswordRehash != null) {
        PasswordHashes.RemoveAt(PasswordHashes.Count - 1);
        PasswordHashes.Add(e.PasswordRehash);
      }
    }

    [UsedImplicitly]
    private void Apply(LoginInvalidPasswordAttempted e) {
      // todo: lock out user after 3 bad password attempts
    }

    #endregion
  }
}
