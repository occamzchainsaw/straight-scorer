using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace StraightScorer.Maui.Views;

public class MarkdownView : VerticalStackLayout
{
    // Dictionary to track heading IDs for internal anchor links
    private readonly Dictionary<string, Element> _headingMap = new();
    private AppTheme? _lastRenderedTheme;

    public static readonly BindableProperty MarkdownProperty =
        BindableProperty.Create(nameof(Markdown), typeof(string), typeof(MarkdownView), propertyChanged: OnMarkdownChanged);

    public string Markdown
    {
        get => (string)GetValue(MarkdownProperty);
        set => SetValue(MarkdownProperty, value);
    }

    public MarkdownView()
    {
        // Listen for theme changes to trigger a re-render
        Loaded += (s, e) =>
        {
            if (Application.Current != null)
            {
                // 1. Subscribe to live changes while we are visible
                Application.Current.RequestedThemeChanged += OnThemeChanged;

                // 2. Catch up: If the theme changed while we were unloaded, re-render now
                if (_lastRenderedTheme != null && _lastRenderedTheme != Application.Current.RequestedTheme)
                {
                    RenderMarkdown(Markdown);
                }
            }
        };

        Unloaded += (s, e) =>
        {
            if (Application.Current != null)
                Application.Current.RequestedThemeChanged -= OnThemeChanged;
        };
    }

    private void OnThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        // Re-render on the main thread when the theme changes
        MainThread.BeginInvokeOnMainThread(() => RenderMarkdown(Markdown));
    }

    private static void OnMarkdownChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MarkdownView view && newValue is string md)
        {
            view.RenderMarkdown(md);
        }
    }

    private void RenderMarkdown(string md)
    {
        if (Application.Current != null)
        {
            _lastRenderedTheme = Application.Current.RequestedTheme;
        }

        Children.Clear();
        _headingMap.Clear();
        Spacing = 10;
        Padding = new Thickness(0, 0, 0, 40);

        if (string.IsNullOrWhiteSpace(md)) return;

        // Use AdvancedExtensions for Tables and Auto-Links
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseAutoIdentifiers() // Crucial for anchor link mapping
            .UseEmphasisExtras()
            .Build();

        var document = Markdig.Markdown.Parse(md, pipeline);

        foreach (var block in document)
        {
            RenderBlock(block, 0, this.Children);
        }
    }

    private void RenderBlock(Block block, int indentationLevel, IList<IView> container)
    {
        switch (block)
        {
            case HeadingBlock heading:
                AddHeading(heading, container);
                break;
            case ParagraphBlock paragraph:
                AddParagraph(paragraph, indentationLevel, container);
                break;
            case ListBlock list:
                AddList(list, indentationLevel, container);
                break;
            case Table table:
                AddTable(table, container);
                break;
            case ThematicBreakBlock:
                AddDivider(container);
                break;
        }
    }

    private void AddHeading(HeadingBlock heading, IList<IView> container)
    {
        var text = GetContainerInlineText(heading.Inline);
        var label = new Label
        {
            FormattedText = GetFormattedText(heading.Inline),
            FontAttributes = FontAttributes.Bold,
            TextColor = GetThemeResource("Primary", Colors.White),
            Margin = new Thickness(0, 15, 0, 5),
            LineBreakMode = LineBreakMode.WordWrap
        };

        label.FontSize = heading.Level switch
        {
            1 => 26,
            2 => 20,
            3 => 18,
            _ => 16
        };

        // Map the heading ID for anchor links
        var id = heading.GetAttributes().Id;
        if (!string.IsNullOrEmpty(id))
        {
            _headingMap[id] = label;
        }

        container.Add(label);
    }

    private void AddParagraph(ParagraphBlock paragraph, int indentationLevel, IList<IView> container)
    {
        var label = new Label
        {
            FormattedText = GetFormattedText(paragraph.Inline),
            FontSize = 16,
            LineBreakMode = LineBreakMode.WordWrap,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Justify,
            Margin = new Thickness(indentationLevel * 20, 0, 0, 0),
            TextColor = GetThemeResource("Text", Colors.LightGray)
        };
        container.Add(label);
    }

    private void AddList(ListBlock list, int indentationLevel, IList<IView> container)
    {
        foreach (var item in list)
        {
            if (item is ListItemBlock listItem)
            {
                var row = new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
                    Margin = new Thickness(indentationLevel * 20, 2, 0, 2),
                    HorizontalOptions = LayoutOptions.Fill
                };

                var bulletText = list.IsOrdered ? $"{listItem.Order}." : "•";
                var bullet = new Label
                {
                    Text = bulletText,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 8, 0),
                    TextColor = GetThemeResource("Primary", Colors.Yellow), // Bullets match Primary theme
                    VerticalOptions = LayoutOptions.Start
                };

                var contentStack = new VerticalStackLayout { Spacing = 5, HorizontalOptions = LayoutOptions.Fill };
                foreach (var subBlock in listItem)
                {
                    RenderBlock(subBlock, indentationLevel + 1, contentStack.Children);
                }

                row.Add(bullet, 0);
                row.Add(contentStack, 1);
                container.Add(row);
            }
        }
    }

    private void AddTable(Table table, IList<IView> container)
    {
        var grid = new Grid
        {
            RowSpacing = 0,
            ColumnSpacing = 0,
            Margin = new Thickness(0, 10),
            BackgroundColor = GetThemeResource("TableBackground", Colors.DimGray),
            HorizontalOptions = LayoutOptions.Fill
        };

        int maxCols = 0;
        foreach (var row in table.OfType<TableRow>()) maxCols = Math.Max(maxCols, row.Count);
        for (int i = 0; i < maxCols; i++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        int rowIndex = 0;
        foreach (var row in table.OfType<TableRow>())
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            for (int colIndex = 0; colIndex < row.Count; colIndex++)
            {
                if (colIndex >= row.Count) continue;

                var cell = (TableCell)row[colIndex];
                var border = new Border
                {
                    Stroke = GetThemeResource("Gray", Colors.Gray),
                    StrokeThickness = 0.5,
                    Padding = 5,
                    BackgroundColor = Colors.Transparent
                };

                var cellStack = new VerticalStackLayout();
                foreach (var cellBlock in cell)
                {
                    if (cellBlock is ParagraphBlock p)
                    {
                        cellStack.Children.Add(new Label
                        {
                            FormattedText = GetFormattedText(p.Inline),
                            FontSize = 14,
                            LineBreakMode = LineBreakMode.WordWrap,
                            TextColor = GetThemeResource("Text", Colors.White),
                            FontAttributes = row.IsHeader ? FontAttributes.Bold : FontAttributes.None
                        });
                    }
                }

                border.Content = cellStack;
                grid.Add(border, colIndex, rowIndex);
            }
            rowIndex++;
        }
        container.Add(grid);
    }

    private FormattedString GetFormattedText(ContainerInline? container)
    {
        var formatted = new FormattedString();
        if (container == null) return formatted;

        foreach (var inline in container)
        {
            AddInlineToFormattedString(inline, formatted);
        }
        return formatted;
    }

    private void AddInlineToFormattedString(Inline inline, FormattedString formatted)
    {
        switch (inline)
        {
            case LiteralInline literal:
                formatted.Spans.Add(new Span { Text = literal.Content.ToString() });
                break;
            case EmphasisInline emphasis:
                formatted.Spans.Add(new Span
                {
                    Text = GetContainerInlineText(emphasis),
                    FontAttributes = emphasis.DelimiterCount == 2 ? FontAttributes.Bold : FontAttributes.Italic
                });
                break;
            case HtmlInline html when html.Tag.ToLower() == "<br>":
                formatted.Spans.Add(new Span { Text = "\n" });
                break;
            case LinkInline link:
                var span = new Span
                {
                    Text = GetContainerInlineText(link),
                    TextColor = GetThemeResource("LinkText", Colors.Cyan),
                    TextDecorations = TextDecorations.Underline
                };

                var tap = new TapGestureRecognizer();
                tap.Tapped += async (s, e) => {
                    if (string.IsNullOrEmpty(link.Url)) return;

                    if (link.Url.StartsWith("#"))
                    {
                        var targetId = link.Url.Substring(1);
                        if (_headingMap.TryGetValue(targetId, out var targetElement))
                        {
                            await ScrollToElement(targetElement);
                        }
                    }
                    else
                    {
                        await Launcher.Default.OpenAsync(link.Url);
                    }
                };
                span.GestureRecognizers.Add(tap);
                formatted.Spans.Add(span);
                break;
            case LineBreakInline:
                formatted.Spans.Add(new Span { Text = "\n" });
                break;
            case ContainerInline container:
                foreach (var subInline in container)
                    AddInlineToFormattedString(subInline, formatted);
                break;
        }
    }

    private async Task ScrollToElement(Element element)
    {
        // Traverse up to find the parent ScrollView
        Element parent = this.Parent;
        while (parent != null && parent is not ScrollView)
        {
            parent = parent.Parent;
        }

        if (parent is ScrollView scrollView)
        {
            await scrollView.ScrollToAsync(element, ScrollToPosition.Start, true);
        }
    }

    private string GetContainerInlineText(ContainerInline inline)
    {
        return string.Join("", inline.Select(i => i switch {
            LiteralInline lit => lit.Content.ToString(),
            ContainerInline ci => GetContainerInlineText(ci),
            _ => ""
        }));
    }

    private void AddDivider(IList<IView> container)
    {
        container.Add(new BoxView { HeightRequest = 1, BackgroundColor = GetThemeResource("Gray", Colors.Gray), Margin = new Thickness(0, 10) });
    }

    /// <summary>
    /// Look up resources based on current theme. 
    /// Matches the user's defined keys: Primary/PrimaryDark, LinkText/LinkTextDark, etc.
    /// </summary>
    private Color GetThemeResource(string key, Color defaultValue)
    {
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        string finalKey = isDark ? $"{key}Dark" : key;

        if (Application.Current?.Resources.TryGetValue(finalKey, out var val) == true && val is Color typedVal)
            return typedVal;

        // Fallback to base key if Dark version is missing
        if (Application.Current?.Resources.TryGetValue(key, out val) == true && val is Color baseVal)
            return baseVal;

        return defaultValue;
    }
}
