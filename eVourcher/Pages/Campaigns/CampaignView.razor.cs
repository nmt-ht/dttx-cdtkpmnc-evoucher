using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Campaigns.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Campaigns;
public partial class CampaignView : ComponentBase
{
    [Inject] public ICampaignService CampaignService { get; set; }
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
 
    private Campaign selectedCampaign;
    private AddEditCampaignModal addEditCampaignModal;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        await ShowLoadingPage(true);
        Campaigns = await CampaignService.GetCampaigns();
        var index = 0;
        Campaigns.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }

    private void ViewAddEditCampaign()
    {
        addEditCampaignModal.InitData();
    }
    private async Task CampaignActions(eAction action)
    {
        switch (action)
        {
           case eAction.Add:
                addEditCampaignModal.SetParameters(new Campaign(), true);
                addEditCampaignModal.InitData();
                break;
            case eAction.Edit:
                var campaign = await CampaignService.GetCampaignById(selectedCampaign.ID);
                addEditCampaignModal.SetParameters(campaign, false);
                addEditCampaignModal.InitData();
                break;
            case eAction.Delete:
                break;
            default:
                break;
        }
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