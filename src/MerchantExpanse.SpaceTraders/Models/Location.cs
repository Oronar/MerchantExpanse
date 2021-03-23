using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class Location
	{
		public string Symbol { get; set; }

		public string Type { get; set; }

		public string Name { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public IEnumerable<ShipPlate> Ships { get; set; }

		public string Anomaly { get; set; }

		public IEnumerable<string> Messages { get; set; }
	}

	public class LocationDetail : Location
	{
		public decimal AnsibleProgress { get; set; }

		public int DockedShips { get; set; }

		public IEnumerable<Structure> Structures { get; set; }
	}

	public class MarketLocation : Location
	{
		public IEnumerable<Good> Marketplace { get; set; }
	}
}