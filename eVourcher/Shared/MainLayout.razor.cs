using eVoucher.Models;
using eVoucher.Pages.Accounts;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace eVoucher.Shared;

public partial class MainLayout : LayoutComponentBase
{
    #region External services
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILocalStorage LocalStorage { get; set; }
    #endregion

    #region Private variables
    private bool show, display, loggedIn;
    private string? user;
    private Login login;
    private User CurrentUser { get; set; }
    private string Title = "eVoucher System";

    private bool IsHomePage { get { return NavManager.BaseUri == NavManager.Uri; } }
    private bool IsAdminPage { get { return NavManager.Uri.Contains("admin"); } }
    private bool IsPartnerPage { get { return NavManager.Uri.Contains("partner"); } }
    #endregion

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
        CurrentUser = userLogin;
        await Login(userLogin);
        StateHasChanged();
    }

    public async Task Logout()
    {
        user = null;
        loggedIn = false;
        CurrentUser = null;
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
        }

        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}