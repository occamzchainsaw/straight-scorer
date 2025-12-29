using HorusStudio.Maui.MaterialDesignControls;
using Mopups.Pages;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Views;

public partial class EditPlayerPopup : PopupPage
{
    public EditPlayerPopup(GameSetupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    private async Task TextFieldFocused(object sender, FocusEventArgs e)
    {
		await Task.Delay(50);

		if (sender is not MaterialTextField textField) return;

		textField.CursorPosition = 0;
		textField.InternalEntry!.SelectionLength = textField.Text?.Length ?? 0;
    }
}