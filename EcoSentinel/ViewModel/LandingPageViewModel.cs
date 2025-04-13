using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace EcoSentinel.ViewModel;

public partial class LandingPageViewModel : ObservableObject
{
   public ICommand NavigateCommand {get;}
    public LandingPageViewModel()
    {
        NavigateCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(LandingPage));
        });
    }
   
}

