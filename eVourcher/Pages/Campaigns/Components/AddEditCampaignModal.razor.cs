using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Games.Components;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Game = eVoucher.Models.Game;


namespace eVoucher.Pages.Campaigns.Components;
public partial class AddEditCampaignModal : ComponentBase
{
    [Inject] public ICampaignService CampaignService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public Campaign Campaign { get; set; } = new();
    [Parameter] public Guid CurrentUserId { get; set; }

    /// <summary>
    /// Event callback, it means the child component (AddEditCampaignModal) would like to reload data for the list view (datagrid)
    /// but the current data grid is from the parent component (CampaignView), hence, we have to use event callback to trigger or call
    /// from child to parent for re-loading data.
    /// </summary>
    [Parameter] public EventCallback<bool> ReloadData { get; set; }
    
    private Modal campaignRef;
    private bool IsAdded = false;
    private string Title => IsAdded ? "Add Campaign" : "Edit Campaign";

      
    private Game SelectedGame { get; set; }

    private AddEditAddresModal addEditGameModal;

    public void SetParameters(Campaign campaign, bool isAdded)
    {
        Campaign = campaign;
        IsAdded = isAdded;
    }
    public void InitData()
    {
        if (!IsAdded)
        {
            
        }
        else
        {
            Campaign = new();
        }

        ShowModal();
    }

    private Task ShowModal()
    {
        return campaignRef.Show();
    }

    private Task HideModal()
    {
        return campaignRef.Hide();
    }

    private async Task UpdateData()
    {
        if(IsAdded)
        {
            Campaign.CreatedBy = Campaign.ModifiedBy = CurrentUserId;
            var result = await CampaignService.UpdateCampaign(Campaign);
            if (result)
                await NotificationService.Info(IsAdded ? "Added Campaign successfully." : "Edit Campaign successfully.");
            await HideModal();
        }
    }

    private bool IsEditingGame = false;
    private async Task GameActions(eAction action)
    {
        IsEditingGame = false;
        switch (action)
        {
            case eAction.Add:
                addEditGameModal.SetParameters(new Game());
                addEditGameModal.InitData();
                break;
            case eAction.Edit:
                EditGame();
                break;
            case eAction.Delete:
                await DeleteGame();
                break;
        }
    }

    private void EditGame()
    {
        IsEditingGame = true;
        addEditGameModal.SetParameters(SelectedGame);
        addEditGameModal.InitData();
    }

    private async Task DeleteGame()
    {
        if (SelectedGame != null && SelectedGame.ID == System.Guid.Empty)// Delete on memory only
        {
            var deletedGame = Campaign.Games.FirstOrDefault(x => x.Index == SelectedGame.Index);
            if (deletedGame != null) { Campaign.Games.Remove(deletedGame); }
        }
        else
        {
            await GameService.DeleteGame(SelectedGame.ID);

            //Reload data on memory
            var deletedGame = Campaign.Games.FirstOrDefault(x => x.ID == SelectedGame.ID);
            if (deletedGame != null) { Campaign.Games.Remove(deletedGame); }
        }
    }

    private async Task OnUpdateGameCallBack(Game game)
    {
        if (!IsEditingGame)
        {
            var maxIndex = Campaign.Games.Count + 1;
            game.Index = maxIndex;
            Campaign.Games.Add(game);
        }
        else
        {
            var editGame = Campaign.Games.Where(x => x.ID == game.ID).FirstOrDefault();
            if (editGame is not null)
                UCampaign.Games.Remove(editGame);

            Campaign.Games.Add(game);

            // Update Address to database
            await CampaignService.UpdateGame(game);
        }
        StateHasChanged();
    }
}