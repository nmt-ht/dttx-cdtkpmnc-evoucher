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
            IList<Campaign> campaigns = new List<Campaign>();

            string requestURL = "/api/Campaigns";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                campaigns = JsonConvert.DeserializeObject<IList<Campaign>>(response.Data.ToString());
            }

            return campaigns;
        }
        public Task<bool> CreateCampaign(Campaign campaign)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteCampaign(Campaign campaign)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateCampaign(Campaign campaign)
        {
            string requestURL = "/api/campaigns/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, campaign);

            if (response != null && response.Success && response.Data != null)
            {
                return true;
            }
            return false;
        }
    }
}
