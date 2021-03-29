namespace MerchantExpanse.SpaceTraders.Models
{
	public class Good
	{
		public int QuantityAvailable { get; set; }

		public int VolumePerUnit { get; set; }

		public int PricePerUnit { get; set; }

		public string Symbol { get; set; }

		public int Spread { get; set; }

		public int PurchasePricePerUnit { get; set; }

		public int SellPricePerUnit { get; set; }
	}
}