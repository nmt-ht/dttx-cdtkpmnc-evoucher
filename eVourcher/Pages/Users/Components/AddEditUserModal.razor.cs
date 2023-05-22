using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Address = eVoucher.Models.Address;
namespace eVoucher.Pages.Users.Components;
public partial class AddEditUserModal : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public EventCallback<bool> ReloadData { get; set; }
    private User User { get; set; }
    
    private Modal userRef;
    private bool IsAdded = false;
    private string Title => IsAdded ? "Add User" : "Edit User";
    private Address SelectedAddress { get; set; }

    private AddEditAddresModal addEditAddressModal;
   
    public void SetParameters(User user, bool isAdded)
    {
        User = user;
        IsAdded = isAdded;
    }
    public void InitData()
    {
        ShowModal();
    }

    private Task ShowModal()
    {
        return userRef.Show();
    }

    private Task HideModal()
    {
        return userRef.Hide();
    }

    private async Task UpdateData()
    {
        var result = false;
        if(IsAdded)
        {
            var user = await UserService.CreateUser(User);
            result = user is not null ? true : false;
        }
        else
            result = await UserService.UpdateUser(User);

        if (result)
            await NotificationService.Info(IsAdded ? "Added user successfully." : "Edit user successfully.");

        await HideModal();

        await ReloadData.InvokeAsync(true);
    }

    private async void AddressActions(eAction action)
    {
        switch (action)
        {
            case eAction.Add:
                addEditAddressModal.SetParameters(new Address());
                addEditAddressModal.InitData();
                break;
            case eAction.Edit:
                addEditAddressModal.SetParameters(SelectedAddress);
                addEditAddressModal.InitData();
                break;
            case eAction.Delete:
                UserService.DeleteUser(SelectedAddress.Id);
                break;
        }
    }

    private void OnSelectedAddressChanged(Address address)
    {
        SelectedAddress = address;
    }

    private void OnUpdateAddressCallBack(Address address)
    {
        User.Addresses.Add(address);
        StateHasChanged();
    }
}