using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nx.Kernel
{
    public enum ComponentLifestyle
    {
        Transient,
        Singleton,
        PerSession,
        PerRequest
    }
}
