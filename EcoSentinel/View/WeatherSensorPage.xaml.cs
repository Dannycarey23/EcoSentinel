using EcoSentinel.ViewModel;
namespace EcoSentinel;

public partial class WeatherSensorPage : ContentPage
{
	public WeatherSensorPage()
	{
		InitializeComponent();
		BindingContext = new WeatherSensorPageViewModel();
		
	}
}