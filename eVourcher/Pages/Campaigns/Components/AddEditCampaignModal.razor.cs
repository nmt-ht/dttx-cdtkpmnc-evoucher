using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

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

        await ReloadData.InvokeAsync(true);
    }
}