using eVoucher.Models;
using eVoucher.Pages.Accounts;
using eVoucher.Pages.Campaigns;
using eVoucher.Pages.Games;
using eVoucher.Pages.Partners;
using eVoucher.Pages.Users;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Shared;

public partial class AdminLayout : LayoutComponentBase, IDisposable
{
    #region External Service
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public IUserService UserService { get; set; }
    #endregion

    [Parameter] public User CurrentUser { get; set; }

    #region Private variables
    private bool show, display;
    private string? user;
    private Login login;
    private string Title = "eVoucher Administrator";
    private UserView userView;
    private PartnerView partnerView;
    private CampaignView campaignView;
    private GameView gameView;
    private eAdminComponent SelectedComponent { get; set; } = eAdminComponent.Dasboard;
    private bool loggedIn;
    private bool LoggedIn
    {
        get
        {
            return loggedIn || CurrentUser is not null;
        }
        set { loggedIn = value; }
    }
    #endregion

    public string? GetUserName()
    {
        return LoggedIn ? user : null;
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

        NavManager.NavigateTo("/admin");
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

            CurrentUser = await UserService.GetUserById(userLogin.ID);
        }
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        StateHasChanged();
    }

    public void Dispose()
    {
        NavManager.LocationChanged -= LocationChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        NavManager.LocationChanged += LocationChanged;
    }

    private async void LocationChanged(object sender, LocationChangedEventArgs e)
    {
        var url = NavManager.Uri.ToString();
        if (url.Contains("users"))
        {
            SelectedComponent = eAdminComponent.User;
            StateHasChanged();
            await userView.InitData();
        }
        else if (url.Contains("partners"))
        {
            SelectedComponent = eAdminComponent.Partner;
            StateHasChanged();
            await partnerView.InitData();
        }
        else if (url.Contains("campaigns"))
        {
            SelectedComponent = eAdminComponent.Campaign;
            StateHasChanged();
            await campaignView.InitData();
        }
        StateHasChanged();
    }
}