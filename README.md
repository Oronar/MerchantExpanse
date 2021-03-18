# Merchant Expanse

MerchantExpanse is a third-party .NET 5 API wrapper written for [SpaceTraders.io](https://spacetraders.io), a multiplayer space trading game built entirely on RESTful API endpoints.

The Space Traders API and this wrapper are still in development and subject to frequent breaking!

## Examples
```C#
// Replace these with your own credentials
var apiToken = "<your-token-here>";
var username = "<your-username-here>";

// Initilize a Client with an existing account
var client = ClientFactory.Initialize(apiToken, username); 

// Retrieve a User object containing your account's data
var user = await client.GetUserAsync();

// Retrieve a list of your owned ships
var ships = await client.GetShipsAsync(); 

Console.WriteLine($"Username: {user.Username}");
Console.WriteLine($"Credits: {user.Credits}");
Console.WriteLine($"# of Ships: {ships.Count()}");

// Register a new account with SpaceTraders
var newClient = await ClientFactory.RegisterAsync("john_test_account");
Console.WriteLine($"Username: {newClient.Username}");
Console.WriteLine($"Token: {newClient.ApiToken}"); // Make sure to store this token somewhere safe!
```