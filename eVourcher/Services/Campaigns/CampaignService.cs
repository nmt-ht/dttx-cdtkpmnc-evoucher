using eVoucher.Handlers;
using Newtonsoft.Json;
using System;
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

            string requestURL = "/api/campaigns";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                campaigns = JsonConvert.DeserializeObject<IList<Campaign>>(response.Data.ToString());
            }

            return campaigns;
        }
        public Task<Campaign> CreateCampaign(Campaign campaign)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteCampaign(Guid id)
        {
            string requestURL = $"api/campaigns/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCampaign(Campaign campaign)
        {
            string requestURL = "/api/campaigns/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, campaign);

            if (response != null && response.Success)
            {
                return true;
            }
            return false;
        }

        public async Task<Campaign> GetCampaignById(Guid id)
        {
            var campaign = new Campaign();
            string requestURL = $"/api/campaigns/{id}";
            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                campaign = JsonConvert.DeserializeObject<Campaign>(response.Data.ToString());
            }

            return campaign;
        }

        public async Task<bool> DeleteGame(Guid id)
        {
            string requestURL = $"api/campaigns/game/delete/{id}";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateGame(eVoucher.Models.Game game)
        {
            string requestURL = "api/users/game/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, game);

            if (response is not null && response.Success)
            {
                return true;
            }
            return false;
        }
    }
}
