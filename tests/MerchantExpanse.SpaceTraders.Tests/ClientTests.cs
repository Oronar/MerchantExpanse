using MerchantExpanse.SpaceTraders.Constants;
using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Tests
{
	[TestClass]
	public partial class ClientTests
	{
		#region Client

		[TestMethod]
		public void Client_WithNullApiToken_ThrowsException()
		{
			var result = Assert.ThrowsException<ArgumentNullException>(() => new Client(null, "username", new Mock<IRestClient>().Object));

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Client_WithNullUsername_ThrowsException()
		{
			var result = Assert.ThrowsException<ArgumentNullException>(() => new Client("apiToken", null, new Mock<IRestClient>().Object));

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Client_WithNullRestClient_ThrowsException()
		{
			var result = Assert.ThrowsException<ArgumentNullException>(() => new Client("apiToken", "username", null));

			Assert.IsNotNull(result);
		}

		#endregion Client

		[TestMethod]
		public async Task GetStatusAsync_ReturnsStatus()
		{
			var status = "spacetraders is currently online and available to play";
			var builder = new TestBuilder()
				.WithResource("game/status")
				.WithPayload("status", status);
			var client = builder.Build();

			var result = await client.GetStatusAsync();

			builder.MockRestClient.Verify();
			Assert.AreEqual(status, result);
		}

		#region User

		[TestMethod]
		public async Task GetUserAsync_ReturnsUser()
		{
			var builder = new TestBuilder()
				.WithResource($"users/username")
				.WithPayload("user", new User());
			var client = builder.Build();

			var result = await client.GetUserAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		#endregion User

		#region Loans

		[TestMethod]
		public async Task GetLoansAsync_ReturnsLoans()
		{
			var loans = new List<Loan>()
			{
				new Loan()
			};
			var builder = new TestBuilder()
				.WithResource("users/username/loans")
				.WithPayload("loans", loans);

			var client = builder.Build();

			var result = await client.GetLoansAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetAvailableLoansAsync_ReturnsLoans()
		{
			var loans = new List<AvailableLoan>()
			{
				new AvailableLoan()
			};
			var builder = new TestBuilder()
				.WithResource("game/loans")
				.WithPayload("loans", loans);

			var client = builder.Build();

			var result = await client.GetAvailableLoansAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task TakeOutLoanAsync_ReturnsUser()
		{
			var expectedLoanType = LoanTypes.Startup;
			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource("users/username/loans")
				.WithPayload("user", new User())
				.WithParameter("type", expectedLoanType);

			var client = builder.Build();

			var result = await client.TakeOutLoanAsync(expectedLoanType);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		#endregion Loans

		#region Ships

		[TestMethod]
		public async Task GetShipAsync_ReturnsShip()
		{
			var ship = new Ship()
			{
				Id = "1a2b"
			};
			var builder = new TestBuilder()
				.WithResource($"users/username/ships/{ship.Id}")
				.WithPayload("user", new User());

			var client = builder.Build();

			var result = await client.GetShipAsync(ship.Id);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetShipsAsync_ReturnsShips()
		{
			var ships = new List<Ship>()
			{
				new Ship()
			};
			var builder = new TestBuilder()
				.WithResource("users/username/ships")
				.WithPayload("ships", ships);
			var client = builder.Build();

			var result = await client.GetShipsAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetAvailableShipsAsync_ReturnsShips()
		{
			var ships = new List<Ship>()
			{
				new Ship()
			};
			var builder = new TestBuilder()
				.WithResource("game/ships")
				.WithPayload("ships", ships);
			var client = builder.Build();

			var result = await client.GetAvailableShipsAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetAvailableShipsAsync_WithClassFilter_ReturnsShips()
		{
			var expectedShipClass = "MK-1";
			var ships = new List<Ship>()
			{
				new Ship()
			};

			var builder = new TestBuilder()
				.WithResource("game/ships")
				.WithPayload("ships", ships)
				.WithParameter("class", expectedShipClass);
			var client = builder.Build();

			var result = await client.GetAvailableShipsAsync(expectedShipClass);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task PurchaseShipAsync_ReturnsUser()
		{
			var expectedLocation = "OE";
			var expectedType = "OE-1";

			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource("users/username/ships")
				.WithPayload("user", new User())
				.WithParameter("location", expectedLocation)
				.WithParameter("type", expectedType);
			var client = builder.Build();

			var result = await client.PurchaseShipAsync(expectedLocation, expectedType);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task ScrapShipAsync_Returns()
		{
			var expectedShipId = "1a2b";
			var builder = new TestBuilder()
				.WithMethod(Method.DELETE)
				.WithResource($"users/username/ships/{expectedShipId}")
				.WithPayload("success", new SuccessResponse());
			var client = builder.Build();

			await client.ScrapShipAsync(expectedShipId);

			builder.MockRestClient.Verify();
		}

		#endregion Ships

		#region Systems

		[TestMethod]
		public async Task GetSystemsAsync_ReturnsSystems()
		{
			var systems = new List<StarSystem>()
			{
				new StarSystem()
			};

			var builder = new TestBuilder()
				.WithResource($"game/systems")
				.WithPayload("systems", systems);
			var client = builder.Build();

			var result = await client.GetSystemsAsync();

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		#endregion Systems

		#region Locations

		[TestMethod]
		public async Task GetSystemLocationsAsync_ReturnsLocations()
		{
			var expectedSystemSymbol = "OE";
			var locations = new List<Location>()
			{
				new Location()
			};
			var builder = new TestBuilder()
				.WithResource($"game/systems/{expectedSystemSymbol}/locations")
				.WithPayload("locations", locations);
			var client = builder.Build();

			var result = await client.GetSystemLocations(expectedSystemSymbol);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetLocationAsync_ReturnsLocation()
		{
			var location = new Location()
			{
				Symbol = "OE-PM-TR"
			};
			var expectedShips = 10;

			var builder = new TestBuilder()
				.WithResource($"game/locations/{location.Symbol}")
				.WithPayload("location", location)
				.WithPayload("dockedShips", expectedShips);
			var client = builder.Build();

			var result = await client.GetLocationAsync(location.Symbol);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedShips, result.DockedShips);
		}

		[TestMethod]
		public async Task GetLocationShipsAsync_ReturnsShips()
		{
			var location = new LocationDetail()
			{
				Symbol = "OE",
				Ships = new List<ShipPlate>()
				{
					new ShipPlate()
				}
			};
			var builder = new TestBuilder()
				.WithResource($"game/locations/{location.Symbol}/ships")
				.WithPayload("location", location);
			var client = builder.Build();

			var result = await client.GetLocationShipsAsync(location.Symbol);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(location.Ships.Count(), result.DockedShips);
		}

		#endregion Locations

		#region Marketplace

		[TestMethod]
		public async Task GetMarketplaceAsync_ReturnsMarketLocation()
		{
			var location = new MarketLocation()
			{
				Symbol = "OE",
				Marketplace = new List<Good>()
				{
					new Good()
				}
			};
			var builder = new TestBuilder()
				.WithResource($"game/locations/{location.Symbol}/marketplace")
				.WithPayload("location", location);
			var client = builder.Build();

			var result = await client.GetMarketplaceAsync(location.Symbol);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task PurchaseGoodAsync_ReturnsOrder()
		{
			var order = new Order()
			{
				Good = GoodTypes.Fuel,
				Quantity = 100
			};
			var expectedShipId = "1a2b";
			var expectedCredits = 10000;

			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource("users/username/purchase-orders")
				.WithPayload("order", order)
				.WithPayload("credits", expectedCredits)
				.WithParameter("shipId", expectedShipId)
				.WithParameter("good", order.Good)
				.WithParameter("quantity", order.Quantity.ToString());
			var client = builder.Build();

			var result = await client.PurchaseGoodAsync(expectedShipId, order.Good, order.Quantity);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCredits, result.Credits);
		}

		[TestMethod]
		public async Task SellGoodAsync_ReturnsOrder()
		{
			var order = new Order()
			{
				Good = GoodTypes.Fuel,
				Quantity = 100
			};
			var expectedShipId = "1a2b";
			var expectedCredits = 10000;

			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource("users/username/sell-orders")
				.WithPayload("order", order)
				.WithPayload("credits", expectedCredits)
				.WithParameter("shipId", expectedShipId)
				.WithParameter("good", order.Good)
				.WithParameter("quantity", order.Quantity.ToString());
			var client = builder.Build();

			var result = await client.SellGoodAsync(expectedShipId, order.Good, order.Quantity);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCredits, result.Credits);
		}

		[TestMethod]
		public async Task JettisonCargoAsync_ReturnsJettisonedCargo()
		{
			var cargo = new JettisonedCargo()
			{
				Good = "FUEL",
				QuantityRemaining = 1,
				ShipId = "1a2b"
			};

			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, cargo);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.JettisonCargoAsync(cargo.ShipId, cargo.Good, 1);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				request.Resource.Contains(cargo.ShipId) &&
				ContainsParameter(request, "good", cargo.Good) &&
				ContainsParameter(request, "quantity", 1.ToString()))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
		}

		#endregion Marketplace

		#region Flight Plans

		[TestMethod]
		public async Task GetFlightPlansAsync_ReturnsFlightPlans()
		{
			var expectedSystemSymbol = "OE";
			var flightPlans = new List<PublicFlightPlan>()
			{
				new PublicFlightPlan()
			};
			var builder = new TestBuilder()
				.WithResource($"game/systems/{expectedSystemSymbol}/flight-plans")
				.WithPayload("flightPlans", flightPlans);
			var client = builder.Build();

			var result = await client.GetFlightPlansAsync(expectedSystemSymbol);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetFlightPlanAsync_ReturnsFlightPlan()
		{
			var flightPlan = new FlightPlan()
			{
				Id = "1a2b"
			};
			var builder = new TestBuilder()
				.WithResource($"users/username/flight-plans/{flightPlan.Id}")
				.WithPayload("flightplan", flightPlan);
			var client = builder.Build();

			var result = await client.GetFlightPlanAsync(flightPlan.Id);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task SubmitFlightPlanAsync_ReturnsFlightPlan()
		{
			var flightPlan = new FlightPlan()
			{
				Id = "1a2b",
				Ship = "3c4e",
				Departure = "AB",
				Destination = "CD"
			};
			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource($"users/username/flight-plans")
				.WithPayload("flightplan", flightPlan)
				.WithParameter("shipId", flightPlan.Ship)
				.WithParameter("destination", flightPlan.Destination);
			var client = builder.Build();

			var result = await client.SubmitFightPlanAsync(flightPlan.Ship, flightPlan.Destination);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task WarpShipAsync_ReturnsFlightPlan()
		{
			var flightPlan = new FlightPlan()
			{
				Ship = "1a2b"
			};
			var builder = new TestBuilder()
				.WithMethod(Method.POST)
				.WithResource($"users/username/warp-jump")
				.WithPayload("flightplan", flightPlan)
				.WithParameter("shipId", flightPlan.Ship);
			var client = builder.Build();

			var result = await client.WarpShipAsync(flightPlan.Ship);

			builder.MockRestClient.Verify();
			Assert.IsNotNull(result);
		}

		#endregion Flight Plans

		#region Private Methods

		private bool ContainsParameter(IRestRequest request, string name, string value)
		{
			return request.Parameters.Any(x => x.Name.Equals(name) && x.Value.Equals(value));
		}

		#endregion Private Methods
	}
}