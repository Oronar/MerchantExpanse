using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Tests
{
	[TestClass]
	public class ClientTests
	{
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

			Assert.IsNotNull(result);
			Assert.AreEqual(user.Username, result.Username);
		}

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

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public async Task TakeOutLoanAsync_ReturnsUser()
		{
			var user = new User()
			{
				Loans = new List<Loan>()
				{
					new Loan()
					{
						Id = "1",
						Due = DateTime.UtcNow,
						RepaymentAmount = 1000,
						Status = "CURRENT",
						Type = "STARTUP"
					}
				}
			};
			var mockRestClient = RestSharpMocks.BuildMockRestClient(HttpStatusCode.OK, "user", user);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.TakeOutLoanAsync("STARTUP");

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Loans.Count());
		}
	}
}