using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVoucher.Pages;
public partial class UserView : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    private IList<User> Users { get; set; } = new List<User>();
 
    private User selectedUser;

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetUsers();
        StateHasChanged();
    }
}