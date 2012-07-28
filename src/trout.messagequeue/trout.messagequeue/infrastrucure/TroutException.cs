using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trout.messagequeue.infrastrucure
{
    /// <summary>
    /// Base exception for trout
    /// </summary>
    public class TroutException : Exception
    {
        /// <summary>
        /// Constructor for Trout
        /// </summary>
        public TroutException()
        {
        }

        /// <summary>
        /// Constructor for trout
        /// </summary>
        /// <param name="message"></param>
        public TroutException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Constructor for trout
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TroutException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
