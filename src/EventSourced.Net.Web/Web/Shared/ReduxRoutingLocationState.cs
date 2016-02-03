using System.Linq;
using Microsoft.AspNet.Http;

namespace EventSourced.Net.Web
{
  public class ReduxRoutingLocationState
  {
    public ReduxRoutingLocationState(HttpRequest request) {
      Query = request.Query.ToDictionary(x => x.Key, x => x.Value);
      Search = request.QueryString.ToString();
    }

    public object Query { get; }
    public string Search { get; }
  }
}
