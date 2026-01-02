namespace StraightScorer.Maui.Views.Controls;

public partial class LabeledTextEntry : ContentView
{
	public LabeledTextEntry()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(LabelText), typeof(string), typeof(LabeledTextEntry), string.Empty);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(LabeledTextEntry), string.Empty, defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty MinLengthProperty = BindableProperty.Create(
        nameof(MinLength), typeof(int), typeof(LabeledTextEntry), 0);
    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength), typeof(int), typeof(LabeledTextEntry), 500);
    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
        nameof(IsValid), typeof(bool), typeof(LabeledTextEntry), true);

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
    public int MinLength
    {
        get => (int)GetValue(MinLengthProperty);
        set => SetValue(MinLengthProperty, value);
    }
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
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