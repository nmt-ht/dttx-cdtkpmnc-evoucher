using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Linq;
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
        if (IsAdded)
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

    private bool IsEditingAddress = false;
    private async Task AddressActions(eAction action)
    {
        IsEditingAddress = false;
        switch (action)
        {
            case eAction.Add:
                addEditAddressModal.SetParameters(new Address());
                addEditAddressModal.InitData();
                break;
            case eAction.Edit:
                EditAddress();
                break;
            case eAction.Delete:
                await DeleteAddress();
                break;
        }
    }

    private void EditAddress()
    {
        IsEditingAddress = true;
        addEditAddressModal.SetParameters(SelectedAddress);
        addEditAddressModal.InitData();
    }

    private async Task DeleteAddress()
    {
        if(SelectedAddress != null && SelectedAddress.ID == System.Guid.Empty)// Delete on memory only
        {
            var deletedAddress = User.Addresses.FirstOrDefault(x => x.Index == SelectedAddress.Index);
            if (deletedAddress != null) { User.Addresses.Remove(deletedAddress); }
        }
        else
        {
            await UserService.DeleteAddress(SelectedAddress.ID);

            //Reload data on memory
            var deletedAddress = User.Addresses.FirstOrDefault(x => x.ID == SelectedAddress.ID);
            if (deletedAddress != null) { User.Addresses.Remove(deletedAddress); }
        }
    }

    private async Task OnUpdateAddressCallBack(Address address)
    {
        if (!IsEditingAddress)
        {
            var maxIndex = User.Addresses.Count + 1;
            address.Index = maxIndex;
            User.Addresses.Add(address);
        }
        else
        {
            var editAddress = User.Addresses.Where(x => x.ID == address.ID).FirstOrDefault();
            if (editAddress is not null)
                User.Addresses.Remove(editAddress);

            User.Addresses.Add(address);

            // Update Address to database
            await UserService.UpdateAddess(address);
        }
        StateHasChanged();
    }
}