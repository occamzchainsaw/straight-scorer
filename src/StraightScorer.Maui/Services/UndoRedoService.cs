using StraightScorer.Maui.Services.Interfaces;

namespace StraightScorer.Maui.Services;

public class UndoRedoService : IUndoRedoService
{
    private readonly List<IUndoRedoCommand> _history = [];
    private const int MaxHistory = 100;

    public void ExecuteAndAdd(IUndoRedoCommand command)
    {
        command.Execute();
        _history.Add(command);

        if (_history.Count > MaxHistory)
            _history.RemoveAt(0);
    }

    public void Undo(int steps = 1)
    {
        int actualSteps = Math.Min(steps, _history.Count);
        if (actualSteps <= 0)
            return;

        for (int i = 0; i < actualSteps; i++)
        {
            IUndoRedoCommand lastCommand = _history[^1];
            lastCommand.Undo();
            _history.RemoveAt(_history.Count - 1);
        }
    }

    public bool CanUndo => _history.Count > 0;
}