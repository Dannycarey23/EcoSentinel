using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class SensorStatusPage : ContentPage
{
	public SensorStatusPage()
	{
		InitializeComponent();
        BindingContext = new SensorStatusViewModel();
    }
}