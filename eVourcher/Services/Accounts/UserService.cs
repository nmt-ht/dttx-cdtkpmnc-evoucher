using eVoucher.Handlers;
using eVoucher.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public async Task<IList<User>> GetUsers()
        {
            IList<User> users = new List<User>();

            string requestURL = "/api/users";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                users = JsonConvert.DeserializeObject<IList<User>>(response.Data.ToString());
            }

            return users;
        }
        public Task<bool> CreateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
