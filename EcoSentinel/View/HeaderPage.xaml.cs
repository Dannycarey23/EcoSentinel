using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace EcoSentinel.View;

public partial class HeaderPage : ContentView
{
	public ICommand NavigateHome {get;}
	public HeaderPage()
	{
		InitializeComponent();
		NavigateHome = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(MainPage));
        });
	}
}