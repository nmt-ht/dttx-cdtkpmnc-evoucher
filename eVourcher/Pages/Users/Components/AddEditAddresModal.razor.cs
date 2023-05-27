using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Address = eVoucher.Models.Address;

namespace eVoucher.Pages.Users.Components;
public partial class AddEditAddresModal : ComponentBase
{
    private Address Address { get; set; } = new();
    [Parameter] public EventCallback<Address> UpdateAddressCallBack { get; set; }
    private eAction Action;
    private IList<eAddressType> AddressTypes = new List<eAddressType>() { eAddressType.ShipTo, eAddressType.BillTo, eAddressType.BillToShipTo, eAddressType.Company };
    private string Title => Action == eAction.Add ? "Add Address" : "Edit Address";
    private Modal addressRef;
    Validations validations;
    public void SetParameters(Address address, eAction action = eAction.Add)
    {
        Action = action;
        Address = address;
    }
    public async void InitData()
    {
        await validations.ClearAll();
        StateHasChanged();
        await ShowModal();
    }

    private Task ShowModal()
    {
        return addressRef.Show();
    }

    private Task HideModal()
    {
        return addressRef.Hide();
    }

    private async Task UpdateData()
    {
        if(await validations.ValidateAll())
        {
            await UpdateAddressCallBack.InvokeAsync(Address);
            await HideModal();
        }
    }
}