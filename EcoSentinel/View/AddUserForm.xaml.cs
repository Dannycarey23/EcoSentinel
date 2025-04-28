using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class AddUserForm : ContentPage
{
    public AddUserForm()
    {
        InitializeComponent();
        BindingContext = new AddUserFormViewModel();
    }
}
