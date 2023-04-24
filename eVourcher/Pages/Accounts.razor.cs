using eVoucher.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace eVoucher.Pages;
public partial class Accounts : ComponentBase
{
    public Account Account { get; set; }
    private List<Account> AccountData { get; set; } = new List<Account>
    {
        new Account
        {
            ID = new Guid(),
            FirstName = "Tu",
            LastName = "Nguyen"
        }
    };
    private Account selectedAccount;

    protected override async Task OnInitializedAsync()
    {
        //await base.OnInitializedAsync();
    }
}