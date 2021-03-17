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
	}
}