using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcoSentinel.Model;
using EcoSentinel.View;

namespace EcoSentinel.ViewModel;

public partial class LoginPageViewModel: ObservableObject
{
    [ObservableProperty]
    private string? username;
    [ObservableProperty]
    private string? password;
    [ObservableProperty]
    private string? loginMessage;
    private readonly UserService _userService;

    public LoginPageViewModel(UserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    private async Task Login()
    {
        await LoginAuthentication(Username, Password);
    }
    [RelayCommand]
    private void Register()
    {
        LoginMessage = "Redirecting to Registration Page...";
    }

    private async Task NavigateToLanding()
    {
        await Shell.Current.GoToAsync(nameof(LandingPage));
    }

    private async Task LoginAuthentication(string u, string p)
    {
        User user = new User();
        
        if(user.LoginAuthenticated(u,p))
        {
            CurrentlyLoggedInUser(u);
            await NavigateToLanding();
        }
        else
        {
            LoginMessage = "Invalid Username or Password entered!";
        }
    }

    private void CurrentlyLoggedInUser(string u)
    {
        _userService.username = u;
    }
    
}
