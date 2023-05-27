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
    [Parameter] public EventCallback OnUpdatePartner { get; set; }
    private Validations validationsRef;
    public Partner Partner { get; set; } = new Partner();
    
    private Modal modalRef;
    private bool IsAdded => Partner is not null && Partner.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Partner" : "Edit Partner";

    public async void InitData(Partner partner)
    {
        Partner = partner;
        await validationsRef.ClearAll();
        await ShowModal();
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
        if (IsAdded)
        {
            var result = await PartnerService.CreatePartner(Partner);
            if (result)
            {
                await NotificationService.Info("Added Partner successfully.");
                await HideModal();
                await OnUpdatePartner.InvokeAsync();
            }
            else
            {
                await NotificationService.Info("An error occurred please try again.");
            }
        }
        else
        {
            var result = await PartnerService.UpdatePartner(Partner);
            if (result)
            {
                await NotificationService.Info("Added Partner successfully.");
                await HideModal();
                await OnUpdatePartner.InvokeAsync();
            }
            else
            {
                await NotificationService.Info("An error occurred please try again.");
            }    
        }
    }
}