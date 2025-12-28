namespace StraightScorer.Maui.Services.Interfaces;

public interface IUndoRedoCommand
{
    void Execute();
    void Undo();
}