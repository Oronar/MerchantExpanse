using MerchantExpanse.SpaceTraders.Factories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantExpanse.BasicExample
{
	public class Program
	{
		public static async Task Main()
		{
			// Replace these with your own credentials
			var apiToken = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
			var username = "john";

			var client = ClientFactory.Initialize(apiToken, username); // Initilize a Client with an existing account
			var user = await client.GetUserAsync(); // Retrieve a User object
			var ships = await client.GetShipsAsync(); // Retrieve a list of the user's ships

			Console.WriteLine($"Username: {user.Username}");
			Console.WriteLine($"Credits: {user.Credits}");
			Console.WriteLine($"# of Ships: {ships.Count()}");

			var newClient = await ClientFactory.RegisterAsync("john_test_account"); // Register a new account with SpaceTraders

			Console.WriteLine($"Username: {newClient.Username}");
			Console.WriteLine($"Token: {newClient.ApiToken}"); // Make sure to store this token somewhere safe!
		}
	}
}