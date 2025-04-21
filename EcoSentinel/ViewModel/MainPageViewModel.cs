using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.View;
namespace EcoSentinel.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
    public ICommand NavigateCommand {get;}
    public MainPageViewModel()
    {
        NavigateCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        });
    }

         
}
