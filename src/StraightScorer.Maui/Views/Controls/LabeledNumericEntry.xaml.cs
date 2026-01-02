namespace StraightScorer.Maui.Views.Controls;

public partial class LabeledNumericEntry : ContentView
{
	public LabeledNumericEntry()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
		nameof(LabelText), typeof(string), typeof(LabeledNumericEntry), string.Empty);
	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(LabeledNumericEntry), string.Empty, defaultBindingMode: BindingMode.TwoWay);
	public static readonly BindableProperty MinValueProperty = BindableProperty.Create(
		nameof(MinValue), typeof(int), typeof(LabeledNumericEntry), 0);
	public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
		nameof(MaxValue), typeof(int), typeof(LabeledNumericEntry), 100);
    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
        nameof(IsValid), typeof(bool), typeof(LabeledNumericEntry), true);

    public string LabelText
	{
		get => (string)GetValue(LabelTextProperty);
		set => SetValue(LabelTextProperty, value);
	}
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}
	public int MinValue
	{
		get => (int)GetValue(MinValueProperty);
		set => SetValue(MinValueProperty, value);
	}
    public int MaxValue
    {
        get => (int)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }
    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
		InternalEntry.Focus();
    }
}