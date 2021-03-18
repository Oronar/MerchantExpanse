namespace MerchantExpanse.SpaceTraders.Models
{
	public class Order
	{
		public string Good { get; set; }

		public int Quantity { get; set; }

		public int PricePerUnit { get; set; }

		public int Total { get; set; }

		public int Credits { get; set; }
	}
}