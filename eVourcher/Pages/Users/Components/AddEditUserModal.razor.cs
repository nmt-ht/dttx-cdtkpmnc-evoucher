using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Address = eVoucher.Models.Address;
namespace eVoucher.Pages.Users.Components;
public partial class AddEditUserModal : ComponentBase
{
    #region External service
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    #endregion

    #region Paramters
    [Parameter] public EventCallback<bool> ReloadData { get; set; }
    #endregion

    #region Private varialbles
    private User User { get; set; } = new();
    private Modal userRef;
    private bool IsAdded = false;
    private string Title => IsAdded ? "Add User" : "Edit User";
    private Address SelectedAddress { get; set; }
    private AddEditAddresModal addEditAddressModal;
    private bool IsEditingAddress = false;
    Validations validations;
    private IList<UserGroup> UserGroups { get; set; }   
    private Guid SelectedUserGroupID { get; set; }
    #endregion

    public void SetParameters(User user, bool isAdded)
    {
        User = user;
        IsAdded = isAdded;
    }
    public async void InitData()
    {
        await validations.ClearAll();
        UserGroups = await UserService.GetUserGroups();
        await ShowModal();
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
        if(SelectedUserGroupID != Guid.Empty)
        {
            User.UserGroups = new List<UserGroup>();
            User.UserGroups.Add(new UserGroup { ID = SelectedUserGroupID});
        }

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
    private async Task AddressActions(eAction action)
    {
        IsEditingAddress = false;
        switch (action)
        {
            case eAction.Add:
                addEditAddressModal.SetParameters(new Address(), eAction.Add);
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
        addEditAddressModal.SetParameters(SelectedAddress, eAction.Edit);
        addEditAddressModal.InitData();
    }
    private async Task DeleteAddress()
    {
        if (SelectedAddress != null && SelectedAddress.ID == System.Guid.Empty)// Delete on memory only
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
        if (await validations.ValidateAll())
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
        }
        StateHasChanged();
    }

    void ValidateDate(ValidatorEventArgs e)
    {
        if (e.Value == null)
            e.Status = ValidationStatus.Error;
        else
        {
            var dateOfBirth = DateTime.Parse(e.Value.ToString());
            e.Status = dateOfBirth.Date.Equals(DateTime.MinValue.Date) || dateOfBirth.Date < DateTime.Now.Date || (DateTime.Now.Year - dateOfBirth.Date.Year) <= 16
                ? ValidationStatus.Error 
                : ValidationStatus.Success;
        }
    }
}