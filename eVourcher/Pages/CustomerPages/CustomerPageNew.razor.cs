using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Campaigns.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.CustomerPages
{
    public class CustomerPage
    {
        #region External services
        /// <summary>
        /// Inject campaign service is to send a request to API
        /// </summary>
        [Inject] public ICampaignService GameService { get; set; }
        /// <summary>
        /// It is using for getting the data which saved to local storage.
        /// Ref: https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage
        /// </summary>
        [Inject] public ILocalStorage LocalStorage { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IMessageService MessageService { get; set; }
        #endregion

        private Guid? CurrentUserId { get; set; }
        private IList<Game> Games { get; set; } = new List<Game>();
        private Game selectedGame;


        public async Task InitData()
        {
            var userID = await LocalStorage.GetStringAsync("userId");
            if (!string.IsNullOrEmpty(userID))
                CurrentUserId = Guid.Parse(userID);
            await LoadData();
        }
        private async Task LoadData()
        {
            await ShowLoadingPage(true);
            Games = await GameService.GetGames();
            var index = 0;
            Games.ToList().ForEach(x => x.Index = ++index);
            await ShowLoadingPage(false);
        }

        private async Task OnUpdateCampaign()
        {
            await LoadData();
        }


        #region Show Loading page
        [Inject] IPageProgressService PageProgressService { get; set; }
        private async Task ShowLoadingPage(bool isShow)
        {
            if (isShow)
            {
                await SetPageProgressIndeterminate();
            }
            else { await SetPageProgressHidden(); }
// StateHasChanged();
        }
        Task SetPageProgressIndeterminate()
        {
            return PageProgressService.Go(null, options => { options.Color = Color.Info; });
        }
        Task SetPageProgressHidden()
        {
            // setting it to -1 will hide the progress bar
            return PageProgressService.Go(-1);
        }
        #endregion
    }
}