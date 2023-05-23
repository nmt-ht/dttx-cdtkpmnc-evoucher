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
    private bool IsAdded = false;
    private string Title => IsAdded ? "Add Game" : "Edit Game";

    public void SetParameters(Game game, bool isAdded)
    {
        Game = game;
        IsAdded = isAdded;
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

    private async Task UpdateData()
    {
        if(IsAdded)
        {
            Game.CreatedBy = Guid.Parse("7CF80730-A4C2-4A55-96D0-811F549947C6");
            Game.ModifiedBy = Guid.Parse("7CF80730-A4C2-4A55-96D0-811F549947C6");
            var result = await GameService.UpdateGame(Game);
            if (result)
                await NotificationService.Info(IsAdded ? "Added Game successfully." : "Edit Game successfully.");
        }
    }

    //
    private Game Game { get; set; }
    [Parameter] public EventCallback<Game> UpdateAddressCallBack { get; set; }
    private bool IsAdded => Game is not null && Game.Id != Guid.Empty ? false : true;
    //private IList<eAddressType> AddressTypes = new List<eAddressType>() { eAddressType.ShipTo, eAddressType.BillTo, eAddressType.BillToShipTo, eAddressType.Company };
    private string Title => IsAdded ? "Add Game" : "Edit Game";
    private Modal addressRef;

    public void SetParameters(Game game)
    {
        Game = game;
    }
    

   
}