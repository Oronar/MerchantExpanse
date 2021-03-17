using MerchantExpanse.SpaceTraders.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;

namespace MerchantExpanse.SpaceTraders.Extensions
{
	public static class RestResponseExtensions
	{
		public static T DeserializeContent<T>(this IRestResponse response, string propertyName)
		{
			var jobject = JObject.Parse(response.Content);

			return response.StatusCode switch
			{
				HttpStatusCode.OK => jobject[propertyName].ToObject<T>(),
				HttpStatusCode.NotFound => throw new KeyNotFoundException(GetError(jobject)),
				HttpStatusCode.Unauthorized => throw new AuthenticationException(GetError(jobject)),
				_ => throw new InvalidOperationException(GetError(jobject)),
			};
		}

		private static string GetError(JObject response)
		{
			return response["error"].ToObject<Error>().Message;
		}
	}
}