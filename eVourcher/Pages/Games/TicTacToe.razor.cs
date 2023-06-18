using Blazorise;
using eVoucherGames.Models.TicTacToe;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games;

public partial class TicTacToe : ComponentBase
{
    [Parameter] public EventCallback<bool> CanReceiveVoucher { get; set; }
    [Parameter] public EventCallback<bool> OnCloseCallback { get; set; }
    GameBoard board  = new GameBoard();
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await _jsRuntime.InvokeVoidAsync("Prism.highlightAll");
    }

    public void InitGame()
    {
        ShowModal();
        StateHasChanged();
    }

    private Modal modalRef;

    private Task ShowModal()
    {
        return modalRef.Show();
    }

    private Task HideModal()
    {
        OnCloseCallback.InvokeAsync();
        return modalRef.Hide();
    }

    private void OnResetGame()
    {
        var winPlay = board.GetWinner();
        if (board.GameComplete && winPlay is not null && winPlay.WinningStyle != eVoucherGames.Models.TicTacToe.Enums.PieceStyle.Blank)
        {
            CanReceiveVoucher.InvokeAsync();
            HideModal();
        }
        board.Reset();
    }
}