using StraightScorer.Maui.Services;

namespace StraightScorer.Maui;

public partial class App : Application
{
	private readonly AppShell _shell;
	public App(IServiceProvider serviceProvider, AppShell shell)
	{
		InitializeComponent();
		_shell = shell;

		var settings = serviceProvider.GetService<SettingsService>();
		if (settings != null)
		{
			UserAppTheme = settings.CurrentTheme;
		}
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
		return new Window(_shell);
    }
}