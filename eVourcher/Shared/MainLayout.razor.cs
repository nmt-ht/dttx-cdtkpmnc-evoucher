using eVoucher.Models;
using eVoucher.Pages.Accounts;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace eVoucher.Shared;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public IUserService UserService { get; set; }
    private bool show, display, loggedIn;
    private string? user;
    private Login login;
    private User CurrentUser { get; set; }
    private string Title = "eVoucher System";

    private bool IsHomePage { get { return NavManager.BaseUri == NavManager.Uri; } }
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
        NavManager.NavigateTo("/");
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
            await LocalStorage.SaveStringAsync("userId", userLogin.ID.ToString());
            await LocalStorage.SaveStringAsync("user", user);

            user = await LocalStorage.GetStringAsync("userId");
            CurrentUser = await UserService.GetUserById(Guid.Parse(user));
        }
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}