using StraightScorer.Core.Services;

namespace StraightScorer.Maui.Views;

public class ScoreViewSelector : DataTemplateSelector
{
    public DataTemplate? HeadToHeadTemplate { get; set; }
    public DataTemplate? ListTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is GameState state)
        {
            return state.Players.Count == 2 ? HeadToHeadTemplate! : ListTemplate!;
        }
        return ListTemplate!;
    }
}
