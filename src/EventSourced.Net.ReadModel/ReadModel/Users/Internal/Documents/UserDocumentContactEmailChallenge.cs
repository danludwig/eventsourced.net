using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserDocumentContactEmailChallenge : UserDocumentContactChallenge
  {
    public UserDocumentContactEmailChallenge() { }

    public UserDocumentContactEmailChallenge(ContactEmailChallengePrepared message)
      : base(message) {
      EmailAddress = message.EmailAddress;
      MessageSubject = message.MessageSubject;
      MessageBody = message.MessageBody;
    }

    public string EmailAddress { get; set; }
    public string MessageSubject { get; set; }
    public string MessageBody { get; set; }
    public override string ContactValue => EmailAddress;
  }
}