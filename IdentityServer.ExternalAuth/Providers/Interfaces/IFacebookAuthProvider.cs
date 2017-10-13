using IdentityServer.ExternalAuth.Entities;

namespace IdentityServer.ExternalAuth.Interfaces
{
    public interface IFacebookAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
