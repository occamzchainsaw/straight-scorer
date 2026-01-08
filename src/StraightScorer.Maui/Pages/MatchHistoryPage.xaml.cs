using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Pages;

public partial class MatchHistoryPage : ContentPage
{
	private readonly MatchHistoryViewModel _viewModel;

	public MatchHistoryPage(MatchHistoryViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.LoadHistoryCommand.Execute(null);
    }
}