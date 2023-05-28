using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace eVoucher.Shared;

public partial class LeftMenu : ComponentBase
{
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public IUserService  UserService { get; set; }
    [Parameter] public User CurrentUser { get; set; }  
    protected override async Task OnInitializedAsync()
    {
        //var userIDString = await LocalStorage.GetStringAsync("userId");
        //if(!string.IsNullOrEmpty(userIDString))
        //{
        //    CurrentUser = await UserService.GetUserById(Guid.Parse(userIDString));
        //}
    }
}