using eVoucher.Handlers;
using eVoucher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Game = eVoucher.Models.Game;

namespace eVourcher.Services
{
    public class GameService : IGameService
    {
        public GameService() { }

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
        public async Task<bool> CreateGame(Game game)
        {
            var result = false;
            string requestURL = "/api/games/create";

            var response = await RestClient.APIClient.PostAsync(requestURL, game);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        } 
        public async Task<bool> DeleteGame(Guid id)
        {
            var result = false;

            string requestURL = $"api/games/{id}/delete";

            var response = await RestClient.APIClient.DeleteAsync(requestURL);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(),out result);
            }
            return result;
        }
        public async Task<bool> UpdateGame(Game game)
        {
            var result = false;
            string requestURL = "/api/games/update";

            var response = await RestClient.APIClient.PostAsync(requestURL, game);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            } 
            return result;
        }
        public async Task<bool> CreateVoucher(Voucher voucher)
        {
            var result = false;
            string requestURL = "/api/games/voucher/create";
            var response = await RestClient.APIClient.PostAsync(requestURL, voucher);

            if (response != null && response.Success && response.Data != null)
            {
                bool.TryParse(response.Data.ToString(), out result);
            }
            return result;
        }
    }
}
