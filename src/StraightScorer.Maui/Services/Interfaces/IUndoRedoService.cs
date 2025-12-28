namespace StraightScorer.Maui.Services.Interfaces;

public interface IUndoRedoService
{
    void ExecuteAndAdd(IUndoRedoCommand command);
    void Undo(int steps = 1);
}