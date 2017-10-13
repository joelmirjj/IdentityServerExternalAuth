using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.ExternalAuth.Helpers
{
    public static class QueryBuilder
    {
        public static string FacebookUserInfoQuery(List<string> fields, string token)
        {
            return "?fields=" + string.Join(",", fields) + "&access_token=" + token;
        }

        public static string GetQuery(Dictionary<string, string> values, ProviderType provider)
        {
            switch (provider)
            {
                case ProviderType.Facebook:

                    var fields = values["fields"];
                    var accessToken = values["access_token"];
                    return $"?fields={fields}&access_token={accessToken}";

                case ProviderType.Twitter:
                    var token = values["tokenString"];
                    var userInfoEndpoint = values["endpoint"];

                    var tokenString = token.Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
                    if (tokenString.Count < 4) return null;

                    var oauthConsumerKey = tokenString["oauth_consumer_key"];
                    var consumerSecret = tokenString["oauth_consumer_secret"];
                    var oauthTokenSecret = tokenString["oauth_token_secret"];
                    var oauthToken = tokenString["oauth_token"];
                    var oauthVersion = "1.0";
                    var oauthSignatureMethod = "HMAC-SHA1";
                    var oauthNonce =
                        Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

                    var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    var oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

                    var sd = new SortedDictionary<string, string>
                    {
                        { "oauth_version", oauthVersion },
                        { "oauth_consumer_key", oauthConsumerKey },
                        { "oauth_nonce", oauthNonce },
                        { "oauth_signature_method", oauthSignatureMethod },
                        { "oauth_timestamp", oauthTimestamp },
                        { "oauth_token", oauthToken }
                    };

                    //GS - Build the signature string
                    var baseString = String.Empty;
                    baseString += "GET" + "&";
                    baseString += Uri.EscapeDataString(userInfoEndpoint) + "&";
                    foreach (var entry in sd)
                    {
                        baseString += Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&");
                    }

                    baseString = baseString.Substring(0, baseString.Length - 3);

                    var signingKey = Uri.EscapeDataString(consumerSecret) + "&" +
                                     Uri.EscapeDataString(oauthTokenSecret);

                    var hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));

                    var signatureString =
                        Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

                    //prepare the request
                    var authorizationHeaderParams = String.Empty;
                    authorizationHeaderParams += "OAuth ";
                    authorizationHeaderParams += $"oauth_nonce=\"{Uri.EscapeDataString(oauthNonce)}\",";

                    authorizationHeaderParams +=
                        $"oauth_signature_method=\"{Uri.EscapeDataString(oauthSignatureMethod)}\",";

                    authorizationHeaderParams += $"oauth_timestamp=\"{Uri.EscapeDataString(oauthTimestamp)}\",";

                    authorizationHeaderParams += $"oauth_consumer_key=\"{Uri.EscapeDataString(oauthConsumerKey)}\",";

                    authorizationHeaderParams += $"oauth_token=\"{Uri.EscapeDataString(oauthToken)}\",";

                    authorizationHeaderParams += $"oauth_signature=\"{Uri.EscapeDataString(signatureString)}\",";

                    authorizationHeaderParams += $"oauth_version=\"{Uri.EscapeDataString(oauthVersion)}\"";

                    return authorizationHeaderParams;

                case ProviderType.Google:

                    var googleAccessToken = values["token"];
                    return $"?access_token={googleAccessToken}";

                case ProviderType.LinkedIn:
                    return null;
                default:
                    return null;
            }
        }
    }
}
