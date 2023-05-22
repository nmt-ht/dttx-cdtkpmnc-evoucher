using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Address = eVoucher.Models.Address;

namespace eVoucher.Pages.Users.Components;
public partial class AddEditAddresModal : ComponentBase
{
    private Address Address { get; set; }
    [Parameter] public EventCallback<Address> UpdateAddressCallBack { get; set; }
    private bool IsAdded => Address is not null && Address.Id != Guid.Empty ? false : true;
    private IList<eAddressType> AddressTypes = new List<eAddressType>() { eAddressType.ShipTo, eAddressType.BillTo, eAddressType.BillToShipTo, eAddressType.Company };
    private string Title => IsAdded ? "Add Address" : "Edit Address";
    private Modal addressRef;
    
    public void SetParameters(Address address)
    {
        Address = address;
    }
    public void InitData()
    {
        Address = IsAdded ? new() : Address;
        StateHasChanged();
        ShowModal();
    }

    private Task ShowModal()
    {
        return addressRef.Show();
    }

    private Task HideModal()
    {
        return addressRef.Hide();
    }

    private void UpdateData()
    {
        UpdateAddressCallBack.InvokeAsync(Address);
        HideModal();
    }
}