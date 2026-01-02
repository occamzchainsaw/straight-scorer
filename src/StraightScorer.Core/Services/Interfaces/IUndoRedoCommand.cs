namespace StraightScorer.Core.Services.Interfaces;

public interface IUndoRedoCommand
{
    void Execute();
    void Undo();
}
