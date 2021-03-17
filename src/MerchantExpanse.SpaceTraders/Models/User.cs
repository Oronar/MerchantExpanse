using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class User
	{
		public string Username { get; set; }

		public int Credits { get; set; }

		public IEnumerable<Ship> Ships { get; set; }

		public IEnumerable<Loan> Loans { get; set; }
	}
}