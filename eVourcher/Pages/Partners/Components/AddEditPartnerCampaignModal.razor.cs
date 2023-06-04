using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Campaign = eVoucher.Models.Campaign;

namespace eVoucher.Pages.Partners.Components;
public partial class AddEditPartnerCampaignModal : ComponentBase
{
    [Inject] public ICampaignService CampaignService { get; set; }
    private  PartnerCampaign PartnerCampaign { get; set; }
    [Parameter] public EventCallback<PartnerCampaign> UpdatePartnerCampaignCallBack { get; set; }
    private bool IsAdded { get; set; }
    private string Title => IsAdded ? "Add Campaign" : "Edit Campaign";
    private Modal campaignsRef;
    public IList<Campaign> Campaigns { get; set; }
    public IList<Guid> CampaignIds { get; set; } = new List<Guid>();
    public Campaign SelectedCampaign { get; set; }

    public async void SetParameters(PartnerCampaign partnerCampaign, bool isAdded = true, IList<Guid> campaignIds = null)
    {
        PartnerCampaign = partnerCampaign;
        IsAdded = isAdded;
        CampaignIds = campaignIds;
    }

    private async Task BindData()
    {
        await ShowLoadingPage(true);
        Campaigns = await CampaignService.GetCampaigns();
        Campaigns = Campaigns.Where(x => !CampaignIds.Contains(x.Id)).ToList();
        var index = 0;
        Campaigns.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }
    public async void InitData()
    {
        await BindData();
        await ShowModal();
    }

    private Task ShowModal()
    {
        return campaignsRef.Show();
    }

    private Task HideModal()
    {
        return campaignsRef.Hide();
    }

    private void UpdateData()
    {
        PartnerCampaign.Campaign = SelectedCampaign;
        UpdatePartnerCampaignCallBack.InvokeAsync(PartnerCampaign);
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