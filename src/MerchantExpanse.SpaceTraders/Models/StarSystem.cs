using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public partial class StarSystem
	{
		public string Symbol { get; set; }

		public string Name { get; set; }

		public IEnumerable<Location> Locations { get; set; }
	}
}