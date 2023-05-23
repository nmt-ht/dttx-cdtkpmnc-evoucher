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
public partial class AddEditCampaignModal : ComponentBase
{
    [Inject] public ICampaignService CampaignService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public Campaign Campaign { get; set; } = new();
    
    private Modal modalRef;
    private bool IsAdded => Campaign is not null && Campaign.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Campaign" : "Edit Campaign";

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
        return modalRef.Show();
    }

    private Task HideModal()
    {
        return modalRef.Hide();
    }

    private async Task UpdateData()
    {
        if(IsAdded)
        {
            Campaign.CreatedBy = Guid.Parse("7CF80730-A4C2-4A55-96D0-811F549947C6");
            Campaign.ModifiedBy = Guid.Parse("7CF80730-A4C2-4A55-96D0-811F549947C6");
            var result = await CampaignService.UpdateCampaign(Campaign);
            if (result)
                await NotificationService.Info(IsAdded ? "Added Campaign successfully." : "Edit Campaign successfully.");
        }
    }
}