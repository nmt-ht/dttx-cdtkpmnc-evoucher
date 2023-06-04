using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games;

public partial class SelectGameModal : ComponentBase
{
    // reference to the modal component
    private Modal modalRef;
    private eGameBoard SelectedGame = eGameBoard.None;
    private Tetris tetrisGame;
    private TicTacToe tictactoeGame;
    private ConnectFour connectFour;
    public void InitData()
    {
        ShowModal();
    }
    private Task ShowModal()
    {
        return modalRef.Show();
    }

    private Task HideModal()
    {
        return modalRef.Hide();
    }

    private async Task OnSelectedGame(eGameBoard gameBoard)
    {
        SelectedGame = gameBoard;
        switch (SelectedGame)
        {
            case eGameBoard.None:
                break;
            case eGameBoard.Tetris:
                await tetrisGame.InitGame();
                break;
            case eGameBoard.TicTacToe:
                tictactoeGame.InitGame();
                break;
            case eGameBoard.ContectFour:
                connectFour.InitGame();
                break;
            default:
                break;
        }

        await HideModal();
    }
}

public enum eGameBoard
{
    None = 0,
    Tetris = 1,
    TicTacToe = 2,
    ContectFour = 3
}