using System;
using System.Collections.Generic;
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
    }

    public User(Guid id) : this() {
      RaiseEvent(new UserCreated(id));
    }

    [UsedImplicitly]
    private void Apply(UserCreated e) {
      Id = e.Id;
    }

    #endregion
    #region Challenge Contact Info
    #region Behavior Methods

    public void PrepareContactIdChallenge(Guid correlationId, string emailOrPhone) {
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
      string token = ContactChallengers.DataProtectionTokenProvider.Instance.Generate(Id, purpose, stamp);
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
      string token = ContactChallengers.DataProtectionTokenProvider.Instance.Generate(Id, purpose, stamp);
      var assembly = Assembly.GetExecutingAssembly();
      string message = assembly.GetManifestResourceText(assembly.GetManifestResourceName($"{purpose}.Message.txt"))
        .Replace("{Code}", code);
      RaiseEvent(new ContactSmsChallengePrepared(correlationId, Id, phoneNumber.NationalNumber,
        ContactIdParser.DefaultRegionCode, purpose, stamp, token, message));
    }

    public void VerifyContactChallengeResponse(Guid correlationId, string code) {
      if (!ContactChallenges.ContainsKey(correlationId) || ContactChallenges[correlationId] == null)
        throw new InvalidOperationException("nope");
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsVerified) throw new InvalidOperationException("nope..?");
      bool isValid = ContactChallengers.TotpCodeProvider.Validate(code, Id, challenge.ContactValue, challenge.Purpose, challenge.Stamp);
      if (!isValid) throw new InvalidOperationException("nope");
      RaiseEvent(new ContactChallengeVerified(correlationId, Id));
    }

    public void CreatePassword(Guid correlationId, string token, string password, string passwordConfirmation) {
      if (string.IsNullOrWhiteSpace(password)) throw new InvalidOperationException("nope");
      if (password != passwordConfirmation) throw new InvalidOperationException("also nope");
      if (!ContactChallenges.ContainsKey(correlationId) || ContactChallenges[correlationId] == null)
        throw new InvalidOperationException("nope");
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsRedeemed || PasswordHash != null) throw new InvalidOperationException("nope");
      bool isValid = ContactChallengers.DataProtectionTokenProvider.Instance.Validate(token, Id, challenge.Purpose, challenge.Stamp);
      if (!isValid) throw new InvalidOperationException("nope");

      string hashedPassword = ContactChallengers.PasswordHasher.Instance.HashPassword(password);
      RaiseEvent(new PasswordCreated(correlationId, Id, hashedPassword));
    }

    #endregion
    #region Internal State

    private IDictionary<Guid, ContactChallenge> ContactChallenges { get; }
    private string PasswordHash { get; set; }

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

    [UsedImplicitly]
    private void Apply(ContactChallengeVerified e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsVerified = true;
    }

    [UsedImplicitly]
    private void Apply(PasswordCreated e) {
      var challenge = ContactChallenges[e.CorrelationId];
      challenge.IsRedeemed = true;
      PasswordHash = e.EncryptedPassword;
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
  }
}
