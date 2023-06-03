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

namespace eVoucher.Pages.Partners.Components;
public partial class AddEditPartnerModal : ComponentBase
{
    [Inject] public IPartnerService PartnerService { get; set; }
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public EventCallback OnUpdatePartner { get; set; }

    private Validations validationsRef;
    public Partner Partner { get; set; } = new Partner();
    private Modal modalRef;
    private bool IsAdded => Partner is not null && Partner.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Partner" : "Edit Partner";
    private IList<User> Users { get; set; }
    private Guid SelectedUserId { get; set; }
    public async void InitData(Partner partner)
    {
        ResetValue();
        Partner = partner;
        Users = await UserService.GetUsers();
        SelectedUserId = partner.User_ID_FK;
        ImageDataUrl = GetImageDataUrlFromBytes(Partner.Image);
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
                var result = await PartnerService.CreatePartner(Partner);
                if (result)
                {
                    await NotificationService.Info("Added Partner successfully.");
                    await HideModal();
                    await OnUpdatePartner.InvokeAsync();
                }
                else
                {
                    await NotificationService.Error("An error occurred please try again.");
                }
            }
            else
            {
                var result = await PartnerService.UpdatePartner(Partner);
                if (result)
                {
                    await NotificationService.Info("Added Partner successfully.");
                    await HideModal();
                    await OnUpdatePartner.InvokeAsync();
                }
                else
                {
                    await NotificationService.Error("An error occurred please try again.");
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
            Partner.Image = imageBytes;
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var imageDataUrl = $"data:image/png;base64,{imageBase64}";

            return imageDataUrl;
        }
    }
    void ValidateDate(ValidatorEventArgs e)
    {
        if (e.Value == null)
            e.Status = ValidationStatus.Error;
        else
        {
            var joinDate = DateTime.Parse(e.Value.ToString());
            e.Status = joinDate.Date.Equals(DateTime.MinValue.Date) || (IsAdded && joinDate.Date < DateTime.Now.Date) ? ValidationStatus.Error : ValidationStatus.Success;
        }
    }
    void ValidateSelectedUser(ValidatorEventArgs e)
    {
        if (e.Value == null)
            e.Status = ValidationStatus.Error;
        else
        {
            e.Status = SelectedUserId != Guid.Empty ? ValidationStatus.Success : ValidationStatus.Error;
        }
    }
    private void OnUserChanged(Guid userId)
    {
        SelectedUserId = userId;
    }
    private void ResetValue()
    {
        SelectedUserId = Guid.Empty;
        ImageDataUrl = string.Empty;
    }
}
