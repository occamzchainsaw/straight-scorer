using HorusStudio.Maui.MaterialDesignControls;
using Microsoft.Extensions.Logging;
using StraightScorer.Maui.Services;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui;

public static class MauiProgram
{
	private const string FontRegular = "FontRegular";
	private const string FontSemibold = "FontSemibold";
	private const string FontBold = "FontBold";
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMaterialDesignControls(options =>
			{
				options.ConfigureFonts(fonts =>
				{
					fonts.AddFont("GoogleSans-Regular.ttf", FontRegular);
					fonts.AddFont("GoogleSans-SemiBold.ttf", FontSemibold);
					fonts.AddFont("GoogleSans-Bold.ttf", FontBold);
				}, new(FontRegular, FontSemibold, FontBold));
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<GameSession>();
		builder.Services.AddTransient<GameSetupViewModel>();
		builder.Services.AddTransient<GameViewModel>();

		return builder.Build();
	}
}
