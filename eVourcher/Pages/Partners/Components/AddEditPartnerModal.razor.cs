using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Campaigns.Components;
using eVoucher.Pages.Games.Components;
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
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Partners.Components;
public partial class AddEditPartnerModal : ComponentBase
{
    [Inject] public IPartnerService PartnerService { get; set; }
    [Inject] public IUserService UserService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }
    [Parameter] public EventCallback OnUpdatePartner { get; set; }

    private Validations validationsRef;
    private AddEditPartnerCampaignModal addEditPartnerCampaignModal;
    public Partner Partner { get; set; } = new Partner();
    private Modal modalRef;
    private bool IsAdded => Partner is not null && Partner.Id != Guid.Empty ? false : true;
    private string Title => IsAdded ? "Add Partner" : "Edit Partner";
    private IList<User> Users { get; set; }
    private Guid SelectedUserId { get; set; }
    private PartnerCampaign SelectedPartnerCampaign { get; set; }
    public IList<Guid> CampaignIds => Partner != null && Partner.PartnerCampaigns != null ? Partner.PartnerCampaigns.Select(x => x.Campaign.Id).ToList() : new List<Guid>();

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

    private bool IsEditingGame = false;
    private async Task CampaignActions(eAction action)
    {
        IsEditingGame = false;
        switch (action)
        {
            case eAction.Add:
                addEditPartnerCampaignModal.SetParameters(new PartnerCampaign(), true, CampaignIds);
                addEditPartnerCampaignModal.InitData();
                break;
            case eAction.Edit:
                EditPartnerCampaign();
                break;
            case eAction.Delete:
                await DeletePartnerCampaign();
                break;
        }
    }
    private void EditPartnerCampaign()
    {
        IsEditingGame = true;
        addEditPartnerCampaignModal.SetParameters(SelectedPartnerCampaign, false, CampaignIds);
        addEditPartnerCampaignModal.InitData();
    }

    private async Task DeletePartnerCampaign()
    {
        var confirm = await MessageService.Confirm("Are you sure delete this game?");
        if (confirm)
        {
            if (SelectedPartnerCampaign != null)// Delete on memory only
            {
                Partner.PartnerCampaigns.Remove(SelectedPartnerCampaign);
                if (Partner.PartnerCampaigns is not null && Partner.PartnerCampaigns.Any())
                {
                    var index = 0;
                    Partner.PartnerCampaigns.ToList().ForEach(a => a.Index = ++index);
                }
            }
        }
    }

    private async Task OnUpdatePartnerCampaignCallBack(PartnerCampaign partnerCampaign)
    {
        if (!IsEditingGame)
        {
            var maxIndex = Partner.PartnerCampaigns.Count + 1;
            partnerCampaign.Index = maxIndex;
            Partner.PartnerCampaigns.Add(partnerCampaign);
        }
        else
        {
            SelectedPartnerCampaign = partnerCampaign;
        }
        StateHasChanged();
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
