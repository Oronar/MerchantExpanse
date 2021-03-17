using MerchantExpanse.SpaceTraders.Extensions;
using MerchantExpanse.SpaceTraders.Models;
using MerchantExpanse.SpaceTraders.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;

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
		public void DeserializeContent_WithStatusNotFound_ThrowsException()
		{
			var propertyName = "error";
			var expectedMessage = "message";
			var mockResponse = RestSharpMocks.BuildMockRestResponse(HttpStatusCode.NotFound, propertyName, new Error() { Message = expectedMessage }).Object;

			var exception = Assert.ThrowsException<KeyNotFoundException>(() => mockResponse.DeserializeContent<TestObject>(propertyName));

			Assert.AreEqual(expectedMessage, exception.Message);
		}

		[TestMethod]
		public void DeserializeContent_WithStatusNotUnauthorized_ThrowsException()
		{
			var propertyName = "error";
			var expectedMessage = "message";
			var mockResponse = RestSharpMocks.BuildMockRestResponse(HttpStatusCode.Unauthorized, propertyName, new Error() { Message = expectedMessage }).Object;

			var exception = Assert.ThrowsException<AuthenticationException>(() => mockResponse.DeserializeContent<TestObject>(propertyName));

			Assert.AreEqual(expectedMessage, exception.Message);
		}

		public class TestObject
		{
		}
	}
}