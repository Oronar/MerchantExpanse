using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Tests
{
	[TestClass]
	public class ClientTests
	{
		[TestMethod]
		public async Task GetUserAsync_WithStatusOK_ReturnsUser()
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
	}
}