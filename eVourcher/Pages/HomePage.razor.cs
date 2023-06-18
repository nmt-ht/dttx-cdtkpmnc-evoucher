using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Games;
using eVoucher.Pages.Partners.Components;
using eVoucher.Pages.Vouchers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages;

public partial class HomePage
{
    #region External Services
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    #endregion

    #region Parametters
    [CascadingParameter] private User CurrentUser { get; set; }
    #endregion

    #region Private parametters
    private SelectGameModal selectGameModal;
    private IList<string> Locations = new List<string> { "Hồ Chí Minh", "Hà Nội", "Đà Nẵng" };
    private IList<Campaign> CampaignData { get; set; } = new List<Campaign>();
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
    private int pageSize = 10;
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string currentPage = "1";
    private int pageItems = 0;
    private Campaign SelectedCampain { get; set; }
    private string LocationName { get; set; } = "Hồ Chí Minh";
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await ShowLoadingPage(true);
        CurrentUser = UserService.CurrentUser;
        UserService.CurrentUserChanged += UpdateCurrentUser;
        CampaignData = await CampaignService.GetCampaigns();
        Campaigns = GetCampainsByLocation(LocationName);

        pageItems = (Campaigns.Count / pageSize);
        await ShowLoadingPage(false);

        onInputThrottled = ThrottleEvent<ChangeEventArgs>(e => TextSearch = (string)e.Value, TimeSpan.FromMilliseconds(300));
        base.OnInitialized();
    }

    private IList<Campaign> GetCampainsByLocation(string locationName)
    {
       return CampaignData.Where(c =>
               c.Partners.Any(p =>
                   p.CompanyAddess.Any(ca =>
                       string.Compare(ca.City, locationName, true) == 0)
               )
           ).ToList();
    }

    private void OnLocationChange(string location)
    {
        LocationName = location;
        Campaigns = GetCampainsByLocation(LocationName);
        StateHasChanged();
    }

    private async Task OnViewGameBoard(Campaign campain)
    {
        if (CurrentUser is null)
        {
            await NotificationService.Info("Please login first. Thank you");
            return;
        }
        SelectedCampain = campain;
        await selectGameModal.InitData();
    }


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
        if (UserService.CurrentUser is not null)
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

    private VoucherManagementModal refVouchermanagement;
    private async Task ViewVoucherManagement()
    {
        await refVouchermanagement.BindData();
    }

    private void OnFilterByPartnerType(ePartnerType type)
    {
        Campaigns = GetCampainsByLocation(LocationName);
        Campaigns = Campaigns.Where(c => c.Partners.Any(p => p.Type == type)).ToList();
        StateHasChanged();
    }

    private string TextSearch { get; set; } = string.Empty;

    Action<ChangeEventArgs> onInputThrottled;
    Action<T> ThrottleEvent<T>(Action<T> action, TimeSpan interval)
    {
        return Throttle<T>(arg =>
        {
            InvokeAsync(() =>
            {
                action(arg);
                StateHasChanged();
            });
        }, interval);
    }

    Action<T> Throttle<T>(Action<T> action, TimeSpan interval)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        Task task = null;
        var l = new object();
        T args = default;
        return (T arg) =>
        {
            args = arg;
            if (task != null)
                return;

            lock (l)
            {
                if (task != null)
                    return;
                task = Task.Delay(interval).ContinueWith(t =>
                {
                    action(args);
                    task = null;
                });
            }
        };
    }
    private void OnSearch()
    {
        if(string.IsNullOrEmpty(TextSearch))
        {
            NotificationService.Warning("Vui lòng nhập thông tin bạn cần tìm kiếm.");
            return;
        }

        Campaigns = GetCampainsByLocation(LocationName);
        Campaigns = Campaigns.Where(x => x.Name.ToLower().Contains(TextSearch.ToLower()) 
                || x.Partners.Any(p => p.CompanyName.ToLower().Contains(TextSearch.ToLower()))).ToList();
        StateHasChanged();
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