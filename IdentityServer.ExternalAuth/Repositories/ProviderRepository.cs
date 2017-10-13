using System.Collections.Generic;
using IdentityServer.ExternalAuth.Entities;
using IdentityServer.ExternalAuth.Helpers;
using IdentityServer.ExternalAuth.Repositories.Interfaces;

namespace IdentityServer.ExternalAuth.Repositories
{
    public class ProviderRepository : IProviderRepository
    {

        public  IEnumerable<Provider> Get()
        {
            return ProviderDataSource.GetProviders();
        }
    }
}
