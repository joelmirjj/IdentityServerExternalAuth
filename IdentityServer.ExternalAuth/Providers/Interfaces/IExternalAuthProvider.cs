using Newtonsoft.Json.Linq;

namespace IdentityServer.ExternalAuth.Interfaces
{
    public interface IExternalAuthProvider
    {
        JObject GetUserInfo(string accessToken);
    }
}
