using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Games;
using eVoucher.Pages.Partners.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVoucher.Pages;

public partial class HomePage
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    private SelectGameModal selectGameModal;
    private IList<string> Locations = new List<string> { "Hồ Chí Minh", "Hà Nội", "Đà Nẵng" };
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
    [CascadingParameter]private User CurrentUser { get; set; }
    private int pageSize = 10;
    protected override async Task OnInitializedAsync()
    {
        await ShowLoadingPage(true);
        CurrentUser = UserService.CurrentUser;
        UserService.CurrentUserChanged += UpdateCurrentUser;
        Campaigns = await CampaignService.GetCampaigns();
        pageItems = (Campaigns.Count / pageSize);
        await ShowLoadingPage(false);
    }

    private void OnViewGameBoard()
    {
        selectGameModal.InitData();
    }

    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string currentPage = "1";
    private int pageItems = 0;

    private bool IsActive(string page)
        => currentPage == page;

    private bool IsPageNavigationDisabled(string navigation)
    {
        if (navigation.Equals(PREVIOUS))
        {
            return currentPage.Equals("1");
        }
        else if (navigation.Equals(NEXT))
        {
            return currentPage.Equals(pageItems.ToString());
        }
        return false;
    }

    private void Previous()
    {
        var currentPageAsInt = int.Parse(currentPage);
        if (currentPageAsInt > 1)
        {
            currentPage = (currentPageAsInt - 1).ToString();
        }
    }

    private void Next()
    {
        var currentPageAsInt = int.Parse(currentPage);
        if (currentPageAsInt < pageItems)
        {
            currentPage = (currentPageAsInt + 1).ToString();
        }
    }

    private void SetActive(string page)
        => currentPage = page;

    private AddEditPartnerModal refAddEditPartner;
    private void OnRegisterPartner()
    {
        if(UserService.CurrentUser is not null)
        {
            refAddEditPartner.InitData(new Partner { User_ID_FK = UserService.CurrentUser.ID });
        }
    }

    private void OnViewPartnerManagement()
    {
        NavigationManager.NavigateTo("/partner");
    }

    public void UpdateCurrentUser(User newUser)
    {
        CurrentUser = newUser;
    }

    private void GotoHomePage()
    {
        NavigationManager.NavigateTo("/");
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