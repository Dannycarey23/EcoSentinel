using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


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


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
