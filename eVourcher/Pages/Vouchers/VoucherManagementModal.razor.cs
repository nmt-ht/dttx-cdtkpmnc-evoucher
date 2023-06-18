using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages.Vouchers;

public partial class VoucherManagementModal : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    [Parameter] public Guid CurrentUserID { get; set; }
    private Modal modalRef;
    private IList<UserVoucher> UserVouchers { get; set; }

    public async Task BindData()
    {
        UserVouchers = await UserService.GetUserVouchers(CurrentUserID);
        int index = 0;
        UserVouchers.ToList().ForEach(uv => uv.Index = ++index);
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
}