namespace StraightScorer.Maui.Pages;

public partial class RulesPage : ContentPage
{
	public RulesPage()
	{
		InitializeComponent();
        _ = LoadRulesAsync();
	}

    private async Task LoadRulesAsync()
    {
        try
        {
            // 1. Open the file from the Resources/Raw directory (MauiAsset)
            // Ensure your file is actually named 'rules.md' in that folder
            using var stream = await FileSystem.OpenAppPackageFileAsync("rules.md");

            // 2. Read the content using a StreamReader
            using var reader = new StreamReader(stream) ;
            string contents = await reader.ReadToEndAsync();

            // 3. Update the UI on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MarkdownViewer.Markdown = contents;
            });
        }
        catch (FileNotFoundException)
        {
            // Handle cases where the file might be missing or misnamed
            MarkdownViewer.Markdown = "# Error\nRules file not found in assets.";
        }
        catch (Exception ex)
        {
            MarkdownViewer.Markdown = $"# Error\nUnable to load rules: {ex.Message}";
        }
    }
}