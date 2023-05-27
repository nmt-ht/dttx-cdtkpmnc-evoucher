﻿using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Campaigns.Components;
using eVoucher.Pages.Partners.Components;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Campaigns;
public partial class CampaignView : ComponentBase
{
    #region External services
    /// <summary>
    /// Inject campaign service is to send a request to API
    /// </summary>
    [Inject] public ICampaignService CampaignService { get; set; }
    /// <summary>
    /// It is using for getting the data which saved to local storage.
    /// Ref: https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage
    /// </summary>
    [Inject] public ILocalStorage LocalStorage { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    #endregion

    private Guid? CurrentUserId { get; set; }
    private IList<Campaign> Campaigns { get; set; } = new List<Campaign>();
    private Campaign selectedCampaign;
    private AddEditCampaignModal addEditCampaignModal;
    private DeleteCampaignModal deleteCampaignModal;

    protected override async Task OnInitializedAsync()
    {
        var userID = await LocalStorage.GetStringAsync("userId");
        if (!string.IsNullOrEmpty(userID))
            CurrentUserId = Guid.Parse(userID);
        await LoadData();
    }

    private async Task LoadData()
    {
        await ShowLoadingPage(true);
        Campaigns = await CampaignService.GetCampaigns();
        var index = 0;
        Campaigns.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }

    private async Task CampaignActions(eAction action)
    {
        switch (action)
        {
            case eAction.Add:
                var campaign = new Campaign();               
                addEditCampaignModal.SetParameters(campaign, true, CurrentUserId);
                addEditCampaignModal.InitData();
                break;
            case eAction.Edit:
                if (selectedCampaign != null && selectedCampaign.Id != Guid.Empty)
                {
                    var editCampaign = await CampaignService.GetCampaignById(selectedCampaign.Id);
                    if (editCampaign.Games is not null && editCampaign.Games.Any())
                    {
                        var index = 0;
                        editCampaign.Games.ToList().ForEach(a => a.Index = ++index);
                    }
                    addEditCampaignModal.SetParameters(editCampaign, false, CurrentUserId);
                    addEditCampaignModal.InitData();
                }
                break;
            case eAction.Delete:
                if (selectedCampaign != null && selectedCampaign.Id != Guid.Empty)
                {
                    deleteCampaignModal.InitData();
                }
                break;
            default:
                break;
        }
    }
    private async Task OnUpdateCampaign()
    {
        await LoadData();
    }
    private async Task OnDeleteCampaign()
    {
        var result = await CampaignService.DeleteCampaign(selectedCampaign.Id);

        if (result)
        {
            await NotificationService.Info("Delete successfully.");
            await LoadData();
        }
        else
        {
            await NotificationService.Info("An error occurred please try again.");
        }
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