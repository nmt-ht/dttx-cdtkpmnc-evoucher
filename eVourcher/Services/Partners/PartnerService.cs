using eVoucher.Handlers;
using Newtonsoft.Json;
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
            IList<Partner> users = new List<Partner>();

            string requestURL = "/api/Partners";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                users = JsonConvert.DeserializeObject<IList<Partner>>(response.Data.ToString());
            }

            return users;
        }
        public Task<bool> CreatePartner(Partner user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeletePartner(Partner user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdatePartner(Partner user)
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
