using MerchantExpanse.SpaceTraders.Exceptions;
using MerchantExpanse.SpaceTraders.Extensions;
using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
			var mockResponse = RestSharpMocks.BuildMockRestResponse(HttpStatusCode.OK, propertyName, new TestObject()).Object;

			var result = mockResponse.DeserializeContent<TestObject>(propertyName);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void DeserializeContent_WithError_ThrowsApiException()
		{
			var propertyName = "error";
			var expectedMessage = "message";
			var expectedStatusCode = 404;
			var mockResponse = RestSharpMocks.BuildMockRestResponse(HttpStatusCode.NotFound, propertyName, new Error() { Message = expectedMessage, Code = expectedStatusCode }).Object;

			var exception = Assert.ThrowsException<ApiException>(() => mockResponse.DeserializeContent<TestObject>(propertyName));

			Assert.AreEqual($"({expectedStatusCode}) {expectedMessage}", exception.Message);
		}

		public class TestObject
		{
		}
	}
}