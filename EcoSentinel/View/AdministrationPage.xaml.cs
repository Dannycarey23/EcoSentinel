using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class AdministrationPage : ContentPage
{
	public AdministrationPage(AdministrationPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}