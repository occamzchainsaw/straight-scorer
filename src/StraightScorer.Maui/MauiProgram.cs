using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using StraightScorer.Core.Services;
using StraightScorer.Core.Services.Interfaces;
using StraightScorer.Maui.Pages;
using StraightScorer.Maui.Services;
using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.ViewModels;
using StraightScorer.Maui.Views.Popup;

namespace StraightScorer.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureMopups()
            .ConfigureFonts(fonts =>
            {
				fonts.AddFont("Montserrat-Regular.ttf", "FontRegular");
				fonts.AddFont("Montserrat-Semibold.ttf", "FontSemibold");
				fonts.AddFont("Montserrat-Bold.ttf", "FontBold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        builder.Services.AddSingleton<GameState>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<IUndoRedoService, UndoRedoService>();
		builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
		var settingsService = new SettingsService();
		builder.Services.AddSingleton(settingsService);
		builder.Services.AddSingleton<IGameSettings>(settingsService);
		builder.Services.AddSingleton<IMatchHistoryService, SqliteMatchHistoryService>();

		builder.Services.AddTransient<GamePage>();
		builder.Services.AddTransient<SetupPage>();
		builder.Services.AddTransient<SettingsPage>();
		builder.Services.AddTransient<MatchHistoryPage>();

		builder.Services.AddTransient<EndGamePopup>();

		builder.Services.AddTransient<SetupViewModel>();
		builder.Services.AddTransient<GameViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		builder.Services.AddTransient<MatchHistoryViewModel>();

		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CleanEntry", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.Background = null;
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
			handler.PlatformView.SetPadding(0, 0, 0, 0);
#elif WINDOWS
			handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(8,12,8,12);
			handler.PlatformView.MinWidth = 45;
			handler.PlatformView.MinHeight = 0;
			handler.PlatformView.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
#endif
        });

        return builder.Build();
	}
}
