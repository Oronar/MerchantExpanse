using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class Error
	{
		public string Message { get; set; }

		public int Code { get; set; }

		public ErrorData Data { get; set; }
	}

	public class ErrorData
	{
		public IEnumerable<string> Type { get; set; }
	}
}