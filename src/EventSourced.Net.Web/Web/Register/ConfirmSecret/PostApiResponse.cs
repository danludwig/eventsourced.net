using EventSourced.Net.ReadModel.Users;

namespace EventSourced.Net.Web.Register.ConfirmSecret
{
  public class PostApiResponse : CreateLogin.ReduxViewData
  {
    public PostApiResponse(UserContactChallengeTokenData data)
        : base(data) { }
  }
}