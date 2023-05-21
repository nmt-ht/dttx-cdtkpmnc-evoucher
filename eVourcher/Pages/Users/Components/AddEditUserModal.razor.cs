using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Address = eVoucher.Models.Address;

namespace eVoucher.Pages.Users.Components;
public partial class AddEditUserModal : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public User User { get; set; } = new();
    
    private Modal modalRef;
    private Address BillToAddress { get; set; } = new();
    private bool IsAdded => User is not null && User.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add User" : "Edit User";
    
    public void InitData()
    {
        if (!IsAdded)
        {
            BillToAddress = User?.Addresses?.FirstOrDefault(x => x.Type == eAddressType.BillTo);
        }
        {
            User = new();
            BillToAddress = new();
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
            User.Addresses.Add(BillToAddress);
            var result = await UserService.UpdateUser(User);
            if (result)
                await NotificationService.Info(IsAdded ? "Added user successfully." : "Edit user successfully.");
        }
    }
}