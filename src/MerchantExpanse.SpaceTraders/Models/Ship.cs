using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class Ship : ShipBase
	{
		public string Id { get; set; }

		public string Location { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public IEnumerable<Cargo> Cargo { get; set; }

		public int SpaceAvailable { get; set; }
	}

	public abstract class ShipBase
	{
		public string Type { get; set; }

		public string Class { get; set; }

		public int MaxCargo { get; set; }

		public int Speed { get; set; }

		public string Manufacturer { get; set; }

		public int Plating { get; set; }

		public int Weapons { get; set; }
	}

	public class AvailableShip : ShipBase
	{
		public IEnumerable<Purchaselocation> PurchaseLocations { get; set; }
	}

	public class Purchaselocation
	{
		public string Location { get; set; }
		public int Price { get; set; }
	}
}