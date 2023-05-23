using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Game = eVoucher.Models.Game;

namespace eVoucher.Pages.Games.Components;
public partial class AddEditGameModal : ComponentBase
{
    [Inject] public ICampaignService GameService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public Game Campaign { get; set; } = new();
    
    private Modal gameRef;
    /*private bool IsAdded => Campaign is not null && Campaign.Id != Guid.Empty ? false : true;
    */
    private Game Game { get; set; }
    [Parameter] public EventCallback<Game> UpdateGameCallBack { get; set; }

    private bool IsAdded = false;
    //private bool IsAdded => Game is not null && Game.Id != Guid.Empty ? false : true;
    //private IList<eAddressType> AddressTypes = new List<eAddressType>() { eAddressType.ShipTo, eAddressType.BillTo, eAddressType.BillToShipTo, eAddressType.Company };
    private string Title => IsAdded ? "Add Game" : "Edit Game";
    private Modal addressRef;

    public void SetParameters(Game game)
    {
        Game = game;
    }
    public void InitData()
    {
        Game = IsAdded ? new() : Game;
        StateHasChanged();
        ShowModal();
    }

    private Task ShowModal()
    {
        return gameRef.Show();
    }

    private Task HideModal()
    {
        return gameRef.Hide();
    }

    private void UpdateData()
    {
        UpdateGameCallBack.InvokeAsync(Game);
        HideModal();
    }




}