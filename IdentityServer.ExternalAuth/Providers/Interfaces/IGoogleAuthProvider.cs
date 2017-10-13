using IdentityServer.ExternalAuth.Entities;

namespace IdentityServer.ExternalAuth.Interfaces
{
    public interface IGoogleAuthProvider:IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
