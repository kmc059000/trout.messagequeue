using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace trout.messagequeue.infrastrucure.logging
{
    public static class TroutLog
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log
        {
            get { return log; }
        }
    }
}
