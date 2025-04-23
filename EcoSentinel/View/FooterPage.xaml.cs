using EcoSentinel.Model;
using EcoSentinel.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace EcoSentinel.View;

public partial class FooterPage : ContentView
{
    public FooterPage()
    {
        InitializeComponent();
        var userService = App.Current.Handler.MauiContext.Services.GetService<UserService>();
        BindingContext = new FooterPageViewModel(userService);
    }
}
