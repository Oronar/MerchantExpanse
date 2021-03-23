using System.Collections.Generic;

namespace MerchantExpanse.SpaceTraders.Models
{
	public class Structure
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public bool Completed { get; set; }

		public IEnumerable<Material> Materials { get; set; }
	}
}