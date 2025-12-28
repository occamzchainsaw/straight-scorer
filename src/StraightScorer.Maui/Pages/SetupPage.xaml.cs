using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Pages;

public partial class SetupPage : ContentPage
{
	public SetupPage(GameSetupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}