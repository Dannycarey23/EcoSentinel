using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class ResetPasswordForm : ContentPage
{
	public ResetPasswordForm()
	{
		InitializeComponent();
		BindingContext = new ResetPasswordFormViewModel();
	}
}