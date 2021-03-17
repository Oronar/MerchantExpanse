using MerchantExpanse.SpaceTraders.Contracts;
using MerchantExpanse.SpaceTraders.Extensions;
using MerchantExpanse.SpaceTraders.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
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

		public async Task<IEnumerable<Loan>> GetLoansAsync()
		{
			var request = new RestRequest($"users/{Username}/loans");
			var response = await RestClient.ExecuteAsync(request);

			return response.DeserializeContent<IEnumerable<Loan>>("loans"); ;
		}

		public async Task<IEnumerable<AvailableLoan>> GetAvailableLoansAsync()
		{
			var request = new RestRequest("game/loans");
			var response = await RestClient.ExecuteAsync(request);

			return response.DeserializeContent<IEnumerable<AvailableLoan>>("loans");
		}
	}
}