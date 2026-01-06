using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}