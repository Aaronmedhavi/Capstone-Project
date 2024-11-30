using System.Collections.Generic;

public static class Invoker
{
    static Stack<ICommand> undoStack = new();
    public static int Count => undoStack.Count;
    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();
        undoStack.Push(command);
    }
    public static ICommand UndoCommand()
    {
        if (Count > 0)
        {
            ICommand activeCommand = undoStack.Pop();
            activeCommand.Undo();
            return activeCommand;
        }
        return null;
    }
}
