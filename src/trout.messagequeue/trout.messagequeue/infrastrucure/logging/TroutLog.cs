﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Appender;

namespace trout.messagequeue.infrastrucure.logging
{
    /// <summary>
    /// Log for Trout
    /// </summary>
    public static class TroutLog
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool isConfigured = false;

        /// <summary>
        /// ILog to use for logging
        /// </summary>
        public static ILog Log
        {
            get
            {
                if (!isConfigured)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    isConfigured = true;
                }

                return log;
            }
        }
    }
}
