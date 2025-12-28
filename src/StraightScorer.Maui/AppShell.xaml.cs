using StraightScorer.Maui.Pages;

namespace StraightScorer.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("setup", typeof(SetupPage));
		Routing.RegisterRoute("game", typeof(GamePage));
	}
}
