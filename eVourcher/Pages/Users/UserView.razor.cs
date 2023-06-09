﻿using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Users;
public partial class UserView : ComponentBase
{
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }
    [Inject] public IUserService UserService { get; set; }
    private IList<User> Users { get; set; } = new List<User>();

    private User selectedUser;
    private AddEditUserModal addEditUserModal;

    //protected override async Task OnInitializedAsync()
    //{
    //    await LoadData();
    //}

    public async Task InitData()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        await ShowLoadingPage(true);
        Users = await UserService.GetUsers();
        var index = 0;
        Users.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }

    private async Task UserActions(eAction action)
    {
        switch (action)
        {
            case eAction.Add:
                addEditUserModal.SetParameters(new User(), true);
                addEditUserModal.InitData();
                break;
            case eAction.Edit:
                await EditUser();
                break;
            case eAction.Delete:
                var confirm = await MessageService.Confirm("Are you sure delete this user?");
                if (confirm)
                {
                    var result = await UserService.DeleteUser(selectedUser.ID);
                    if (result)
                    {
                        await NotificationService.Info("Delete user successfully.");
                        await LoadData();
                    }
                }
                break;
        }
    }

    private async Task EditUser()
    {
        var user = await UserService.GetUserById(selectedUser.ID);      
        addEditUserModal.SetParameters(user, false);
        addEditUserModal.InitData();
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