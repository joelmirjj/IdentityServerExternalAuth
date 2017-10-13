using System.Collections.Generic;
using IdentityServer.ExternalAuth.Entities;

namespace IdentityServer.ExternalAuth.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
        

    }
}
