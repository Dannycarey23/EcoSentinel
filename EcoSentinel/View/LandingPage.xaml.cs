using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class LandingPage : ContentPage
{
	public LandingPage()
	{
		InitializeComponent();
		BindingContext = new LandingPageViewModel();
	}

	public void airSensorFrameTapped(object sender, EventArgs e)
	{
		// Navigate to the WindSensorPage when the WindFrame is tapped
		Shell.Current.GoToAsync(nameof(AirSensorPage));
	}
}