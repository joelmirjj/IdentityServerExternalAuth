using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.ExternalAuth.Entities;
using IdentityServer.ExternalAuth.Interfaces.Processors;
using IdentityServer.ExternalAuth.Repositories.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace IdentityServer.ExternalAuth.Processors
{
    public class NonEmailUserProcessor<TUser> : INonEmailUserProcessor where TUser : IdentityUser, new()
    {
        private readonly UserManager<TUser> _userManager;
        public NonEmailUserProcessor(UserManager<TUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider)
        {

            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {

                var existingUser = await  _userManager.FindByLoginAsync(provider, userExternalId);
                if (existingUser == null)
                {
                    var customResponse = new Dictionary<string, object>();
                    customResponse.Add("userInfo", userInfo);

                    
                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider, include email paramater and send request again.", customResponse);
                    
                }
                else
                {
                    var userClaims = await _userManager.GetClaimsAsync(existingUser);
                    return new GrantValidationResult(existingUser.Id, provider, userClaims, provider, null);
                }

            }
            else
            {
                var newUser = new TUser { Email = userEmail,UserName = userEmail };
                var result =  _userManager.CreateAsync(newUser).Result;
                if (result.Succeeded)
                {
                    await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                    var userClaims = _userManager.GetClaimsAsync(newUser).Result;
                    return new GrantValidationResult(newUser.Id, provider, userClaims, provider, null);
                }
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }

        }
    }
}
