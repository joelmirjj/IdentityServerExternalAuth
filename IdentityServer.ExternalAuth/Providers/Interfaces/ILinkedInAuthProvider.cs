using IdentityServer.ExternalAuth.Entities;

namespace IdentityServer.ExternalAuth.Interfaces
{
   public interface ILinkedInAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
