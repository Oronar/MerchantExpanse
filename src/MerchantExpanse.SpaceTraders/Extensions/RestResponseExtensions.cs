using MerchantExpanse.SpaceTraders.Exceptions;
using MerchantExpanse.SpaceTraders.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MerchantExpanse.SpaceTraders.Extensions
{
	public static class RestResponseExtensions
	{
		public static T DeserializeContent<T>(this IRestResponse response, string propertyName = null)
		{
			var jobject = JObject.Parse(response.Content);

			if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
			{
				return (jobject[propertyName ?? string.Empty] ?? jobject).ToObject<T>();
			}

			throw new ApiException(GetError(jobject));
		}

		private static Error GetError(JObject response)
		{
			return response["error"].ToObject<Error>();
		}
	}
}