using eVoucher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IUserService
    {
        Task<User> Login(string email, string passWord);
        Task<IList<User>> GetUsers();
        Task<User> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
    }
}
