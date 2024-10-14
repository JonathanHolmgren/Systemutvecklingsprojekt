using System.Windows.Input;

namespace PresentationLayer.Command;

public abstract class CommandBase : ICommand

{
    public abstract bool CanExecute(object parameter);
    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

}