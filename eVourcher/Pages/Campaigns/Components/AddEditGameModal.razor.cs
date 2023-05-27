using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;
using Game = eVoucher.Models.Game;

namespace eVoucher.Pages.Campaigns.Components;
public partial class AddEditGameModal : ComponentBase
{
    private Game Game { get; set; }
    [Parameter] public EventCallback<Game> UpdateGameCallBack { get; set; }
    private bool IsAdded => Game is not null && Game.ID != Guid.Empty ? false : true;
    //private IList<eAddressType> AddressTypes = new List<eAddressType>() { eAddressType.ShipTo, eAddressType.BillTo, eAddressType.BillToShipTo, eAddressType.Company };
    private string Title => IsAdded ? "Add Address" : "Edit Address";
    private Modal gameRef;
    
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