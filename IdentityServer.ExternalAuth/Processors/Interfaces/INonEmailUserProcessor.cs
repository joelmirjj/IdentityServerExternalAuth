﻿using System.Threading.Tasks;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;

namespace IdentityServer.ExternalAuth.Interfaces.Processors
{
    public interface INonEmailUserProcessor
    {
        Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider);
    }
}
