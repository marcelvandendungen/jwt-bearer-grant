using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            System.Console.WriteLine(resource);
            var token = await acquireToken(resource);

            var data = "Hello from Api2";
            return Ok(data);
        }

        private async Task<string> acquireToken(string resource)
        {
            string tenantId = Configuration["TenantId2"];
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
            var result = await adal.AcquireTokenAsync(resource, clientId, userAssertion);

            return result.AccessToken;
        }

        private string GetUserName()
        {
            var username = User.Claims.Single(c => c.Type == ClaimTypes.Upn).Value;
            System.Console.WriteLine(username);
            return username;
        }

        private string GetJwt()
        {
            var jwt = User.Identities.First().BootstrapContext as string;
            System.Console.WriteLine(jwt);
            return jwt;
        }
    }
}