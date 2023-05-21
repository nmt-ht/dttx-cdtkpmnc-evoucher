using Blazorise;
using eVoucher.Models;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVoucher.Pages.Users;
public partial class UserView : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    private IList<User> Users { get; set; } = new List<User>();

    private User selectedUser;
    private AddEditUserModal addEditUserModal;

    protected override async Task OnInitializedAsync()
    {
        await ShowLoadingPage(true);
        Users = await UserService.GetUsers();
        var index = 0;
        Users.ToList().ForEach(x => x.Index = ++index);
        await ShowLoadingPage(false);
    }

    private void ViewAddEdiUser()
    {
        addEditUserModal.InitData();
    }

    #region Show Loading page
    [Inject] IPageProgressService PageProgressService { get; set; }
    private async Task ShowLoadingPage(bool isShow)
    {
        if (isShow)
        {
            await SetPageProgressIndeterminate();
        }
        else { await SetPageProgressHidden(); }
        StateHasChanged();
    }
    Task SetPageProgressIndeterminate()
    {
        return PageProgressService.Go(null, options => { options.Color = Color.Info; });
    }
    Task SetPageProgressHidden()
    {
        // setting it to -1 will hide the progress bar
        return PageProgressService.Go(-1);
    }
    #endregion

}