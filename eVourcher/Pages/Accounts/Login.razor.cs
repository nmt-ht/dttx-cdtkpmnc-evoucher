using Blazorise;
using eVoucher.Models;
using eVourcher.Services;
using Microsoft.AspNetCore.Components;
using System;
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
    private User user = new User();
    private Modal modalRef;
    private bool showPassWordLogin = false;
    private bool showPasswordCreateAccount = false;
    private string typePassWordLogin;
    private string typePassWordCreateAccount;
    private string errorLogin;
    private string errorCreateAccount;
    public async void InitData()
    {
        email = string.Empty;
        password = string.Empty;
        errorLogin = errorCreateAccount = string.Empty;
        typePassWordLogin = typePassWordCreateAccount = "password";
        user = new User();
        user.DateOfBirth = DateTime.Now;
        await loginValidationsRef.ClearAll();
        await registerValidationsRef.ClearAll();
        await ShowModal();
        StateHasChanged();
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
        StateHasChanged();
    }

    async Task OnRegisterClicked()
    {
        errorCreateAccount = string.Empty;
        if (await registerValidationsRef.ValidateAll())
        {
            user.EmailAddress = email;
            var result = await UserService.CreateUser(user);
            var userRegister = result.Item1;
            await registerValidationsRef.ClearAll();
            if(userRegister != null)
            {
                await NotificationService.Info("Register successfully.");
                await HideModal();
                await OnloginSuccess.InvokeAsync(userRegister);
            }
            else
            {
                errorCreateAccount = !string.IsNullOrEmpty(result.Item2) ? result.Item2 : "An error occurred please try again.";
            }    
        }
        StateHasChanged();
    }
}