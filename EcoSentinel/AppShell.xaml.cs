namespace EcoSentinel;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
	}
}
