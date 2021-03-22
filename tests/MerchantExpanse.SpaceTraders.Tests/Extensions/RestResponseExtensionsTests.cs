using MerchantExpanse.SpaceTraders.Exceptions;
using MerchantExpanse.SpaceTraders.Extensions;
using MerchantExpanse.SpaceTraders.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;

namespace MerchantExpanse.SpaceTraders.Tests.Extensions
{
	[TestClass]
	public class RestResponseExtensionsTests
	{
		[TestMethod]
		public void DeserializeContent_WithStatusOK_ReturnsObject()
		{
			var propertyName = "property";
			var mockResponse = BuildRestResponse(HttpStatusCode.OK, propertyName, new TestObject());

			var result = mockResponse.DeserializeContent<TestObject>(propertyName);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void DeserializeContent_WithStatusOk_WithNoPropertyName_ReturnsObject()
		{
			var mockResponse = new Mock<IRestResponse>();
			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(new TestObject()));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(HttpStatusCode.OK);

			var result = mockResponse.Object.DeserializeContent<TestObject>();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void DeserializeContent_WithError_ThrowsApiException()
		{
			var error = new Error()
			{
				Message = "message",
				Code = (int)HttpStatusCode.BadRequest
			};
			var propertyName = "error";
			var mockResponse = BuildRestResponse(HttpStatusCode.NotFound, propertyName, error);

			var exception = Assert.ThrowsException<ApiException>(() => mockResponse.DeserializeContent<TestObject>(propertyName));

			Assert.AreEqual($"({error.Code}) {error.Message}", exception.Message);
		}

		private IRestResponse BuildRestResponse(HttpStatusCode status, string propertyName, object data)
		{
			var mockResponse = new Mock<IRestResponse>();
			var payload = new ExpandoObject() as IDictionary<string, object>;
			payload.Add(propertyName, data);

			mockResponse.SetupGet(m => m.Content)
				.Returns(JsonConvert.SerializeObject(payload));
			mockResponse.SetupGet(m => m.StatusCode)
				.Returns(status);

			return mockResponse.Object;
		}

		public class TestObject
		{
		}
	}
}