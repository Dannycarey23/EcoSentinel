using EcoSentinel.View;
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
		Shell.Current.GoToAsync(nameof(AirSensorPage));
	}
	public void waterSensorFrameTapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync(nameof(WaterSensorPage));
	}

	public void weatherSensorFrameTapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync(nameof(WeatherSensorPage));
	}

	public void adminFrameTapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync(nameof(AdministrationPage));
	}

	public void sensorStatusFrameTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(SensorStatusPage));
    }
}