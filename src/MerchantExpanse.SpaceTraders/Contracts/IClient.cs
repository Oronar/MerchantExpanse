using MerchantExpanse.SpaceTraders.Models;
using System.Threading.Tasks;

namespace MerchantExpanse.SpaceTraders.Contracts
{
	public interface IClient
	{
		Task<User> GetUserAsync();
	}
}