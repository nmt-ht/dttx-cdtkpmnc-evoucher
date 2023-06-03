using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Game = eVoucher.Models.Game;

namespace eVoucher.Pages.Campaigns.Components;
public partial class AddEditGameCampaignModal : ComponentBase
{
    [Inject] public IGameService GameService { get; set; }
    private CampaignGame CampaignGame { get; set; }
    [Parameter] public EventCallback<CampaignGame> UpdateGameCallBack { get; set; }
    private bool IsAdded { get; set; }
    private string Title => IsAdded ? "Add Game" : "Edit Game";
    private Modal gameRef;
    public IList<Game> Games { get; set; }
    public IList<Guid> GameIds { get; set; } = new List<Guid>();
    public Game SelectedGame { get; set; }

    public async void SetParameters(CampaignGame campaignGame, bool isAdded = true, IList<Guid> gameIds = null)
    {
        CampaignGame = campaignGame;
        IsAdded = isAdded;
        GameIds = gameIds;
    }

    private async Task BindData()
    {
        await ShowLoadingPage(true);
        Games = await GameService.GetGames();
        Games = Games.Where(x => !GameIds.Contains(x.ID)).ToList();
        var index = 0;
        Games.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }
    public async void InitData()
    {
        await BindData();
        await ShowModal();
    }

    private Task ShowModal()
    {
        return gameRef.Show();
    }

    private Task HideModal()
    {
        return gameRef.Hide();
    }

    private void UpdateData()
    {
        CampaignGame.Game = SelectedGame;
        UpdateGameCallBack.InvokeAsync(CampaignGame);
        HideModal();
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