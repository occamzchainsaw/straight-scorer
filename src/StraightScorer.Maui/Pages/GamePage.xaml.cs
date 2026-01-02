using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Pages;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
	}
}