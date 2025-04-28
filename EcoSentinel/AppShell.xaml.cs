using EcoSentinel.View;
namespace EcoSentinel;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(AirSensorPage), typeof(AirSensorPage));
		Routing.RegisterRoute(nameof(WaterSensorPage), typeof(WaterSensorPage));
		Routing.RegisterRoute(nameof(WeatherSensorPage), typeof(WeatherSensorPage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(AdministrationPage), typeof(AdministrationPage));
		Routing.RegisterRoute(nameof(SensorStatusPage), typeof(SensorStatusPage));
    }
}
