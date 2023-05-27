using eVoucher.Handlers;
using Newtonsoft.Json;
using System;
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
        public async Task<bool> DeleteUser(Guid id)
        {
            string requestURL = $"api/users/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateUser(User user)
        {
            string requestURL = "api/users/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, user);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
        public async Task<User> GetUserById(Guid id)
        {
            var user = new User();
            string requestURL = $"/api/users/{id}";
            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                user = JsonConvert.DeserializeObject<User>(response.Data.ToString());
            }

            return user;
        }
        public async Task<bool> DeleteAddress(Guid id)
        {
            string requestURL = $"api/users/address/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateAddess(eVoucher.Models.Address address)
        {
            string requestURL = "api/users/address/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, address);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
    }
}
