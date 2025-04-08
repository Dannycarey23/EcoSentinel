using EcoSentinel.ViewModel;

namespace EcoSentinel;

public partial class MainPage : ContentPage
{
	
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel();
	}

}

