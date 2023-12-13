using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mvp24Hours.Identity.Keycloak.Infrastructure.Clients
{
    public class TokenClient
    {
        private readonly HttpClient _client;
        private readonly ClientCredentialsTokenRequest _tokenRequest;

        public TokenClient(HttpClient client, ClientCredentialsTokenRequest tokenRequest)
        {
            _client = client;
            _tokenRequest = tokenRequest;
        }

        public async Task<string> GetClientCredentialsToken()
        {
            var response = await _client.RequestClientCredentialsTokenAsync(_tokenRequest);
            return response.AccessToken;
        }
    }
}
