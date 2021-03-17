using MerchantExpanse.SpaceTraders.Exceptions;
using MerchantExpanse.SpaceTraders.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace MerchantExpanse.SpaceTraders.Extensions
{
	public static class RestResponseExtensions
	{
		public static T DeserializeContent<T>(this IRestResponse response, string propertyName)
		{
			var jobject = JObject.Parse(response.Content);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				return jobject[propertyName].ToObject<T>();
			}

			throw new ApiException(GetError(jobject));
		}

		private static Error GetError(JObject response)
		{
			return response["error"].ToObject<Error>();
		}
	}
}