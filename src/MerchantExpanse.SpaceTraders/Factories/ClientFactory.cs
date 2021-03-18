using MerchantExpanse.SpaceTraders.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Factories
{
	public static class ClientFactory
	{
		private const string SpaceTradersUrl = "https://api.spacetraders.io";

		public async static Task<Client> RegisterAsync(string username)
		{
			var request = new RestRequest($"users/{username}/token", Method.POST);
			var response = await new RestClient(SpaceTradersUrl).ExecuteAsync(request);

			var apiToken = response.DeserializeContent<string>("token");

			return Initialize(apiToken, username);
		}

		public static Client Initialize(string apiToken, string username)
		{
			var restClient = new RestClient(SpaceTradersUrl)
			{
				Authenticator = new JwtAuthenticator(apiToken)
			};
			return new Client(apiToken, username, restClient);
		}
	}
}