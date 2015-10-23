using System.Diagnostics;
using SimpleFollow.UI;
using Zeta.Common;

namespace SimpleFollow.Helpers
{
    [DebuggerStepThrough]
    internal class Logr
    {
        private static readonly log4net.ILog logger = Logger.GetLoggerInstanceForType();
        private static string _lastDebugMessage = "";

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        [DebuggerStepThrough]
        internal static void Debug(string message, params object[] args)
        {
            if (!Settings.Instance.DebugLogging)
                return;

            string msg = string.Format(message, args);
            if (msg != _lastDebugMessage)
            {
                _lastDebugMessage = msg;
                logger.DebugFormat("[SimpleFollow] " + message, args);
            }
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        internal static void Debug(string message)
        {
            Debug(message, 0);
        }

        private static string _lastLogMessage = "";

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        [DebuggerStepThrough]
        internal static void Log(string message, params object[] args)
        {
            string msg = string.Format(message, args);
            if (msg != _lastLogMessage)
            {
                _lastLogMessage = msg;
                logger.InfoFormat("[SimpleFollow] " + message, args);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        internal static void Log(string message)
        {
            Log(message, 0);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        [DebuggerStepThrough]
        internal static void Error(string message, params object[] args)
        {
            string msg = string.Format(message, args);
            if (msg != _lastLogMessage)
            {
                _lastLogMessage = msg;
                logger.ErrorFormat("[SimpleFollow] " + message, args);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        internal static void Error(string message)
        {
            Log(message, 0);
        }
    }
}