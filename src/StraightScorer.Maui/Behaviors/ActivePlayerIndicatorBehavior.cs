namespace StraightScorer.Maui.Behaviors;

public class ActivePlayerIndicatorBehavior : Behavior<VisualElement>
{
    private const int AnimationDuration = 500;
    private VisualElement? _associatedElement;

    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
            nameof(IsActive), 
            typeof(bool), 
            typeof(ActivePlayerIndicatorBehavior), 
            false,
            propertyChanged: OnIsActiveChanged);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        _associatedElement = bindable;

        this.BindingContext = bindable.BindingContext;
        bindable.BindingContextChanged += OnAssociatedBindingContextChanged;

        _associatedElement.Opacity = IsActive ? 1.0 : 0.0;
    }

    private void OnAssociatedBindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is VisualElement visual)
        {
            this.BindingContext = visual.BindingContext;
        }
    }

    private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ActivePlayerIndicatorBehavior behavior && behavior._associatedElement != null)
        {
            _ = behavior.Animate((bool)newValue);
        }
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BindingContextChanged -= OnAssociatedBindingContextChanged;
        _associatedElement = null;
    }

    private async Task Animate(bool isActive)
    {
        if (_associatedElement is null) return;

        _associatedElement.CancelAnimations();

        if (isActive)
        {
            await _associatedElement.FadeToAsync(1.0, AnimationDuration, Easing.CubicOut);
        }
        else
        {
            await _associatedElement.FadeToAsync(0.0, AnimationDuration, Easing.CubicIn);
        }
    }
}
