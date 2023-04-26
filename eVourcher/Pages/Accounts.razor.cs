using eVoucher.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using eVourcher.Services;

namespace eVoucher.Pages;
public partial class Accounts : ComponentBase
{
    private IAccountService _accountService { get; set; }
    public Account Account { get; set; }
    private IList<Account> AccountList { get; set; } = new List<Account>
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
        AccountList = await _accountService.GetAccounts();
    }
}