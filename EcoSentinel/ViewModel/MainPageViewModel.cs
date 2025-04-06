using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace EcoSentinel.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
   public ICommand NavigateCommand {get;}
    public MainPageViewModel()
    {
        NavigateCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(LandingPage));
        });
    }

         
}
