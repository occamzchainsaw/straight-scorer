using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services;

public class UndoRedoService : IUndoRedoService
{
    private readonly List<IUndoRedoCommand> _history = [];
    private const int _maxHistory = 100;

    public bool CanUndo => _history.Count > 0;

    public void ExecuteAndAdd(IUndoRedoCommand command)
    {
        command.Execute();
        _history.Add(command);

        if (_history.Count > _maxHistory)
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
}
