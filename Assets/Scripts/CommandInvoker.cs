using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    private Stack<ICommand> _undoStack = new Stack<ICommand>();
    private Stack<ICommand> _redoStack = new Stack<ICommand>();
    public event Action<ICommand> OnCommandUndone;
    public event Action<ICommand> OnCommandRedone;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void UndoCommand()
    {
        if (_undoStack.Count > 0)
        {
            ICommand activeCommand = _undoStack.Pop();
            activeCommand.Undo();
            _redoStack.Push(activeCommand);
            OnCommandUndone?.Invoke(activeCommand);
        }

    }
    public void RedoCommand()
    {
        if (_redoStack.Count > 0)
        {
            ICommand activeCommand = _redoStack.Pop();
            activeCommand.Execute();
            _undoStack.Push(activeCommand);
            OnCommandRedone?.Invoke(activeCommand);
        }
    }
}
