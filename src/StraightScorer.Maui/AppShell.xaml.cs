using StraightScorer.Core.Services;
using StraightScorer.Maui.Pages;

namespace StraightScorer.Maui;

public partial class AppShell : Shell
{
	public GameState State { get; }

	public AppShell(GameState state)
	{
		InitializeComponent();
		State = state;
		BindingContext = this;
		Routing.RegisterRoute("game", typeof(GamePage));
	}
}
