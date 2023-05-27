using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Campaigns.Components;
public partial class DeletePartnerModal : ComponentBase
{
    private Modal deleteCampaignRef;
    [Parameter] public EventCallback OnDeleteCampaign { get; set; }

    public void InitData()
    {
        ShowModal();
    }

    private Task ShowModal()
    {
        return deleteCampaignRef.Show();
    }

    private Task HideModal()
    {
        return deleteCampaignRef.Hide();
    }
      
    private async Task DeletePartner()
    {
        await HideModal();
        await OnDeleteCampaign.InvokeAsync();
    }
}