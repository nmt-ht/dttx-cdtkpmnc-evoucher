using Blazorise;
using eVoucher.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace eVoucher.Pages.Users.Components;
public partial class AddEditUserModal : ComponentBase
{
    // reference to the modal component
    private Modal modalRef;
    [Parameter] public User User { get; set; }
    public void InitData()
    {
        ShowModal();
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