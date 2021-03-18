using MerchantExpanse.SpaceTraders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Contracts
{
	public interface IClient
	{
		Task<User> GetUserAsync();

		Task<IEnumerable<Loan>> GetLoansAsync();

		Task<IEnumerable<AvailableLoan>> GetAvailableLoansAsync();

		Task<User> TakeOutLoanAsync(string type);

		Task<Ship> GetShipAsync(string shipId);

		Task<IEnumerable<Ship>> GetShipsAsync();

		Task<IEnumerable<AvailableShip>> GetAvailableShipsAsync(string shipClass = null);

		Task<User> PurchaseShipAsync(string location, string type);

		Task ScrapShipAsync(string shipId);

		Task<IEnumerable<StarSystem>> GetSystemsAsync();

		Task<IEnumerable<Location>> GetSystemLocations(string systemSymbol);

		Task<LocationDetail> GetLocationAsync(string locationSymbol);

		Task<LocationDetail> GetLocationShipsAsync(string locationSymbol);

		Task<MarketLocation> GetMarketplaceAsync(string locationSymbol);

		Task<Order> PurchaseGood(string shipId, string good, int quantity);

		Task<Order> SellGood(string shipId, string good, int quantity);
	}
}