using Blazorise;
using eVoucher.Pages.Accounts;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System;

namespace eVoucher.Shared;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] public ILocalStorage LocalStorage { get; set; }
    private bool show, display, loggedIn;
    private string? user;
    private Login login;

    public string? GetUserName()
    {
        return loggedIn ? user : null;
    }

    public async Task OpenLogin()
    {
        login.InitData();
    }

    private async Task OnloginSuccess(User userLogin)
    {
        user = userLogin.FirstName + " " + userLogin.LastName;
        await Login(userLogin);
    }

    public async Task Logout()
    {
        user = null;
        loggedIn = false;
        await LocalStorage.RemoveAsync("userId");
        await LocalStorage.RemoveAsync("user");
    }

    private async Task Close()
    {
        show = false;
        await Task.Delay(500);
        display = false;
    }

    private async Task Login(User userLogin)
    {
        if (!string.IsNullOrEmpty(user))
        {
            await Close();
            loggedIn = true;
            await LocalStorage.SaveStringAsync("userId", userLogin.Id.ToString());
            await LocalStorage.SaveStringAsync("user", user);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        user = await LocalStorage.GetStringAsync("user");
        loggedIn = !string.IsNullOrEmpty(user);
    }
}