using eVoucher.Handlers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = eVoucher.Models.User;

namespace eVourcher.Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public async Task<User> Login(string email, string passWord)
        {
            User userLogin = new User();
            userLogin.EmailAddress = email;
            userLogin.Password = passWord;

            string requestURL = "/api/users/login";

            var response = await RestClient.APIClient.PostAsync(requestURL, userLogin);

            if (response != null && response.Success && response.Data != null)
            {
                var user = JsonConvert.DeserializeObject<User>(response.Data.ToString());
                return user;
            }
            return null;
        }

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
        public async Task<User> CreateUser(User userRegister)
        {
            if (userRegister != null)
            {
                string requestURL = "/api/users/create";

                var response = await RestClient.APIClient.PostAsync(requestURL, userRegister);

                if (response != null && response.Success && response.Data != null)
                {
                    var user = JsonConvert.DeserializeObject<User>(response.Data.ToString());
                    return user;
                }
            }
            return null;
        }

        public Task<bool> DeleteUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateUser(User user)
        {
            string requestURL = "/api/users/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, user);

            if (response != null && response.Success && response.Data != null)
            {
                return true;
            }
            return false;
        }
    }
}
