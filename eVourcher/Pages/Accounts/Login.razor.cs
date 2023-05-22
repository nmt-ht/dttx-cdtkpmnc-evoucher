using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace eVoucher.Pages.Accounts;
public partial class Login : ComponentBase
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Parameter] public EventCallback<User> OnloginSuccess { get; set; }

    string selectedTab = "login";

    private Validations loginValidationsRef;
    private Validations registerValidationsRef;
    private string email;
    private string password;
    private User user;
    private Modal modalRef;
    private bool showPassWordLogin = false;
    private bool showPasswordCreateAccount = false;
    private string typePassWordLogin;
    private string typePassWordCreateAccount;
    private string errorLogin;
    private string errorCreateAccount;

    protected override async Task OnInitializedAsync()
    {
        errorLogin = errorCreateAccount = string.Empty;
        typePassWordLogin = typePassWordCreateAccount = "password";
        user = new User();
        user.DateOfBirth = DateTime.Now;
    }
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
    private void ShowPassWordLogin()
    {
        showPassWordLogin = !showPassWordLogin;
        if (!showPassWordLogin)
        {
            typePassWordLogin = "password";
        }
        else
        {
            typePassWordLogin = "text";
        }
    }
    private void ShowPassWordCreateAccount()
    {
        showPasswordCreateAccount = !showPasswordCreateAccount;
        if (!showPasswordCreateAccount)
        {
            typePassWordCreateAccount = "password";
        }
        else
        {
            typePassWordCreateAccount = "text";
        }
    }
    private Task OnSelectedTabChanged(string name)
    {
        selectedTab = name;

        return Task.CompletedTask;
    }
    async Task OnLoginClicked()
    {
        errorLogin = string.Empty;
        if (await loginValidationsRef.ValidateAll())
        {
            User userLogin = await UserService.Login(email, password);
            await loginValidationsRef.ClearAll();

            if (userLogin != null)
            {
                await NotificationService.Info("Login successfully.");
                await HideModal();
                await OnloginSuccess.InvokeAsync(userLogin);
            }
            else
            {
                errorLogin = "The email address or password is incorrect. Please try again.";
            }
        }
    }

    async Task OnRegisterClicked()
    {
        errorCreateAccount = string.Empty;
        if (await registerValidationsRef.ValidateAll())
        {
            User userRegister = await UserService.CreateUser(user);
            await registerValidationsRef.ClearAll();
            if(userRegister != null)
            {
                await NotificationService.Info("Register successfully.");
                await HideModal();
                await OnloginSuccess.InvokeAsync(userRegister);
            }
            else
            {
                errorCreateAccount = "An error occurred please try again.";
            }    
        }
    }
}