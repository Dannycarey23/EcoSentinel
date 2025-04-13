using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class AirSensorPage : ContentPage
{
	public AirSensorPage()
	{
		InitializeComponent();
		BindingContext = new AirSensorPageViewModel();
	}
}