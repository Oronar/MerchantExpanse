# Merchant Expanse

MerchantExpanse is a third-party .NET 5 API wrapper written for [SpaceTraders.io](https://spacetraders.io), a multiplayer space trading game built entirely on RESTful API endpoints.

The Space Traders API and this wrapper are still in development and subject to frequent breaking!

## Installation

Using the Nuget Package Manager in Visual Studio, dotnet CLI tool, or the Nuget Package Manager Console install the `MerchantExpanse.SpaceTraders` package.

```ps
# Using dotnet
dotnet add <your-project>.csproj package MerchantExpanse.SpaceTraders
```
```ps
# In Visual Studio select Tools > NuGet Package Manger > Package Manager Console
Install-Package MerchantExpanse.SpaceTraders
```


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
```
```C#
// Register a new account with SpaceTraders
var newClient = await ClientFactory.RegisterAsync("test_account"); // Pick a unique name!
Console.WriteLine($"Username: {newClient.Username}");
Console.WriteLine($"Token: {newClient.ApiToken}"); // Make sure to store this token somewhere safe!
```