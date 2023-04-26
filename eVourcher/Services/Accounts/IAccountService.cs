using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IAccountService
    {
        Task<IList<Account>> GetAccounts();
    }
}
