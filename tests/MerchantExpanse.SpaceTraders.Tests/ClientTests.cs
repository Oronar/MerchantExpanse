using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Tests
{
	[TestClass]
	public class ClientTests
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

		#region User

		[TestMethod]
		public async Task GetUserAsync_ReturnsUser()
		{
			var user = new User()
			{
				Username = "test"
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "user", user);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetUserAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(user.Username, result.Username);
		}

		#endregion User

		#region Loans

		[TestMethod]
		public async Task GetLoansAsync_ReturnsLoans()
		{
			var loans = new List<Loan>()
			{
				new Loan()
				{
					Id = "1",
					Due = DateTime.UtcNow,
					RepaymentAmount = 1000,
					Status = "CURRENT",
					Type = "STARTUP"
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "loans", loans);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetLoansAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task GetAvailableLoansAsync_ReturnsLoans()
		{
			var loans = new List<AvailableLoan>()
			{
				new AvailableLoan()
				{
					Amount = 1000,
					CollateralRequired = false,
					Rate = 40,
					TermInDays = 2,
					Type = "STARTUP"
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "loans", loans);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetAvailableLoansAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task TakeOutLoanAsync_ReturnsUser()
		{
			var expectedLoanType = "STARTUP";
			var user = new User()
			{
				Loans = new List<Loan>()
				{
					new Loan()
					{
						Type = expectedLoanType
					}
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "user", user);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.TakeOutLoanAsync("STARTUP");

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "type", expectedLoanType))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Loans.Count());
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "ship", ship);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetShipAsync(ship.Id);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(ship.Id)), It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetShipsAsync_ReturnsShips()
		{
			var ships = new List<Ship>()
			{
				new Ship()
				{
					Id = "1"
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "ships", ships);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetShipsAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task GetAvailableShipsAsync_ReturnsShips()
		{
			var ships = new List<Ship>()
			{
				new Ship()
				{
					Id = "1"
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "ships", ships);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetAvailableShipsAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task GetAvailableShipsAsync_WithClassFilter_ReturnsShips()
		{
			var expectedShipClass = "MK-1";
			var ships = new List<Ship>()
			{
				new Ship()
			};

			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "ships", ships);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetAvailableShipsAsync(expectedShipClass);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "class", expectedShipClass))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task PurchaseShipAsync_ReturnsUser()
		{
			var expectedLocation = "OE";
			var expectedType = "OE-1";

			var user = new User()
			{
				Ships = new List<Ship>()
				{
					new Ship()
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.Created, "user", user);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.PurchaseShipAsync(expectedLocation, expectedType);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "location", expectedLocation)
				&& ContainsParameter(request, "type", expectedType))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Ships.Count());
		}

		[TestMethod]
		public async Task ScrapShipAsync_Returns()
		{
			var expectedShipId = "1a2b";
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "success", new SuccessResponse());
			var client = new Client("apitoken", "username", mockRestClient.Object);

			await client.ScrapShipAsync(expectedShipId);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(expectedShipId))
				, It.IsAny<CancellationToken>()), Times.Once);
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "systems", systems);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetSystemsAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "locations", locations);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetSystemLocations(expectedSystemSymbol);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(expectedSystemSymbol))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task GetLocationAsync_ReturnsLocation()
		{
			var location = new Location()
			{
				Symbol = "OE-PM-TR"
			};
			var expectedShips = 10;
			var mockResponse = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add("location", location);
			payload.Add("dockedShips", expectedShips);

			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(HttpStatusCode.OK);

			var mockRestClient = new Mock<IRestClient>();

			mockRestClient.Setup(m => m.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockResponse.Object);

			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetLocationAsync(location.Symbol);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(location.Symbol))
				, It.IsAny<CancellationToken>()), Times.Once);

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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "location", location);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetLocationShipsAsync(location.Symbol);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(location.Symbol))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(location.Ships.Count(), result.DockedShips);
			Assert.AreEqual(1, result.Ships.Count());
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "location", location);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetMarketplaceAsync(location.Symbol);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(location.Symbol)), It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, location.Marketplace.Count());
		}

		[TestMethod]
		public async Task PurchaseGoodAsync_ReturnsOrder()
		{
			var order = new Order();
			var expectedShipId = "1a2b";
			var expectedGood = "metals";
			var expectedQuantity = 100;
			var expectedCredits = 10000;
			var mockResponse = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add("order", order);
			payload.Add("credits", expectedCredits);

			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(HttpStatusCode.OK);

			var mockRestClient = new Mock<IRestClient>();

			mockRestClient.Setup(m => m.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockResponse.Object);

			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.PurchaseGoodAsync(expectedShipId, expectedGood, expectedQuantity);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "shipId", expectedShipId) &&
				ContainsParameter(request, "good", expectedGood) &&
				ContainsParameter(request, "quantity", expectedQuantity.ToString()))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCredits, result.Credits);
		}

		[TestMethod]
		public async Task SellGoodAsync_ReturnsOrder()
		{
			var order = new Order();
			var expectedShipId = "1a2b";
			var expectedGood = "metals";
			var expectedQuantity = 100;
			var expectedCredits = 10000;
			var mockResponse = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add("order", order);
			payload.Add("credits", expectedCredits);

			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(HttpStatusCode.OK);

			var mockRestClient = new Mock<IRestClient>();

			mockRestClient.Setup(m => m.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockResponse.Object);

			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.SellGoodAsync(expectedShipId, expectedGood, expectedQuantity);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "shipId", expectedShipId) &&
				ContainsParameter(request, "good", expectedGood) &&
				ContainsParameter(request, "quantity", expectedQuantity.ToString()))
				, It.IsAny<CancellationToken>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCredits, result.Credits);
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "flightPlans", flightPlans);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetFlightPlansAsync(expectedSystemSymbol);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(expectedSystemSymbol)), It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task GetFlightPlanAsync_ReturnsFlightPlan()
		{
			var flightPlan = new FlightPlan()
			{
				Id = "1a2b"
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "flightPlan", flightPlan);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetFlightPlanAsync(flightPlan.Id);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request => request.Resource.Contains(flightPlan.Id)), It.IsAny<CancellationToken>()), Times.Once);
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
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "flightPlan", flightPlan);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.SubmitFightPlanAsync(flightPlan.Ship, flightPlan.Destination);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "shipId", flightPlan.Ship) &&
				ContainsParameter(request, "destination", flightPlan.Destination))
				, It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task WarpShipAsync_ReturnsFlightPlan()
		{
			var flightPlan = new FlightPlan()
			{
				Ship = "1a2b"
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "flightPlan", flightPlan);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.WarpShipAsync(flightPlan.Ship);

			mockRestClient.Verify(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
				ContainsParameter(request, "shipId", flightPlan.Ship))
				, It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsNotNull(result);
		}

		#endregion Flight Plans

		[TestMethod]
		public async Task GetStatusAsync_ReturnsStatus()
		{
			var status = "spacetraders is currently online and available to play";
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "status", status);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetStatusAsync();

			mockRestClient.Verify();
			Assert.IsNotNull(result);
			Assert.AreEqual(status, result);
		}

		#region Private Methods

		private bool ContainsParameter(IRestRequest request, string name, string value)
		{
			return request.Parameters.Any(x => x.Name.Equals(name) && x.Value.Equals(value));
		}

		#endregion Private Methods
	}
}