using eVoucher.Models;
using eVoucher.Pages.Games;
using eVoucher.Pages.Partners.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages;

public partial class HomePage
{
    private SelectGameModal selectGameModal;
    private IList<string> Locations = new List<string> { "Hồ Chí Minh", "Hà Nội", "Đà Nẵng" };
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
    private int pageSize = 15;
    protected override async Task OnInitializedAsync()
    {
        Campaigns = await CampaignService.GetCampaigns();
        Campaigns.ToList().ForEach(campain =>
        {
            Campaigns.Add(campain);
        });

        Campaigns.ToList().ForEach(campain =>
        {
            Campaigns.Add(campain);
        });

        Campaigns.ToList().ForEach(campain =>
        {
            Campaigns.Add(campain);
        });

        Campaigns.ToList().ForEach(campain =>
        {
            Campaigns.Add(campain);
        });

        pageItems = (Campaigns.Count / pageSize);
        StateHasChanged();
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
        if(CurrentUser is not null)
        {

        }
        refAddEditPartner.InitData(new Partner());
    }
}