using System;

namespace GasPrice.Core.EventHandling
{
   
    public class DelegateCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Action<TCommand> action;
        public DelegateCommandHandler(Action<TCommand> action)
        {
            this.action = action;
        }

        public void Handle(TCommand cmd)
        {
            action(cmd);
        }
    }
}
