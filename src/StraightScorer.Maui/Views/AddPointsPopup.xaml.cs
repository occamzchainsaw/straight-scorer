using Mopups.Pages;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Views;

public partial class AddPointsPopup : PopupPage
{
	public AddPointsPopup(GameViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}