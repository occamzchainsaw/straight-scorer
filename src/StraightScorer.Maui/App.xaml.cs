using StraightScorer.Core.Services;

namespace StraightScorer.Maui;

public partial class App : Application
{
	private readonly AppShell _shell;
	public App(AppShell shell)
	{
		InitializeComponent();
		_shell = shell;
		// toremove
		Application.Current.UserAppTheme = AppTheme.Dark;
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
		return new Window(_shell);
    }
}