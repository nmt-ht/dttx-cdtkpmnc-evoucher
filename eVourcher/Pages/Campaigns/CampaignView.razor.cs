using eVoucher.Models;
using eVoucher.Pages.Campaigns.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVoucher.Pages.Campaigns;
public partial class CampaignView : ComponentBase
{
    [Inject] public ICampaignService CampaignService { get; set; }
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
 
    private Campaign selectedCampaign;
    private AddEditCampaignModal addEditCampaignModal;

    protected override async Task OnInitializedAsync()
    {
        Campaigns = await CampaignService.GetCampaigns();
        StateHasChanged();
    }

    private void ViewAddEditCampaign()
    {
        addEditCampaignModal.InitData();
    }
}