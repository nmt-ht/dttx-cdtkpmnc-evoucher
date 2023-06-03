using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Game = eVoucher.Models.Game;


namespace eVoucher.Pages.Campaigns.Components;
public partial class AddEditCampaignModal : ComponentBase
{
    private Validations validationsRef;
    [Inject] public ICampaignService CampaignService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }
    [Parameter] public Campaign Campaign { get; set; } = new();
    [Parameter] public Guid? CurrentUserId { get; set; }

    /// <summary>
    /// Event callback, it means the child component (AddEditCampaignModal) would like to reload data for the list view (datagrid)
    /// but the current data grid is from the parent component (CampaignView), hence, we have to use event callback to trigger or call
    /// from child to parent for re-loading data.
    /// </summary>
    [Parameter] public EventCallback OnUpdateCampaign { get; set; }
    
    private Modal campaignRef;
    private bool IsAdded = false;
    private string Title => IsAdded ? "Add Campaign" : "Edit Campaign";

      
    private CampaignGame SelectedCampaignGame { get; set; }

    private AddEditGameCampaignModal addEditGameModal;
    public IList<Guid> GameIds => Campaign != null && Campaign.CampaignGames != null ? Campaign.CampaignGames.Select(x => x.Game.ID).ToList() : new List<Guid>();

    public void SetParameters(Campaign campaign, bool isAdded, Guid? currentUserId)
    {
        Campaign = campaign;      
        IsAdded = isAdded;
        CurrentUserId = currentUserId;
        if(IsAdded)
        {
            Campaign.StartedDate = DateTime.Now;
            Campaign.ExpiredDate = DateTime.Now;
        }    
    }
    public async void InitData()
    {
        ImageDataUrl = GetImageDataUrlFromBytes(Campaign.Image);
        await validationsRef.ClearAll();
        await ShowModal();
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
        if (await validationsRef.ValidateAll())
        {
            if (IsAdded)
            {
                Campaign.CreatedBy = CurrentUserId != null ? CurrentUserId.Value : Guid.Empty;
                Campaign.ModifiedBy = CurrentUserId != null ? CurrentUserId.Value : Guid.Empty;
                var result = await CampaignService.CreateCampaign(Campaign);
                if (result)
                {
                    await NotificationService.Info("Added Campaign successfully.");
                    await HideModal();
                    await OnUpdateCampaign.InvokeAsync();
                }
                else
                {
                    await NotificationService.Info("An error occurred please try again.");
                }
            }
            else
            {

                var result = await CampaignService.UpdateCampaign(Campaign);
                if (result)
                {
                    Campaign.ModifiedBy = CurrentUserId != null ? CurrentUserId.Value : Guid.Empty;
                    await NotificationService.Info("Updated campaign successfully.");
                    await HideModal();
                    await OnUpdateCampaign.InvokeAsync();
                }
                else
                {
                    await NotificationService.Info("An error occurred please try again.");
                }
            }
        }
    }

    private bool IsEditingGame = false;
    private async Task GameActions(eAction action)
    {
        IsEditingGame = false;
        switch (action)
        {
            case eAction.Add:
                addEditGameModal.SetParameters(new CampaignGame(), true, GameIds);
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
        addEditGameModal.SetParameters(SelectedCampaignGame, false, GameIds);
        addEditGameModal.InitData();
    }

    private async Task DeleteGame()
    {
        var confirm = await MessageService.Confirm("Are you sure delete this game?");
        if(confirm) 
        {
            if (SelectedCampaignGame != null)// Delete on memory only
            {
                Campaign.CampaignGames.Remove(SelectedCampaignGame);
                if (Campaign.CampaignGames is not null && Campaign.CampaignGames.Any())
                {
                    var index = 0;
                    Campaign.CampaignGames.ToList().ForEach(a => a.Index = ++index);
                }
            }
        }
    }

    private async Task OnUpdateGameCallBack(CampaignGame campaignGame)
    {
        if (!IsEditingGame)
        {
            var maxIndex = Campaign.CampaignGames.Count + 1;
            campaignGame.Index = maxIndex;
            Campaign.CampaignGames.Add(campaignGame);
        }
        else
        {
            SelectedCampaignGame = campaignGame;
        }
        StateHasChanged();
    }

    protected string ImageDataUrl { get; set; }
    async Task OnFileUpload(FileUploadEventArgs e)
    {
        try
        {
            using (var stream = new MemoryStream())
            {
                await e.File.WriteToStreamAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                // Convert the image stream to a base64-encoded data URL
                ImageDataUrl = await GetImageDataUrl(stream);
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }
        finally
        {
            this.StateHasChanged();
        }
    }
    private string GetImageDataUrlFromBytes(byte[] imageBytes)
    {
        if (imageBytes is not null && imageBytes.Length > 0)
        {
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var imageDataUrl = $"data:image/png;base64,{imageBase64}";
            return imageDataUrl;
        }
        return string.Empty;
    }
    private async Task<string> GetImageDataUrl(Stream imageStream)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Copy the image stream to the memory stream
            await imageStream.CopyToAsync(memoryStream);

            // Convert the image bytes to a base64-encoded data URL
            var imageBytes = memoryStream.ToArray();
            Campaign.Image = imageBytes;
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var imageDataUrl = $"data:image/png;base64,{imageBase64}";

            return imageDataUrl;
        }
    }
    void ValidateStartedDate(ValidatorEventArgs e)
    {
        if (e.Value == null)
            e.Status = ValidationStatus.Error;
        else
        {
            var startedDate = DateTime.Parse(e.Value.ToString());
            e.Status = startedDate.Date.Equals(DateTime.MinValue.Date) || (IsAdded && startedDate.Date < DateTime.Now.Date) || (Campaign.ExpiredDate != null && Campaign.ExpiredDate < startedDate.Date) ? ValidationStatus.Error : ValidationStatus.Success;
        }
    }
    void ValidateExpiredDate(ValidatorEventArgs e)
    {
        if (e.Value == null)
            e.Status = ValidationStatus.Error;
        else
        {
            var expiredDate = DateTime.Parse(e.Value.ToString());
            e.Status = expiredDate.Date.Equals(DateTime.MinValue.Date) || (IsAdded && expiredDate.Date < DateTime.Now.Date) || (Campaign.StartedDate != null && Campaign.StartedDate > expiredDate.Date) ? ValidationStatus.Error : ValidationStatus.Success;
        }
    }
}