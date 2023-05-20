using eVoucher.Handlers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Campaign = eVoucher.Models.Campaign;

namespace eVourcher.Services
{
    public class CampaignService : ICampaignService
    {
        public CampaignService() { }

        public async Task<IList<Campaign>> GetCampaigns()
        {
            IList<Campaign> users = new List<Campaign>();

            string requestURL = "/api/Campaigns";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                users = JsonConvert.DeserializeObject<IList<Campaign>>(response.Data.ToString());
            }

            return users;
        }
        public Task<bool> CreateCampaign(Campaign user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteCampaign(Campaign user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateCampaign(Campaign user)
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
