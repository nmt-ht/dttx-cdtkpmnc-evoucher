using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Partners.Components;
public partial class AddEditPartnerModal : ComponentBase
{
    [Inject] public IPartnerService PartnerService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public Partner Partner { get; set; } = new();
    
    private Modal modalRef;
    private bool IsAdded => Partner is not null && Partner.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Partner" : "Edit Partner";

    public void InitData(Partner partner)
    {
        Partner = partner;
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
            var result = await PartnerService.CreatePartner(Partner);
            if (result)
                await NotificationService.Info("Added Partner successfully.");

            await HideModal();
        }
        else
        {
            var result = await PartnerService.UpdatePartner(Partner);
            if (result)
                await NotificationService.Info("Edit Partner successfully.");

            await HideModal();
        }
    }
}