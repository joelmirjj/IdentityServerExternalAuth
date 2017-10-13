using IdentityServer.ExternalAuth.Entities;

namespace IdentityServer.ExternalAuth.Interfaces
{
    public interface ITwitterAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
