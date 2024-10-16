using CommunityToolkit.Maui;
using Core;
using UI.Services;
using UI.Utils;

namespace UI;

public static class MauiProgram {
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			//.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		builder.Services.AddSingleton<SettingsService>();
		//builder.Services.AddSingleton<IFolderPickerService, FolderPickerService>();

		
		Logger.AddConsole();
		Logger.AddFile(DataHandler.GetLogPath());

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		Logger.AddDebug();
#endif

		return builder.Build();
	}
}
