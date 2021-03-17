using MerchantExpanse.SpaceTraders.Contracts;
using MerchantExpanse.SpaceTraders.Extensions;
using MerchantExpanse.SpaceTraders.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders
{
	public class Client : IClient
	{
		private string Username { get; set; }
		private IRestClient RestClient { get; set; }

		public Client(string apiToken, string username, IRestClient restClient)
		{
			Username = username;
			RestClient = restClient;

			RestClient.Authenticator = new JwtAuthenticator(apiToken);
		}

		public async Task<User> GetUserAsync()
		{
			var request = new RestRequest($"users/{Username}");
			var response = await RestClient.ExecuteAsync(request);

			return response.DeserializeContent<User>("user");
		}
	}
}