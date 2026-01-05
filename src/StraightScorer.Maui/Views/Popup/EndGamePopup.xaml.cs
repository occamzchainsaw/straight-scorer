using Mopups.Pages;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Views.Popup;

public partial class EndGamePopup : PopupPage
{
	public EndGamePopup(GameViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}