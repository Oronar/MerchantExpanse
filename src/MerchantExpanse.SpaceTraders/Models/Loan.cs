using System;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class Loan : LoanBase
	{
		public string Id { get; set; }

		public DateTime Due { get; set; }

		public int RepaymentAmount { get; set; }

		public string Status { get; set; }
	}

	public class AvailableLoan : LoanBase
	{
		public int Amount { get; set; }

		public int Rate { get; set; }

		public int TermInDays { get; set; }

		public bool CollateralRequired { get; set; }
	}

	public abstract class LoanBase
	{
		public string Type { get; set; }
	}
}