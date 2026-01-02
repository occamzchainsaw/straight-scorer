using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Pages;

public partial class SetupPage : ContentPage
{
	public SetupPage(SetupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}