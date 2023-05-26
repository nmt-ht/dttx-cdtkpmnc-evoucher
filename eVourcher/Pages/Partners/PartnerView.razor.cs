using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Partners.Components;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace eVoucher.Pages.Partners;
public partial class PartnerView : ComponentBase
{
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IPartnerService PartnerService { get; set; }
    private IList<Partner> Partners { get; set; } = new List<Partner>();

    private Partner selectedPartner;
    private AddEditPartnerModal addEditPartnerModal;
    private DeletePartnerModal deletePartnerModal;

    protected override async Task OnInitializedAsync()
    {
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
    
    private void ViewAddPartner()
    {
        addEditPartnerModal.InitData(new Partner());
    }

    private void ViewEditPartner()
    {
        addEditPartnerModal.InitData(selectedPartner);
    }

    private void ViewDeletePartner()
    {
        if (selectedPartner != null)
        {
            deletePartnerModal.InitData();
        }
    }

    private async Task OnDeletePartner()
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