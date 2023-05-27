using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Partners.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages.Partners;
public partial class PartnerView : ComponentBase
{
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }
    [Inject] public IPartnerService PartnerService { get; set; }
    private IList<Partner> Partners { get; set; } = new List<Partner>();

    private Partner selectedPartner;
    private AddEditPartnerModal addEditPartnerModal;
    private Guid? userID;

    protected override async Task OnInitializedAsync()
    {
        await GetUserID();
        await BindData();
    }

    private async Task BindData()
    {
        await ShowLoadingPage(true);
        Partners = await PartnerService.GetPartners();
        var index = 0;
        Partners.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false); 
    }

    private async Task GetUserID()
    {
        var userIDString = await LocalStorage.GetStringAsync("userId");

        userID = string.IsNullOrEmpty(userIDString) ? null : new Guid(userIDString);

    }
    
    private void ViewAddPartner()
    {
        Partner partner = new Partner();
        partner.User_ID_FK = userID != null ? userID.Value : Guid.Empty;
        addEditPartnerModal.InitData(partner);
    }

    private void ViewEditPartner()
    {
        if (selectedPartner != null && selectedPartner.Id != Guid.Empty)
        {
            addEditPartnerModal.InitData(selectedPartner);
        }
    }

    private async Task OnDeletePartner()
    {
        var confirm = await MessageService.Confirm("Are you sure delete this partner?");
        if (confirm) 
        {
            var result = await PartnerService.DeletePartner(selectedPartner.Id);

            if (result)
            {
                await NotificationService.Info("Delete successfully.");
                await BindData();
            }
            else
            {
                await NotificationService.Info("An error occurred please try again.");
            }
        }
    }

    private async Task OnUpdatePartner()
    {
        await BindData();
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