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
      try {
        var mailAddress = new MailAddress(emailOrPhone);
        PrepareContactEmailChallenge(correlationId, mailAddress);
      } catch (FormatException) {
        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try {
          PhoneNumber phoneNumber = phoneNumberUtil.Parse(emailOrPhone, "US");
          bool isValid = phoneNumberUtil.IsValidNumber(phoneNumber);
          if (!isValid) {
            throw new InvalidOperationException($"'{emailOrPhone}' does not appear to be a valid email address or US phone number.");
          }
          PrepareContactSmsChallenge(correlationId, phoneNumber);
        } catch (NumberParseException ex) {
          throw new InvalidOperationException($"'{emailOrPhone}' does not appear to be a valid email address or US phone number.", ex);
        }
      }
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
      RaiseEvent(new ContactSmsChallengePrepared(correlationId, Id, phoneNumber.NationalNumber, "US",
        purpose, stamp, token, message));
    }

    public void VerifyContactChallengeResponse(Guid correlationId, string code) {
      if (!ContactChallenges.ContainsKey(correlationId) || ContactChallenges[correlationId] == null)
        throw new InvalidOperationException("nope");
      ContactChallenge challenge = ContactChallenges[correlationId];
      if (challenge.IsVerified) throw new InvalidOperationException("nope..?");
      bool isValid;
      switch (challenge.Purpose) {
        case ContactChallengePurpose.CreateUserFromEmail:
          ContactEmailChallenge emailChallenge = (ContactEmailChallenge)challenge;
          isValid = ContactChallengers.TotpCodeProvider.Validate(code, Id, emailChallenge.MailAddress, challenge.Purpose, challenge.Stamp);
          break;
        case ContactChallengePurpose.CreateUserFromPhone:
          ContactSmsChallenge smsChallenge = (ContactSmsChallenge)challenge;
          isValid = ContactChallengers.TotpCodeProvider.Validate(code, Id, smsChallenge.PhoneNumber, challenge.Purpose, challenge.Stamp);
          break;
        default:
          throw new InvalidOperationException("oops");
      }
      if (!isValid) throw new InvalidOperationException("nope");
      RaiseEvent(new ContactChallengeVerified(correlationId, Id));
    }

    #endregion
    #region Internal State

    private IDictionary<Guid, ContactChallenge> ContactChallenges { get; }

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

    #endregion
    #region Private Classes

    private abstract class ContactChallenge
    {
      protected ContactChallenge(ContactChallengePrepared e) {
        CorrelationId = e.CorrelationId;
        Stamp = e.Stamp;
        Purpose = e.Purpose;
      }

      internal Guid CorrelationId { get; }
      internal string Stamp { get; }
      internal ContactChallengePurpose Purpose { get; }
      internal bool IsVerified { get; set; }
    }

    private class ContactEmailChallenge : ContactChallenge
    {
      internal ContactEmailChallenge(ContactEmailChallengePrepared e) : base(e) {
        MailAddress = new MailAddress(e.EmailAddress);
      }

      internal MailAddress MailAddress { get; set; }
    }

    private class ContactSmsChallenge : ContactChallenge
    {
      internal ContactSmsChallenge(ContactSmsChallengePrepared e) : base(e) {
        PhoneNumber = PhoneNumberUtil.GetInstance().Parse(e.PhoneNumber.ToString(), e.RegionCode);
      }

      internal PhoneNumber PhoneNumber { get; set; }
    }

    #endregion
    #endregion
  }
}
