using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Domain.Commands
{
    public interface ICommandHandler<T>
        where T : class, ICommand
    {
        void HandleCommand(T command);
    }
}