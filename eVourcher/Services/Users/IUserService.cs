using eVoucher.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface IUserService
    {
        Task<User> Login(string email, string passWord);
        Task<IList<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task<Tuple<User, string>> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);
        Task<bool> DeleteAddress(Guid id);
        Task<bool> UpdateAddess(Address address);
        Task<IList<UserGroup>> GetUserGroups();
        Task<IList<UserVoucher>> GetUserVouchers(Guid currentUserID);
    }
}
