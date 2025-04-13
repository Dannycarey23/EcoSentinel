using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class WaterSensorPage : ContentPage
{
	public WaterSensorPage()
	{
		InitializeComponent();
		BindingContext = new WaterSensorPageViewModel();
	}
}