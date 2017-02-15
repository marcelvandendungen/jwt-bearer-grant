using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Api2 {
    
    [Authorize]
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        public IConfigurationRoot Configuration { get; private set; }

        public TestController(IConfigurationRoot config)
        {
            Configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string resource = Configuration["ResourceUri"];
            System.Console.WriteLine("resource: " + resource);
            var token = await AcquireToken(resource);

            var data = await CallApi1(token);
            return Ok(data);
        }

        private async Task<string> CallApi1(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = new Uri("http://localhost:5000/api/test");
            return await httpClient.GetStringAsync(url);
        }

        private async Task<string> AcquireToken(string resource)
        {
            string tenantId = GetTenantId();
            System.Console.WriteLine(tenantId);
            string clientId = Configuration["ClientId2"];
            System.Console.WriteLine(clientId);
            string clientSecret = Configuration["ClientSecret"];
            System.Console.WriteLine(clientSecret);
            string authority = $"https://login.microsoftonline.com/{tenantId}";
            System.Console.WriteLine(authority);

            ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
            UserAssertion userAssertion = new UserAssertion(GetJwt(), "urn:ietf:params:oauth:grant-type:jwt-bearer", GetUserName());

            var adal = new AuthenticationContext(authority);
            var result = await adal.AcquireTokenAsync(resource, clientCred, userAssertion);

            System.Console.WriteLine("jwt: " + result.AccessToken);
            return result.AccessToken;
        }

        private string GetTenantId()
        {
            var tenantId = User.Claims.Single(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid").Value;
            System.Console.WriteLine("tenantId: " + tenantId);
            return tenantId;
        }

        private string GetUserName()
        {
            var username = User.Claims.Single(c => c.Type == ClaimTypes.Upn).Value;
            System.Console.WriteLine("username: " + username);
            return username;
        }

        private string GetJwt()
        {
            var jwt = User.Identities.First().BootstrapContext as string;
            System.Console.WriteLine("bootstrap: " + jwt);
            return jwt;
        }
    }
}