using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Games;

public partial class SelectGameModal : ComponentBase
{
    [Inject] public IGameService GameService { get; set; }
    [Inject] public ICampaignService CampaignService { get; set; }
    [Parameter] public Guid CurrentUserID { get; set; }
    [Parameter] public Campaign Campaign { get; set; }
    private Modal modalRef;
    private eGameBoard SelectedGame = eGameBoard.None;
    private Tetris tetrisGame;
    private TicTacToe tictactoeGame;
    private ConnectFour connectFour;
    private VoucherGiftModal voucherGiftModal;
    private IList<Game> Games { get; set; } 
    private Guid SelectedGameID { get; set; }
       
    public async Task InitData()
    {
        Games = await GameService.GetGames();
        await ShowModal();
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
                SelectedGameID = Games.FirstOrDefault(x => x.Name.ToLower() == "tetris".ToLower()).ID;
                await tetrisGame.InitGame();
                break;
            case eGameBoard.TicTacToe:
                SelectedGameID = Games.FirstOrDefault(x => x.Name.ToLower() == "TicTacToe".ToLower()).ID;
                tictactoeGame.InitGame();
                break;
            case eGameBoard.ConnectFour:
                SelectedGameID = Games.FirstOrDefault(x => x.Name.ToLower() == "ConnectFour".ToLower()).ID;
                connectFour.InitGame();
                break;
        }

        // Insert data for Game User table
        await CampaignService.UpdateCampaignUser(Campaign.Id, CurrentUserID);
        await HideModal();
    }

    private async void OnCanReceiveVoucher()
    {
        var voucher = GetRandomNumber(20, 70);
        eVoucherType selectedVoucherType = eVoucherType.NetPrice;

        if (voucher <= 25)
        {
            selectedVoucherType = eVoucherType.TwentyFive;
        }
        else if (voucher > 25 && voucher <= 35)
        {
            selectedVoucherType = eVoucherType.ThirtyFive;
        }
        else if (voucher > 35 && voucher <= 50)
        {
            selectedVoucherType = eVoucherType.ThirtyFive;
        }

        voucherGiftModal.InitVoucherGift(selectedVoucherType);
        // Insert data for Voucher
        await GameService.CreateVoucher(new Voucher
        {
            Type = selectedVoucherType,
            CreatedDate = DateTime.Now,
            ExpiredDate = DateTime.Now.AddDays(45),
            Quantity = 1,
            Game_ID_FK = SelectedGameID,
            User_ID_FK = CurrentUserID,
            Name = Campaign.Name,
            Code = $"{GenerateAcronym(Campaign.Name)}-{GetRandomNumber(100000, 1000000).ToString("D6")}"
        }) ;
    }

    public static string GenerateAcronym(string input)
    {
        string[] words = input.Split(' ');
        string acronym = "";
        foreach (string word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                char firstChar = char.ToUpper(word[0]);
                acronym += firstChar;
            }
        }
        return acronym;
    }

    private readonly Random getrandom = new Random();
    public int GetRandomNumber(int min, int max)
    {
        lock (getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }
}

public enum eGameBoard
{
    None = 0,
    Tetris = 1,
    TicTacToe = 2,
    ConnectFour = 3
}