namespace StraightScorer.Maui.Views;

public partial class PageWithTopBar : ContentView
{
	public static readonly BindableProperty PageContentProperty = BindableProperty.Create(
		nameof(PageContent),
		typeof(View),
		typeof(PageWithTopBar),
		propertyChanged: (bindable, _, newValue) =>
		{
			if (bindable is PageWithTopBar self && newValue is View view)
			{
				self.contentPresenter.Content = view;
			}
		});

	public View? PageContent
	{
		get => (View?)GetValue(PageContentProperty);
		set => SetValue(PageContentProperty, value);
	}

	public PageWithTopBar()
	{
		InitializeComponent();
	}
}