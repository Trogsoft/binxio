using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Binxio.Abstractions;
using Binxio.Common.Clients;
using Binxio.Common.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binxio.Management.Web.Controllers
{
    [Route("auth")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpFactory;
        private readonly IClientRepository clients;
        private readonly IUserRepository users;
        private readonly IXioLog<AuthController> log;

        public AuthController(IHttpClientFactory httpFactory, IClientRepository clients, IUserRepository users, IXioLog<AuthController> log)
        {
            this.httpFactory = httpFactory;
            this.clients = clients;
            this.users = users;
            this.log = log;
        }

        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return Redirect("https://login.microsoftonline.com/e591b0fa-f212-4c0f-a347-f7f70460dadb/oauth2/v2.0/authorize?client_id=27764a95-e99f-4501-a490-94f3c6cfee1a&scope=https://graph.microsoft.com/User.Read%20https://graph.microsoft.com/User.Read.All&response_type=code");
        }

        // todo: tidy this up.  This is just to get to the point of being able to write the rest of the stuff.

        [Route("sso")]
        [AllowAnonymous]
        public async Task<IActionResult> ReceiveSSOCode(string code, string session_state)
        {

            log.DescribeOperation("Microsoft SSO Login");

            // The secret for the authentication is stored in Azure Key Vault.  Without this, you can not authenticate using this method.
            var client = new SecretClient(vaultUri: new Uri("https://binxio.vault.azure.net/"), credential: new DefaultAzureCredential());
            var secret = client.GetSecret("BinxioAuthClientSecret");

            var httpClient = httpFactory.CreateClient("msAuth");
            var dict = new Dictionary<string, string>();
            dict.Add("client_id", "27764a95-e99f-4501-a490-94f3c6cfee1a");
            dict.Add("scope", "https://graph.microsoft.com/User.Read https://graph.microsoft.com/User.Read.All");
            dict.Add("grant_type", "authorization_code");
            dict.Add("code", code);
            dict.Add("session_state", session_state);
            dict.Add("redirect_url", Request.Scheme + "://" + Request.Host.Value + "/auth/configure_session");
            dict.Add("client_secret", secret.Value.Value);
            FormUrlEncodedContent fuec = new FormUrlEncodedContent(dict);
            var request = httpClient.PostAsync("/e591b0fa-f212-4c0f-a347-f7f70460dadb/oauth2/v2.0/token", fuec).Result;
            if (request.IsSuccessStatusCode)
            {

                log.LogInformation("Token request successful.");
                var graph = httpFactory.CreateClient("msGraph");

                // token
                var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Content.ReadAsStringAsync().Result);
                var token = obj["access_token"];

                // client info
                long clientId = -1;
                var orgRequest = new HttpRequestMessage(HttpMethod.Get, "v1.0/organization");
                orgRequest.Headers.Add("Authorization", "Bearer " + token);
                var orgResponse = await graph.SendAsync(orgRequest);
                if (orgResponse.IsSuccessStatusCode)
                {
                    log.LogInformation("Organization information request successful.");
                    var org = JsonConvert.DeserializeObject<dynamic>(orgResponse.Content.ReadAsStringAsync().Result);
                    if (((JArray)org.value).Count > 0)
                    {
                        // the client is the Azure AD tenant that provied the login.  Create or get it here.
                        var cli = clients.GetClient(new Common.Clients.ClientSpecModel
                        {
                            CountryCode = org.value[0].countryLetterCode,
                            PostCode = org.value[0].postalCode,
                            MicrosoftTenantId = org.value[0].id,
                            Title = org.value[0].displayName,
                        });
                        clientId = cli.Result.Id;
                    }
                    else
                    {
                        // todo: user has to create a client here.
                    }
                }
                else
                {
                    log.LogError("Unable to read information from Microsoft Graph endpoint /organization",
                        ("statusCode", orgResponse.StatusCode.ToString()),
                        ("errorMessage", orgResponse.Content.ReadAsStringAsync().Result));

                    return Content(orgResponse.Content.ReadAsStringAsync().Result);
                }

                var meRequest = new HttpRequestMessage(HttpMethod.Get, "v1.0/me");
                meRequest.Headers.Add("Authorization", "Bearer " + token);
                var meResponse = await graph.SendAsync(meRequest);
                if (meResponse.IsSuccessStatusCode)
                {
                    log.LogInformation("Request for user information successful.");
                    var content = await meResponse.Content.ReadAsStringAsync();
                    var jobj = JsonConvert.DeserializeObject<dynamic>(content);
                    var extant = await users.UserExists(new SingleUserQueryModel(emailAddress: (string)jobj.mail));
                    if (extant.IsSuccessful)
                    {
                        // log this user in.
                        await HttpContext.SignInAsync(users.GetClaimsPrincipal(extant.Result));
                        return Redirect("/");
                    }
                    else
                    {
                        // create user and log in
                        var createdUser = await users.CreateUser(new CreateUserModel
                        {
                            AccessToken = token,
                            ClientId = clientId,
                            DisplayName = jobj.displayName,
                            FirstName = jobj.givenName,
                            LastName = jobj.surname,
                            EmailAddress = jobj.mail,
                            JobTitle = jobj.jobTitle,
                            MicrosoftAccountId = Guid.Parse((string)jobj.id),
                            PreferredLanguage = jobj.preferredLanguage
                        });
                        await HttpContext.SignInAsync(users.GetClaimsPrincipal(createdUser.Result));
                        return Redirect("/");
                    }
                }
                else
                {
                    log.LogError("Unable to read information from Microsoft Graph endpoint /me",
                        ("statusCode", meResponse.StatusCode.ToString()),
                        ("errorMessage", meResponse.Content.ReadAsStringAsync().Result));

                    return Content(meResponse.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                log.LogError("Token request failed.", 
                    ("statusCode", request.StatusCode.ToString()), 
                    ("errorMessage", request.Content.ReadAsStringAsync().Result));

                return Content(request.Content.ReadAsStringAsync().Result);
            }
        }

    }
}
