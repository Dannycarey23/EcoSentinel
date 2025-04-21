using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    async Task NavigateToLanding()
    {
        await Shell.Current.GoToAsync(nameof(LandingPage));
    }

    private async Task LoginAuthentication(string u, string p)
    {
        User user = new User();
        user.username = "Admin";
        user.password = "EcoSentinel25";        
        user.role = "Admin";
        
        if(u == user.username  && p == user.password)
        {
            await NavigateToLanding();
        }
        else
        {
            LoginMessage = "Invalid Username or Password entered!";
        }
    }
}
