using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Threading;

namespace MerchantExpanse.SpaceTraders.Tests.TestHelpers
{
	public static class RestSharpMocks
	{
		public static Mock<IRestResponse> BuildMockRestResponse(HttpStatusCode status, string propertyName, object data)
		{
			var mockResponse = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add(propertyName, data);

			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(status);

			return mockResponse;
		}

		public static Mock<IRestClient> BuildMockRestClient(HttpStatusCode status, string propertyName, object data)
		{
			var mockRestClient = new Mock<IRestClient>();
			var mockResponse = BuildMockRestResponse(status, propertyName, data);

			mockRestClient.Setup(m => m.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockResponse.Object)
				.Verifiable();

			return mockRestClient;
		}
	}
}