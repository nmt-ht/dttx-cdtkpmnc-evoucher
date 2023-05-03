using eVoucher.Models;
using eVoucher.Pages.Users.Components;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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
        Users = await UserService.GetUsers();
        StateHasChanged();
    }

    private void ViewAddEdiUser()
    {
        addEditUserModal.InitData();
    }
}