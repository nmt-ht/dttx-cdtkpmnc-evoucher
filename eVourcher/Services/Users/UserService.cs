﻿using eVoucher.Handlers;
using eVoucher.Models;
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
        public event Action<User> CurrentUserChanged;

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                CurrentUserChanged?.Invoke(currentUser);
            }
        }
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
        public async Task<Tuple<User, string>> CreateUser(User userRegister)
        {
            User user = null;
            if (userRegister != null)
            {
                string requestURL = "/api/users/create";

                var response = await RestClient.APIClient.PostAsync(requestURL, userRegister);

                if (response != null && response.Success)
                {                  
                    if(response.Data is null && !string.IsNullOrEmpty(response.Message))
                    {
                        return Tuple.Create(user, response.Message);
                    }

                    if(response.Data != null)
                        user = JsonConvert.DeserializeObject<User>(response.Data.ToString());

                    return Tuple.Create(user, string.Empty);
                }
            }
            return Tuple.Create(user, string.Empty);
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
        public async Task<bool> UpdateAddess(Address address)
        {
            string requestURL = "api/users/address/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, address);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
        public async Task<IList<UserGroup>> GetUserGroups()
        {
            IList<UserGroup> users = new List<UserGroup>();

            string requestURL = "/api/users/usergroups";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                users = JsonConvert.DeserializeObject<IList<UserGroup>>(response.Data.ToString());
            }

            return users;
        }
        public async Task<IList<UserVoucher>> GetUserVouchers(Guid currentUserID)
        {
            IList<UserVoucher> userVouchers = new List<UserVoucher>();

            string requestURL = $"/api/users/vouchers/{currentUserID}";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                userVouchers = JsonConvert.DeserializeObject<IList<UserVoucher>>(response.Data.ToString());
            }

            return userVouchers;
        }
    }
}
