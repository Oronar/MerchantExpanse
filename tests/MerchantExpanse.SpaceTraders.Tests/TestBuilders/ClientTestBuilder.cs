using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;

namespace MerchantExpanse.SpaceTraders.Tests
{
	public partial class ClientTests
	{
		public class TestBuilder
		{
			public Mock<IRestResponse> MockRestResponse { get; set; } = new Mock<IRestResponse>();

			public Mock<IRestClient> MockRestClient { get; set; } = new Mock<IRestClient>();

			public string Username { get; set; } = "username";

			public string ApiToken { get; set; } = "token";

			public Method Method { get; set; } = Method.GET;

			public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

			public string Resource { get; set; }

			public IDictionary<string, object> Data { get; set; }

			public IDictionary<string, string> Parameters { get; set; }

			public object Payload { get; set; }

			public TestBuilder()
			{
				Data = new Dictionary<string, object>();
				Parameters = new Dictionary<string, string>();
			}

			public TestBuilder WithUsername(string username)
			{
				Username = username;
				return this;
			}

			public TestBuilder WithApiToken(string token)
			{
				ApiToken = token;
				return this;
			}

			public TestBuilder WithMethod(Method method)
			{
				Method = method;
				return this;
			}

			public TestBuilder WithStatus(HttpStatusCode status)
			{
				Status = status;
				return this;
			}

			public TestBuilder WithResource(string resource)
			{
				Resource = resource;
				return this;
			}

			public TestBuilder WithPayload(string propertyName, object payload)
			{
				Data.Add(propertyName, payload);
				return this;
			}

			public TestBuilder WithParameter(string name, string value)
			{
				Parameters.Add(name, value);
				return this;
			}

			public Client Build()
			{
				var payload = new ExpandoObject() as IDictionary<string, object>;
				foreach (var data in Data)
				{
					payload.Add(data.Key, data.Value);
				}

				MockRestResponse.SetupGet(m => m.Content)
					.Returns(JsonConvert.SerializeObject(payload));

				MockRestResponse.SetupGet(m => m.StatusCode)
					.Returns(Status);

				MockRestClient.Setup(m => m.ExecuteAsync(It.Is<IRestRequest>(request =>
					request.Resource.Equals(Resource) &&
					request.Method.Equals(Method) &&
					Parameters.All(x => ContainsParameter(request, x.Key, x.Value)))
					, It.IsAny<CancellationToken>()))
					.ReturnsAsync(MockRestResponse.Object)
					.Verifiable();

				return new Client(ApiToken, Username, MockRestClient.Object);
			}

			private bool ContainsParameter(IRestRequest request, string name, string value)
			{
				return request.Parameters.Any(x => x.Name.Equals(name) && x.Value.Equals(value));
			}
		}
	}
}