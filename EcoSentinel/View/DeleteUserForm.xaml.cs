using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class DeleteUserForm : ContentPage
{
	public DeleteUserForm()
	{
		InitializeComponent();
		BindingContext = new DeleteUserFormViewModel();
	}
}