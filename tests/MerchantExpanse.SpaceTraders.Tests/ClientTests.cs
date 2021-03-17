using MerchantExpanse.SpaceTraders.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using System.Threading;
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
			var mockRestClient = BuildMockRestClient(HttpStatusCode.OK, "user", user);
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var result = await client.GetUserAsync();

			Assert.IsNotNull(result);
			Assert.AreEqual(user.Username, result.Username);
		}

		[TestMethod]
		public async Task GetUserAsync_WithStatusNotFound_ThrowsException()
		{
			var expectedMessage = "message";
			var mockRestClient = BuildMockRestClient(HttpStatusCode.NotFound, "error", new Error() { Message = expectedMessage });
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var exception = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await client.GetUserAsync());

			Assert.AreEqual(expectedMessage, exception.Message);
		}

		[TestMethod]
		public async Task GetUserAsync_WithStatusNotUnauthorized_ThrowsException()
		{
			var expectedMessage = "message";
			var mockRestClient = BuildMockRestClient(HttpStatusCode.Unauthorized, "error", new Error() { Message = expectedMessage });
			var client = new Client("apitoken", "username", mockRestClient.Object);

			var exception = await Assert.ThrowsExceptionAsync<AuthenticationException>(async () => await client.GetUserAsync());

			Assert.AreEqual(expectedMessage, exception.Message);
		}

		private Mock<IRestClient> BuildMockRestClient(HttpStatusCode status, string propertyName, object data)
		{
			var mockRestClient = new Mock<IRestClient>();
			var response = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add(propertyName, data);

			response.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			response.SetupGet(m => m.StatusCode)
				.Returns(status);

			mockRestClient.Setup(m => m.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(response.Object);

			return mockRestClient;
		}
	}
}