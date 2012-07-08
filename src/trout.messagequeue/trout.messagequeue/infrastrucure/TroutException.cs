using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trout.messagequeue.infrastrucure
{
    public class TroutException : Exception
    {
        public TroutException()
        {
        }

        public TroutException(string message) : base(message)
        {
            
        }

        public TroutException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
