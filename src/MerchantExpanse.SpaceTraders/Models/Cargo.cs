namespace MerchantExpanse.SpaceTraders.Models
{
	public class Cargo
	{
		public string Good { get; set; }

		public int Quantity { get; set; }

		public int TotalVolume { get; set; }
	}

	public class JettisonedCargo
	{
		public string Good { get; set; }

		public int QuantityRemaining { get; set; }

		public string ShipId { get; set; }
	}
}