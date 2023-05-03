using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetUsers();
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
    }
}
