using MerchantExpanse.SpaceTraders.Models;
using System;

namespace MerchantExpanse.SpaceTraders.Exceptions
{
	public class ApiException : Exception
	{
		public Error Error { get; set; }

		public ApiException(Error error) : base($"({error.Code}) {error.Message}")
		{
			Error = error;
		}
	}
}