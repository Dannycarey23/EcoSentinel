using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    private void Login()
    {
        if(Username == "Admin" && Password == "EcoSentinel25")
        {
            LoginMessage = "Login Successful!";
        }
        else
        {
            LoginMessage = "Invalid Username or Password entered!";
        }
    }
    [RelayCommand]
    private void Register()
    {
        LoginMessage = "Redirecting to Registration Page...";
    }

}
