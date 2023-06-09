﻿using eVoucher.Handlers;
using eVoucher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Partner = eVoucher.Models.Partner;

namespace eVourcher.Services
{
    public class PartnerService : IPartnerService
    {
        public PartnerService() { }

        public async Task<IList<Partner>> GetPartners()
        {
            IList<Partner> partners = new List<Partner>();

            string requestURL = "/api/partners";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                partners = JsonConvert.DeserializeObject<IList<Partner>>(response.Data.ToString());
            }

            return partners;
        }
        public async Task<bool> CreatePartner(Partner partner)
        {
            var result = false;
            string requestURL = "/api/partners/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, partner);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        } 

        public async Task<bool> DeletePartner(Guid id)
        {
            var result = false;

            string requestURL = $"api/partners/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(),out result);
            }
            return result;
        }

        public async Task<bool> UpdatePartner(Partner partner)
        {
            var result = false;
            string requestURL = "/api/partners/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, partner);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            } 
            return result;
        }

        public async Task<Partner> GetPartnerById(Guid id)
        {
            var partner = new Partner();
            string requestURL = $"/api/partners/partners/{id}";
            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                partner = JsonConvert.DeserializeObject<Partner>(response.Data.ToString());
            }

            return partner;
        }
    }
}
