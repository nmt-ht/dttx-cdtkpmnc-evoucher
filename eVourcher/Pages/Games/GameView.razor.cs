using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Games.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games;
public partial class GameView : ComponentBase
{
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }
    [Inject] public IGameService GameService { get; set; }
    private IList<Game> Games { get; set; } = new List<Game>();

    private Game selectedGame;
    private AddEditGameModal addEditGameModal;
    private Guid? userID;

    protected override async Task OnInitializedAsync()
    {
        await BindData();
    }

    public async Task BindData()
    {
        await GetUserID();
        await ShowLoadingPage(true);
        Games = await GameService.GetGames();
        var index = 0;
        Games.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false); 
    }

    private async Task GetUserID()
    {
        var userIDString = await LocalStorage.GetStringAsync("userId");

        userID = string.IsNullOrEmpty(userIDString) ? null : new Guid(userIDString);
    }
    
    private void ViewAddGame()
    {
        Game Game = new Game();
        Game.CreatedBy = userID != null ? userID.Value : Guid.Empty;
        addEditGameModal.InitData(Game, userID);
    }

    private void ViewEditGame()
    {
        if (selectedGame != null && selectedGame.ID != Guid.Empty)
        {
            addEditGameModal.InitData(selectedGame, userID);
        }
    }

    private async Task OnDeleteGame()
    {
        var confirm = await MessageService.Confirm("Are you sure delete this Game?");
        if (confirm) 
        {
            var result = await GameService.DeleteGame(selectedGame.ID);

            if (result)
            {
                await NotificationService.Info("Delete successfully.");
                await BindData();
            }
            else
            {
                await NotificationService.Error("An error occurred please try again.");
            }
        }
    }

    private async Task OnUpdateGame()
    {
        await BindData();
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
        StateHasChanged();
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