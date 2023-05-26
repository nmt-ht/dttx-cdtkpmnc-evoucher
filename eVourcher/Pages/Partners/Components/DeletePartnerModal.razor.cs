using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eVoucher.Models.DataType;

namespace eVoucher.Pages.Partners.Components;
public partial class DeletePartnerModal : ComponentBase
{
    private Modal deleteModalRef;
    [Parameter] public EventCallback OnDeletePartner { get; set; }

    public void InitData()
    {
        ShowModal();
    }

    private Task ShowModal()
    {
        return deleteModalRef.Show();
    }

    private Task HideModal()
    {
        return deleteModalRef.Hide();
    }
      
    private async Task DeletePartner()
    {
        await HideModal();
        await OnDeletePartner.InvokeAsync();
    }
}