using System;

namespace UberBot
{
    class Logging
    {
        private static readonly log4net.ILog Logger = Zeta.Common.Logger.GetLoggerInstanceForType();

        public static void Log(string message, Exception e)
        {
			if (UberBotSettings.Instance.LoggingEnabled)
			{
				Logger.Warn("[ UberBot ] " + message);
				Logger.Warn("[ UberBot ] " + e);
			}
        }
		
		public static void Log(string message)
        {
			if (UberBotSettings.Instance.LoggingEnabled)
				Logger.Warn("[ UberBot ] " + message);
        }

        public static void Log(bool withoutType, bool red, string message)
        {
            if (!UberBotSettings.Instance.LoggingEnabled)
                return;

            if (withoutType && red)
                Logger.Error(message);
            else if (withoutType && !red)
                Logger.Warn(message);
            else if (red)
                Logger.Error("[ UberBot ] " + message);
            else
                Logger.Warn("[ UberBot ] " + message);
        }
    }
	
	class DebugLogging
    {
        private static readonly log4net.ILog Logger = Zeta.Common.Logger.GetLoggerInstanceForType();

        public static void Log(string message)
        {
			if (UberBotSettings.Instance.DebugLoggingEnabled)
				Logger.Info("[ UberBot DEBUG ] " + message);
			else
				Logger.Debug("[ UberBot DEBUG ] " + message);
        }
		
		public static void Log(string message, Exception e)
        {
			if (UberBotSettings.Instance.DebugLoggingEnabled)
			{
                Logger.Info("[ UberBot DEBUG ] " + message);
                Logger.Info("[ UberBot DEBUG ] " + e);
			}
			else
			{
				Logger.Debug("[ UberBot DEBUG ] " + message);
				Logger.Debug("[ UberBot DEBUG ] " + e);
			}
        }
		
		public static void Log(Exception e)
        {
			
			if (UberBotSettings.Instance.DebugLoggingEnabled)
                Logger.Info("   UberBot DEBUG " + e);
			else
				Logger.Debug("   UberBot DEBUG " + e);
        }
    }
}
