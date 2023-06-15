using eVoucher.Models;
using eVoucher.Pages.Accounts;
using eVoucher.Pages.Campaigns;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = eVoucher.Models.User;

namespace eVoucher.Shared;


public partial class Customer : ComponentBase
{
    #region External services
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public IUserService UserService { get; set; }
    [Inject] public ICampaignService CampaignService { get; set; }
    #endregion

    [Parameter] public User CurrentUser { get; set; }

    #region Private variables
    private bool show, display;
    private string? user;
    private Login login;

    private bool loggedIn;
    private bool LoggedIn
    {
        get
        {
            return loggedIn || CurrentUser is not null;
        }
        set { loggedIn = value; }
    }

    private string Title = "eVoucher Partner";
    private CampaignView campaignView;
    
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

            var userId = await LocalStorage.GetStringAsync("userId");
            CurrentUser = await UserService.GetUserById(Guid.Parse(userId));
        }
        StateHasChanged();
    }

    public IList<string> Locations = new List<string> { "Hồ Chí Minh", "Hà Nội", "Đà Nẵng" };
    private IList<Campaign> CampaignForFoods { get; set; } = new List<Campaign>();

    protected override async Task OnInitializedAsync()
    {
        await BindData();
    }

    public async Task BindData()
    {
        CampaignForFoods = await CampaignService.GetCampaigns();
        StateHasChanged();
    }
}
