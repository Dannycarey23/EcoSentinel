using Microsoft.Extensions.Logging;
using EcoSentinel.ViewModel;
using EcoSentinel.View;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using EcoSentinel.Model;



namespace EcoSentinel;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var a = Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream("EcoSentinel.appsettings.json");
			
		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();
			
		builder.Configuration.AddConfiguration(config);

		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		
		builder.Services.AddTransient<LandingPageViewModel>();
		builder.Services.AddTransient<LandingPage>();

		builder.Services.AddTransient<AirSensorPageViewModel>();
		builder.Services.AddTransient<AirSensorPage>();

		builder.Services.AddTransient<WaterSensorPageViewModel>();
		builder.Services.AddTransient<WaterSensorPage>();

		builder.Services.AddTransient<WeatherSensorPageViewModel>();
		builder.Services.AddTransient<WeatherSensorPage>();

		builder.Services.AddTransient<LoginPageViewModel>();
		builder.Services.AddTransient<LoginPage>();

		builder.Services.AddSingleton<UserService>();
		builder.Services.AddTransient<FooterPageViewModel>();

		builder.Services.AddTransient<AdministrationPageViewModel>();
		builder.Services.AddTransient<AdministrationPage>();
		
		builder.Services.AddTransient<HistoricalDataPageViewModel>();
		builder.Services.AddTransient<HistoricalDataPage>();
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
