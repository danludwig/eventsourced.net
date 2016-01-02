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
    }

    public User(Guid id) : this() {
      RaiseEvent(new UserCreated(id));
    }

    [UsedImplicitly]
    private void Apply(UserCreated e) {
      Id = e.Id;
    }

    #endregion
    #region Registration

    private IDictionary<Guid, ContactChallenge> ContactChallenges { get; }
    private IList<string> ConfirmedLogins { get; }
    private string PasswordHash { get; set; }

    #region Prepare Challenge

    public void PrepareContactChallenge(Guid correlationId, string emailOrPhone) {
      MailAddress mailAddress = ContactIdParser.AsMailAddress(emailOrPhone);
      if (mailAddress != null) {
        PrepareContactEmailChallenge(correlationId, mailAddress);
        return;
      }

      PhoneNumber phoneNumber = ContactIdParser.AsPhoneNumber(emailOrPhone);
      if (phoneNumber != null) {
        PrepareContactSmsChallenge(correlationId, phoneNumber);
        return;
      }

      throw new InvalidOperationException(
        $"'{emailOrPhone}' does not appear to be a valid email address or US phone number.");
    }

    private void PrepareContactEmailChallenge(Guid correlationId, MailAddress mailAddress) {
      if (ContactChallenges.ContainsKey(correlationId) && ContactChallenges[correlationId] != null)
        throw new InvalidOperationException(
          $"Contact challenge  for correlationId '{correlationId}' has already been prepared.");

      string stamp = Guid.NewGuid().ToString();
      var purpose = ContactChallengePurpose.CreateUserFromEmail;
      string code = ContactChallengers.TotpCodeProvider.Generate(Id, mailAddress, purpose, stamp);
      string token = ContactChallengers.DataProtectionTokenProvider.Generate(Id, purpose, stamp);
      var assembly = Assembly.GetExecutingAssembly();
      string body = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Body.txt"))
        .Replace("{Code}", code);
      string subject = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Subject.txt"));
      RaiseEvent(new ContactEmailChallengePrepared(correlationId, Id, mailAddress.Address,
        purpose, stamp, token, subject, body));
    }

    private void PrepareContactSmsChallenge(Guid correlationId, PhoneNumber phoneNumber) {
      if (ContactChallenges.ContainsKey(correlationId) && ContactChallenges[correlationId] != null)
        throw new InvalidOperationException(
          $"Contact challenge  for correlationId '{correlationId}' has already been prepared.");

      string stamp = Guid.NewGuid().ToString();
      var purpose = ContactChallengePurpose.CreateUserFromPhone;
      string code = ContactChallengers.TotpCodeProvider.Generate(Id, phoneNumber, purpose, stamp);
      string token = ContactChallengers.DataProtectionTokenProvider.Generate(Id, purpose, stamp);
      var assembly = Assembly.GetExecutingAssembly();
      string message = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Message.txt"))
        .Replace("{Code}", code);
      RaiseEvent(new ContactSmsChallengePrepared(correlationId, Id, phoneNumber.NationalNumber,
        ContactIdParser.DefaultRegionCode, purpose, stamp, token, message));
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

    public void VerifyContactChallengeResponse(Guid correlationId, string code) {
      if (!ContactChallenges.ContainsKey(correlationId) || ContactChallenges[correlationId] == null)
        throw new InvalidOperationException("nope");
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsVerified) throw new InvalidOperationException("nope..?");
      bool isValid = ContactChallengers.TotpCodeProvider.Validate(code, Id, challenge.ContactValue, challenge.Purpose, challenge.Stamp);
      if (!isValid) throw new InvalidOperationException("nope");
      RaiseEvent(new ContactChallengeVerified(correlationId, Id));
    }

    [UsedImplicitly]
    private void Apply(ContactChallengeVerified e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsVerified = true;
    }

    #endregion
    #region Redeem to Create Password

    public void CreatePassword(Guid correlationId, string token, string password, string passwordConfirmation) {
      if (string.IsNullOrWhiteSpace(password)) throw new InvalidOperationException("nope");
      if (password != passwordConfirmation) throw new InvalidOperationException("also nope");
      if (!ContactChallenges.ContainsKey(correlationId) || ContactChallenges[correlationId] == null)
        throw new InvalidOperationException("nope");
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsRedeemed || PasswordHash != null) throw new InvalidOperationException("nope");
      bool isValid = ContactChallengers.DataProtectionTokenProvider.Validate(token, Id, challenge.Purpose, challenge.Stamp);
      if (!isValid) throw new InvalidOperationException("nope");

      string hashedPassword = ContactChallengers.PasswordHasher.Instance.HashPassword(password);
      RaiseEvent(new PasswordCreated(correlationId, Id, hashedPassword));
    }

    [UsedImplicitly]
    private void Apply(PasswordCreated e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsRedeemed = true;
      ConfirmedLogins.Add(challenge.ContactValue);
      PasswordHash = e.PasswordHash;
    }

    #endregion
    #region Private Classes

    private abstract class ContactChallenge
    {
      protected ContactChallenge(ContactChallengePrepared e) {
        Stamp = e.Stamp;
        Purpose = e.Purpose;
      }

      internal string Stamp { get; }
      internal ContactChallengePurpose Purpose { get; }
      internal bool IsVerified { get; set; }
      internal bool IsRedeemed { get; set; }
      internal abstract string ContactValue { get; }
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

    public void VerifyLogin(string login, string password, out Exception exceptionToThrowAfterSave) {
      if (string.IsNullOrWhiteSpace(login)) throw new InvalidOperationException("nope");
      if (!ConfirmedLogins.Any(x => string.Equals(x, ContactIdParser.Normalize(login)))) throw new InvalidOperationException("nope");
      exceptionToThrowAfterSave = null;
      bool isRehashRequired; // = false;
      bool isVerified = ContactChallengers.PasswordHasher.Instance.VerifyHashedPassword(PasswordHash, password, out isRehashRequired);

      DateTime happenedOn = DateTime.UtcNow;
      if (isVerified) {
        string passwordRehash = null;
        if (isRehashRequired) {
          passwordRehash = ContactChallengers.PasswordHasher.Instance.HashPassword(password);
        }
        RaiseEvent(new UserLoginVerified(login, happenedOn, passwordRehash));
      } else {
        exceptionToThrowAfterSave = new Exception("nope");
        RaiseEvent(new UserAttemptedLoginWithUnverifiedPassword(login, happenedOn));
      }
    }


    [UsedImplicitly]
    private void Apply(UserLoginVerified e) {
      if (e.PasswordRehash != null) {
        PasswordHash = e.PasswordRehash;
      }
    }

    [UsedImplicitly]
    private void Apply(UserAttemptedLoginWithUnverifiedPassword e) {
      // todo: lock out user after 3 bad password attempts
    }

    #endregion
  }
}
