using eVoucher.Models;
using eVoucher.Pages.Accounts;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace eVoucher.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public IUserService UserService { get; set; }
    private bool show, display, loggedIn;
    private string? user;
    private Login login;
    private User CurrentUser { get; set; }

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
            await LocalStorage.SaveStringAsync("userId", userLogin.ID.ToString());
            await LocalStorage.SaveStringAsync("user", user);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        NavManager.LocationChanged += LocationChanged;
    }
    private async void LocationChanged(object sender, LocationChangedEventArgs e)
    {
        var sortUrl = NavManager.Uri.Replace(NavManager.BaseUri, string.Empty);
        if (sortUrl.Contains("admin"))
        {
            login.InitData();
            user = await LocalStorage.GetStringAsync("user");
            loggedIn = !string.IsNullOrEmpty(user);
            CurrentUser = await UserService.GetUserById(Guid.Parse(user));
        }
        StateHasChanged();
    }

    public void Dispose()
    {
        NavManager.LocationChanged -= LocationChanged;
    }
}