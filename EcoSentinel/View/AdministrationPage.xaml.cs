using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class AdministrationPage : ContentPage
{
	public AdministrationPage()
	{
		InitializeComponent();
		BindingContext = new AdministrationPageViewModel();
	}
}