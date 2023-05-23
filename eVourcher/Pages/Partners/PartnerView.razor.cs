using eVoucher.Models;
using eVoucher.Pages.Partners.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVoucher.Pages.Partners;
public partial class PartnerView : ComponentBase
{
    [Inject] public IPartnerService PartnerService { get; set; }
    private IList<Partner> Partners { get; set; } = new List<Partner>();
 
    private Partner selectedPartner;
    private AddEditPartnerModal addEditPartnerModal;

    protected override async Task OnInitializedAsync()
    {
        Partners = await PartnerService.GetPartners();
        StateHasChanged();
    }

    private void ViewAddPartner()
    {
        addEditPartnerModal.InitData(new Partner());
    }
     
    private void ViewEditPartner()
    {
        addEditPartnerModal.InitData(selectedPartner);
    }
}