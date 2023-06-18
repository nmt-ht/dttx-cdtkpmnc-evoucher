using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games;

public partial class ConnectFour : ComponentBase
{
    eVoucherGames.Models.ConnectFour.GameBoard board = new eVoucherGames.Models.ConnectFour.GameBoard();
    [Parameter] public EventCallback<bool> CanReceiveVoucher { get; set; }
    [Parameter] public EventCallback<bool> OnCloseCallback { get; set; }

    private Modal modalRef;
    public void InitGame()
    {
        ShowModal();
    }

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
        if(board.WinningPlay.WinningColor != eVoucherGames.Models.ConnectFour.Enums.PieceColor.Blank)
        {
            CanReceiveVoucher.InvokeAsync();
            HideModal();
        }
        board.Reset();
    }
}