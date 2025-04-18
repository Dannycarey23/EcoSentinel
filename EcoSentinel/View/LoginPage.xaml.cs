using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginPageViewModel();
	}
}