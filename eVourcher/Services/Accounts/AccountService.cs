using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public class AccountService : IAccountService
    {
        public Task<IList<Account>> GetAccounts()
        {
            // connect to api to get data
            throw new System.NotImplementedException();
        }
    }
}
