using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Domain.Commands
{
    public interface ICommand
    {
    }

    public interface ICommand<T> : ICommand
        where T : class
    {        
    }
}
