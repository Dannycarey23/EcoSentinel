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
    // public void OnWindFrameTapped(object sender, EventArgs e)
	// {
	// 	// Navigate to the WindSensorPage when the WindFrame is tapped
	// 	Shell.Current.GoToAsync(nameof(AirSensorPage));
      
	// }
}

