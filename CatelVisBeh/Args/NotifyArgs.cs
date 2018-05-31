using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Args
{
    public class NotifyArgs : EventArgs
    {
        public NotifyArgs()
        {
        }

        public NotifyArgs(bool hasAccess)
        {
            HasAccess = hasAccess;
        }

        public bool HasAccess { get; set; }
    }
}
