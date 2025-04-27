using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace EcoSentinel.ViewModel;

public partial class DeleteUserFormViewModel : ObservableObject
{
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string role;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    public event Action<User>? OnSubmit;

    [RelayCommand]
    public async Task Submit()
    {
        OnSubmit?.Invoke(new User
        {
            username = Username,
            password = Password,
            role = Role,
            email = Email,
            fname = FirstName,
            lname = LastName
        });
        await Shell.Current.GoToAsync("//AdministrationPage"); // Navigate to AdministrationPage using Shell
    }

    [RelayCommand]
    public async Task Cancel()
    {
        await Shell.Current.GoToAsync("//AdministrationPage"); // Navigate to AdministrationPage using Shell
    }
}
