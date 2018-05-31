using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Roles
{
    [Flags]
    public enum Roles
    {
        None = 0,
        Admin = 1,
        Slave = 2,
        Pupy = 4,
        Devil = 8, 
        Angel = 16,
        Moir = 32
    };
}
