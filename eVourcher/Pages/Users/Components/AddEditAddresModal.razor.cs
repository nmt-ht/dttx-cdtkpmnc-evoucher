using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Address = eVoucher.Models.Address;

namespace eVoucher.Pages.Users.Components;
public partial class AddEditAddresModal : ComponentBase
{
    [Parameter] public Address Address { get; set; } = new();
    private bool IsAdded => Address is not null && Address.Id != Guid.Empty ? false : true;
    
    private string Title => IsAdded ? "Add Address" : "Edit Address";
    private Modal modalRef;
    
    public void InitData()
    {
        if (!IsAdded)
        {
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

    private void UpdateData()
    {

    }
}