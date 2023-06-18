﻿using Blazorise;
using eVoucherGames.Models.Tetris;
using eVoucherGames.Models.Tetris.Enums;
using eVoucherGames.Models.Tetris.Tetrominos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games;

public partial class Tetris : ComponentBase
{
    [Parameter] public EventCallback<bool> OnCloseCallback { get; set; }
    [Parameter] public EventCallback<bool> CanReceiveVoucher { get; set; }

    Grid grid = new Grid();
    private const int LIMIT_SCORE = 50;

    //Creates new tetrominos as the game needs them
    TetrominoGenerator generator = new TetrominoGenerator();

    //Represents the currently-falling tetromino
    Tetromino currentTetromino;

    //Represents the next three tetromino styles.
    //The actual tetrominos will be created only when they become the current tetromino.
    TetrominoStyle nextStyle;
    TetrominoStyle secondNextStyle;
    TetrominoStyle thirdNextStyle;

    //The standard delay is how long the game waits before dropping the current tetromino by one row.
    int standardDelay = 1000;

    //This flag is set if the player "hard drops" a tetromino all the way to the bottom
    bool skipDelay = false;

    //The level increases for each 4000 points scored. Every time the level increases,
    //the standard delay gets shorter.
    int level = 1;
    int score = 0;
    int previousHighScore = 0;
    string previousScoreCookieValue = "Nothing";

    //This flag changes based on whether or not the user checks the "mute audio" checkbox.
    bool playAudio = true;

    protected ElementReference gameBoardDiv;  // set by the @ref attribute

    protected override async Task OnInitializedAsync()
    {
        //Get the previous high score cookie if one exists
        previousScoreCookieValue = await _jsRuntime.InvokeAsync<string>("ReadCookie", "tetrisHighScore");
        bool hasHighScore = int.TryParse(previousScoreCookieValue, out previousHighScore);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsRuntime.InvokeVoidAsync("SetFocusToElement", gameBoardDiv);
        }
    }

    public void NewGame()
    {
        grid = new Grid();
        generator = new TetrominoGenerator();
        currentTetromino = null;
        level = 1;
        score = 0;
    }

    public async Task RunGame()
    {
        //Start playing the theme music
        if (playAudio)
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "theme");

        //Generate the styles of the first three tetrominos that will be dropped
        nextStyle = generator.Next();
        secondNextStyle = generator.Next(nextStyle);
        thirdNextStyle = generator.Next(nextStyle, secondNextStyle);

        //Start playing the game
        grid.State = GameState.Playing;

        //Focus the browser on the board div
        await _jsRuntime.InvokeVoidAsync("SetFocusToElement", gameBoardDiv);

        //Where there is no tetromino with a row of 21 or greater
        while (!grid.Cells.HasRow(21)) //Game loop
        {
            //Create the next tetromino to be dropped from the already-determined nextStyle,
            //and move the styles "up" in line
            currentTetromino = generator.CreateFromStyle(nextStyle, grid);
            nextStyle = secondNextStyle;
            secondNextStyle = thirdNextStyle;
            thirdNextStyle = generator.Next(currentTetromino.Style, nextStyle, secondNextStyle);

            StateHasChanged();

            //Run the current tetromino until it can't move anymore
            await RunCurrentTetromino();

            //If any rows are filled, remove them from the board
            await ClearCompleteRows();

            //If the score is high enough, move the user to a new level
            LevelChange();
        }

        //Once there is a tetromino with a row of 21 or greater, the game is over.
        grid.State = GameState.GameOver;

        //If the current high score is larger than the old high score, update the cookie
        if (score > previousHighScore)
            await _jsRuntime.InvokeAsync<object>("WriteCookie", "tetrisHighScore", score, 14);

        if (score > LIMIT_SCORE)
        {
            await CanReceiveVoucher.InvokeAsync();
        }
    }

    public async Task ToggleAudio()
    {
        if (playAudio)
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "theme");
        else
            await _jsRuntime.InvokeAsync<string>("PauseAudio", "theme");

        //Focus the browser on the board div
        await _jsRuntime.InvokeVoidAsync("SetFocusToElement", gameBoardDiv);
    }

    //Delays the game up to the passed-in amount of milliseconds in 50 millisecond intervals
    public async Task Delay(int millis)
    {
        int totalDelay = 0;
        while (totalDelay < millis && !skipDelay)
        {
            totalDelay += 50;
            await Task.Delay(50);
        }
        skipDelay = false;
    }

    public async Task RunCurrentTetromino()
    {
        //While the tetromino can still move down
        while (currentTetromino.CanMoveDown())
        {
            //Wait for the standard delay
            await Delay(standardDelay);

            //Move the tetromino down one row
            currentTetromino.MoveDown();

            //Update the display
            StateHasChanged();

            //If the tetromino can no longer move down BUT can still move in other directions,
            //delay for an additional half-second to let the user move if they want.
            if (!currentTetromino.CanMoveDown() && currentTetromino.CanMove())
                await Delay(500);
        }

        //"Solidify" the current tetromino by adding its covered squares to the board's cells
        grid.Cells.AddMany(currentTetromino.CoveredCells.GetAll(), currentTetromino.CssClass);
    }

    public void LevelChange()
    {
        //The user goes up a level for every 4000 points they score.
        int counter = 1;
        int scoreCopy = score;
        while (scoreCopy > 4000)
        {
            counter++;
            scoreCopy -= 4000;
        }

        int newLevel = counter;
        if (newLevel != level) //If the user has gone up a level
        {
            //Reduce the drop delay by 100 milliseconds for every level the User has made.
            standardDelay = 1000 - ((newLevel - 1) * 100);

            //Set the new level
            level = newLevel;
        }
    }

    public async Task ClearCompleteRows()
    {
        //For each row
        List<int> rowsComplete = new List<int>();
        for (int i = 1; i <= grid.Height; i++)
        {
            //If every position in that row is filled...
            if (grid.Cells.GetAllInRow(i).Count == grid.Width)
            {
                //Add the "complete" animation CSS class
                grid.Cells.SetCssClass(i, "tetris-clear-row");

                //Mark that row as complete
                rowsComplete.Add(i);
            }
        }

        //If there are any complete rows
        if (rowsComplete.Any())
        {
            StateHasChanged();

            //Collapse the "higher" cells down to fill in the completed rows.
            grid.Cells.CollapseRows(rowsComplete);

            //Calculate the score for the completed row(s)
            switch (rowsComplete.Count)
            {
                case 1:
                    score += 40 * level;
                    break;

                case 2:
                    score += 100 * level;
                    break;

                case 3:
                    score += 300 * level;
                    break;

                case 4:
                    score += 1200 * level;
                    break;
            }

            await Task.Delay(1000);
        }
        grid.State = GameState.Playing;
    }

    protected async Task KeyDown(KeyboardEventArgs e)
    {
        if (grid.State == GameState.Playing)
        {
            if (e.Key == "ArrowRight")
            {
                currentTetromino.MoveRight();
            }
            if (e.Key == "ArrowLeft")
            {
                currentTetromino.MoveLeft();
            }
            if (e.Key == "ArrowDown" || e.Key == " ")
            {
                int addlScore = currentTetromino.Drop();
                score += addlScore;
                skipDelay = true;
                StateHasChanged();
            }
            if (e.Key == "ArrowUp")
            {
                currentTetromino.Rotate();
            }
            if (e.Key == "m")
            {
                await ToggleAudio();
            }
            StateHasChanged();
        }
    }

    private Modal modalRef;

    private Task ShowModal()
    {
        return modalRef.Show();
    }

    private async Task HideModal()
    {
        await OnCloseCallback.InvokeAsync();
        playAudio = false;
        await ToggleAudio();
        await modalRef.Hide();
    }

    public async Task InitGame()
    {
        await ShowModal();
    }

    private async Task OnPauseSound(bool value)
    {
        playAudio = value;
        await ToggleAudio();
    }
}