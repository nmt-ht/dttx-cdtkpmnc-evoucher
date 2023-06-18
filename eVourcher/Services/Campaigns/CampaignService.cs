using eVoucher.Handlers;
using eVoucher.Models;
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
        public async Task<bool> CreateCampaign(Campaign campaign)
        {
            var result = false;
            string requestURL = "/api/campaigns/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, campaign);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
        public async Task<bool> DeleteCampaign(Guid id)
        {
            var result = false;
            string requestURL = $"api/campaigns/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
        public async Task<bool> UpdateCampaign(Campaign campaign)
        {
            var result = false;
            string requestURL = "/api/campaigns/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, campaign);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
        public async Task<Campaign> GetCampaignById(Guid id)
        {
            var campaign = new Campaign();
            string requestURL = $"/api/campaigns/campaigns/{id}";
            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                campaign = JsonConvert.DeserializeObject<Campaign>(response.Data.ToString());
            }

            return campaign;
        }
        public async Task<IList<Game>> GetGames()
        {
            IList<Game> games = new List<Game>();

            string requestURL = "/api/games";

            var response = await RestClient.APIClient.GetAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                games = JsonConvert.DeserializeObject<IList<Game>>(response.Data.ToString());
            }

            return games;
        }
        public async Task<bool> DeleteGame(Guid id)
        {
            var result = false;
            string requestURL = $"api/campaigns/game/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
        public async Task<bool> UpdateGame(eVoucher.Models.Game game)
        {
            var result = false;
            string requestURL = "api/users/game/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, game);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
        public async Task<bool> UpdateCampaignUser(Guid campaignId, Guid userId)
        {
            var result = false;
            string requestURL = "/api/campaigns/user/update";
            var gameUserDto = new CampaignUserDto
            {
                CampaignId = campaignId,
                UserId = userId
            };

            var response = await RestClient.APIClient.PostAsync(requestURL, gameUserDto);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
    }
}
