using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class LandingPage : ContentPage
{
	public LandingPage()
	{
		InitializeComponent();
		BindingContext = new LandingPageViewModel();
	}
}