using System.IO;
using System.ComponentModel;
using System.Configuration;
using UberBot.Classes;
using UberBot.Helpers;
using Zeta.Game;
using Zeta.Common.Xml;

namespace UberBot
{
    [Zeta.XmlEngine.XmlElement("UberBotSettings")]
    class UberBotSettings : XmlSettings
    {
		private static UberBotSettings _instance;

        // Misc
		private bool _loggingEnabled;
        private bool _debugLoggingEnabled;
		// Run
        private bool _ignoreLeoricsRegretEnabled;
		private bool _ignoreIdolofTerrorEnabled;
		private bool _ignoreVialofPutridnessEnabled;
		private bool _ignoreHeartofEvilEnabled;
		private int _maxIntervalBtwOrgans;
		private int _maxDeathsAllowed;
		private bool _useFakeGoblinTrackEnabled;
		private bool _goblinPriorityPrioritize;
		private bool _goblinPriorityKamikaze;
		// TP
		private bool _instantTeleportationEnabled;
		// Loot
		private int _waitTimerForLoot;
		
         public static string BattleTagName
        {
            get
            {
                if (ZetaDia.Service.Hero.IsValid)
                    return ZetaDia.Service.Hero.BattleTagName;
                return "NotFound";
            }
        }

        public UberBotSettings() :
            base(Path.Combine(SettingsDirectory, "UberBot", BattleTagName, "UberBotSettings.xml"))
        {
        }

        public static UberBotSettings Instance
        {
            get { return _instance ?? (_instance = new UberBotSettings()); }
        }

        [Zeta.XmlEngine.XmlElement("LoggingEnabled")]
        [DefaultValue(true)]
        [Setting]
        public bool LoggingEnabled
        {
            get { return _loggingEnabled; }
            set
            {
                _loggingEnabled = value;
                OnPropertyChanged("LoggingEnabled");
            }
        }

        [Zeta.XmlEngine.XmlElement("DebugLoggingEnabled")]
        [DefaultValue(false)]
        [Setting]
        public bool DebugLoggingEnabled
        {
            get { return _debugLoggingEnabled; }
            set
            {
                _debugLoggingEnabled = value;
                OnPropertyChanged("DebugLoggingEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("IgnoreLeoricsRegretEnabled")]
        [DefaultValue(false)]
        [Setting]
        public bool IgnoreLeoricsRegretEnabled
        {
            get { return _ignoreLeoricsRegretEnabled; }
            set
            {
                _ignoreLeoricsRegretEnabled = value;
				OnPropertyChanged("IgnoreLeoricsRegretEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("IgnoreIdolofTerrorEnabled")]
        [DefaultValue(false)]
        [Setting]
        public bool IgnoreIdolofTerrorEnabled
        {
            get { return _ignoreIdolofTerrorEnabled; }
            set
            {
                _ignoreIdolofTerrorEnabled = value;
                OnPropertyChanged("IgnoreIdolofTerrorEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("IgnoreVialofPutridnessEnabled")]
        [DefaultValue(false)]
        [Setting]
        public bool IgnoreVialofPutridnessEnabled
        {
            get { return _ignoreVialofPutridnessEnabled; }
            set
            {
                _ignoreVialofPutridnessEnabled = value;
                OnPropertyChanged("IgnoreVialofPutridnessEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("IgnoreHeartofEvilEnabled")]
        [DefaultValue(false)]
        [Setting]
        public bool IgnoreHeartofEvilEnabled
        {
            get { return _ignoreHeartofEvilEnabled; }
            set
            {
                _ignoreHeartofEvilEnabled = value;
                OnPropertyChanged("IgnoreHeartofEvilEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("MaxIntervalBtwOrgans")]
        [DefaultValue(3)]
        [Setting]
        public int MaxIntervalBtwOrgans
        {
            get { return _maxIntervalBtwOrgans; }
            set
            {
				_maxIntervalBtwOrgans = value;
                OnPropertyChanged("MaxIntervalBtwOrgans");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("MaxDeathsAllowed")]
        [DefaultValue(3)]
        [Setting]
        public int MaxDeathsAllowed
        {
            get { return _maxDeathsAllowed; }
            set
            {
				if (value > 9)
					value = 0;
					
				_maxDeathsAllowed = value;
                OnPropertyChanged("MaxDeathsAllowed");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("UseFakeGoblinTrackEnabled")]
        [DefaultValue(true)]
        [Setting]
        public bool UseFakeGoblinTrackEnabled
        {
            get { return _useFakeGoblinTrackEnabled; }
            set
            {
                _useFakeGoblinTrackEnabled = value;
                OnPropertyChanged("UseFakeGoblinTrackEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("GoblinPriorityPrioritize")]
        [DefaultValue(true)]
        [Setting]
        public bool GoblinPriorityPrioritize
        {
            get { return _goblinPriorityPrioritize; }
            set
            {
				_goblinPriorityPrioritize = value;
				OnPropertyChanged("GoblinPriorityPrioritize");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("GoblinPriorityKamikaze")]
        [DefaultValue(false)]
        [Setting]
        public bool GoblinPriorityKamikaze
        {
            get { return _goblinPriorityKamikaze; }
            set
            {
				_goblinPriorityKamikaze = value;
				OnPropertyChanged("GoblinPriorityKamikaze");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("InstantTeleportationEnabled")]
        [DefaultValue(true)]
        [Setting]
        public bool InstantTeleportationEnabled
        {
            get { return _instantTeleportationEnabled; }
            set
            {
                _instantTeleportationEnabled = value;
                OnPropertyChanged("InstantTeleportationEnabled");
            }
        }
		
		[Zeta.XmlEngine.XmlElement("WaitTimerForLoot")]
        [DefaultValue(5)]
        [Setting]
        public int WaitTimerForLoot
        {
            get { return _waitTimerForLoot; }
            set
            {
                _waitTimerForLoot = value;
                OnPropertyChanged("WaitTimerForLoot");
            }
        }
	}
}
