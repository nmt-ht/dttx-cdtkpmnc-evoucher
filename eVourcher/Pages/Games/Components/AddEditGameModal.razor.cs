using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eVoucher.Pages.Games.Components;
public partial class AddEditGameModal : ComponentBase
{
    [Inject] public IGameService GameService { get; set; }
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public EventCallback OnUpdateGame { get; set; }

    private Validations validationsRef;
    public Game Game { get; set; } = new Game();
    private Modal modalRef;
    private bool IsAdded => Game is not null && Game.ID != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Game" : "Edit Game";
    private IList<User> Users { get; set; }
    private Guid SelectedUserId { get; set; }
    public async void InitData(Game game, Guid? userID)
    {
        ResetValue();
        Game = game;
        Users = await UserService.GetUsers();
        SelectedUserId = userID.Value;
        ImageDataUrl = GetImageDataUrlFromBytes(Game.Image);
        await validationsRef.ClearAll();
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
    private async Task UpdateData()
    {
        if (await validationsRef.ValidateAll())
        {
            if (IsAdded)
            {
                Game.CreatedDate = DateTime.Now;
                Game.ModifiedDate = DateTime.Now;
                Game.ModifiedBy = SelectedUserId;
                var result = await GameService.CreateGame(Game);
                if (result)
                {
                    await NotificationService.Info("Added Game successfully.");
                    await HideModal();
                    await OnUpdateGame.InvokeAsync();
                }
                else
                {
                    await NotificationService.Info("An error occurred please try again.");
                }
            }
            else
            {
                Game.ModifiedDate = DateTime.Now;
                Game.ModifiedBy = SelectedUserId;
                var result = await GameService.UpdateGame(Game);
                if (result)
                {
                    await NotificationService.Info("Added Game successfully.");
                    await HideModal();
                    await OnUpdateGame.InvokeAsync();
                }
                else
                {
                    await NotificationService.Info("An error occurred please try again.");
                }
            }
        }
    }

    protected string ImageDataUrl { get; set; }
    async Task OnFileUpload(FileUploadEventArgs e)
    {
        try
        {
            using (var stream = new MemoryStream())
            {
                await e.File.WriteToStreamAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                // Convert the image stream to a base64-encoded data URL
                ImageDataUrl = await GetImageDataUrl(stream);
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }
        finally
        {
            this.StateHasChanged();
        }
    }
    private string GetImageDataUrlFromBytes(byte[] imageBytes)
    {
        if (imageBytes is not null && imageBytes.Length > 0)
        {
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var imageDataUrl = $"data:image/png;base64,{imageBase64}";
            return imageDataUrl;
        }
        return string.Empty;
    }
    private async Task<string> GetImageDataUrl(Stream imageStream)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Copy the image stream to the memory stream
            await imageStream.CopyToAsync(memoryStream);

            // Convert the image bytes to a base64-encoded data URL
            var imageBytes = memoryStream.ToArray();
            Game.Image = imageBytes;
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var imageDataUrl = $"data:image/png;base64,{imageBase64}";

            return imageDataUrl;
        }
    }    
    private void ResetValue()
    {
        SelectedUserId = Guid.Empty;
        ImageDataUrl = string.Empty;
    }
}
