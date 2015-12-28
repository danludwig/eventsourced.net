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

    public User() {
      Id = GuidComb.NewGuid();
      ContactChallenges = new Dictionary<Guid, ContactChallenge>();
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
      if (ContactChallenges.ContainsKey(correlationId)) throw new InvalidOperationException(
        $"Contact challenge  with correlationId '{correlationId}' has already been prepared.");

      Guid challengeId = GuidComb.NewGuid();
      string stamp = Guid.NewGuid().ToString();
      var purpose = ContactChallengePurpose.CreateUserFromEmail;
      RaiseEvent(new ContactEmailChallengePrepared(correlationId, challengeId, Id, mailAddress.Address, purpose, stamp));
    }

    private void PrepareContactSmsChallenge(Guid correlationId, PhoneNumber phoneNumber) {
      if (ContactChallenges.ContainsKey(correlationId)) throw new InvalidOperationException(
        $"Contact challenge with correlationId '{correlationId}' has already been prepared.");

      Guid challengeId = GuidComb.NewGuid();
      string stamp = Guid.NewGuid().ToString();
      var purpose = ContactChallengePurpose.CreateUserFromPhone;
      RaiseEvent(new ContactSmsChallengePrepared(correlationId, challengeId, Id, phoneNumber.NationalNumber, "US", purpose, stamp));
    }

    #endregion
    #region Internal State

    private IDictionary<Guid, ContactChallenge> ContactChallenges { get; }

    [UsedImplicitly]
    private void Apply(ContactEmailChallengePrepared e) {
      var challenge = new ContactEmailChallenge(e.EmailAddress, e.Stamp, e.Purpose);
      ContactChallenges[e.CorrelationId] = challenge;
    }

    [UsedImplicitly]
    private void Apply(ContactSmsChallengePrepared e) {
      var challenge = new ContactSmsChallenge(e.PhoneNumber, e.RegionCode, e.Stamp, e.Purpose);
      ContactChallenges[e.CorrelationId] = challenge;
    }

    #endregion
    #region Private Classes

    private abstract class ContactChallenge
    {
      protected ContactChallenge(string stamp, ContactChallengePurpose purpose) {
        Stamp = stamp;
        Purpose = purpose;
      }

      internal string Stamp { get; }
      internal ContactChallengePurpose Purpose { get; }
    }

    private class ContactEmailChallenge : ContactChallenge
    {
      public ContactEmailChallenge(string emailAddress, string stamp, ContactChallengePurpose purpose)
        : base(stamp, purpose) {
        MailAddress = new MailAddress(emailAddress);
      }

      internal MailAddress MailAddress { get; set; }
    }

    private class ContactSmsChallenge : ContactChallenge
    {
      public ContactSmsChallenge(ulong phoneNumber, string regionCode, string stamp, ContactChallengePurpose purpose)
        : base(stamp, purpose) {
        PhoneNumber = PhoneNumberUtil.GetInstance().Parse(phoneNumber.ToString(), regionCode);
      }

      internal PhoneNumber PhoneNumber { get; set; }
    }

    #endregion
    #endregion
  }
}
