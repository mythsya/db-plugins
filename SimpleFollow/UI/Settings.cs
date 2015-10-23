using System.ComponentModel;
using System.Configuration;
using System.IO;
using SimpleFollow.Network;
using Zeta.Common.Xml;
using Zeta.Game;
using Zeta.XmlEngine;

namespace SimpleFollow.UI
{
    [XmlElement("SimpleFollowSettings")]
    internal class Settings : XmlSettings
    {
        private static Settings _instance;
        private int serverPort;
        private bool inviteFriend1;
        private bool inviteFriend2;
        private bool inviteFriend3;
        private bool inviteFriend4;
        private bool debugLogging;
        private int updateInterval;
        private bool useProfilePosition;
        private bool stayInParty;
        private string bindAddress;
        private bool waitForFollowers;

        private static string _battleTagName;

        public static string BattleTagName
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(_battleTagName) && ZetaDia.Service.Hero.IsValid)
                        _battleTagName = ZetaDia.Service.Hero.BattleTagName;
                    else if (string.IsNullOrWhiteSpace(_battleTagName) && !ZetaDia.Service.Hero.IsValid)
                        _battleTagName = "";
                    return _battleTagName;
                }
                catch
                {
                    return "";
                }
            }
        }

        public Settings() :
            base(Path.Combine(SettingsDirectory, "SimpleFollow", BattleTagName, "SimpleFollowSettings.xml"))
        {
        }

        public static Settings Instance
        {
            get { return _instance ?? (_instance = new Settings()); }
        }

        [XmlElement("ServerPort")]
        [DefaultValue(10920)]
        [Setting]
        public int ServerPort
        {
            get { return serverPort; }
            set
            {
                if (serverPort != value)
                {
                    ServiceBase.Initialized = false;
                    serverPort = value;
                    OnPropertyChanged("ServerPort");
                }
            }
        }

        [XmlElement("InviteFriend1")]
        [DefaultValue(true)]
        [Setting]
        public bool InviteFriend1
        {
            get { return inviteFriend1; }
            set
            {
                inviteFriend1 = value;
                OnPropertyChanged("InviteFriend1");
            }
        }

        [XmlElement("InviteFriend2")]
        [DefaultValue(true)]
        [Setting]
        public bool InviteFriend2
        {
            get { return inviteFriend2; }
            set
            {
                inviteFriend2 = value;
                OnPropertyChanged("InviteFriend2");
            }
        }

        [XmlElement("InviteFriend3")]
        [DefaultValue(true)]
        [Setting]
        public bool InviteFriend3
        {
            get { return inviteFriend3; }
            set
            {
                inviteFriend3 = value;
                OnPropertyChanged("InviteFriend3");
            }
        }

        [XmlElement("InviteFriend4")]
        [DefaultValue(false)]
        [Setting]
        public bool InviteFriend4
        {
            get { return inviteFriend4; }
            set
            {
                inviteFriend4 = value;
                OnPropertyChanged("InviteFriend4");
            }
        }

        [XmlElement("DebugLogging")]
        [DefaultValue(true)]
        [Setting]
        public bool DebugLogging
        {
            get { return debugLogging; }
            set
            {
                debugLogging = value;
                OnPropertyChanged("DebugLogging");
            }
        }

        [XmlElement("UpdateInterval")]
        [DefaultValue(300)]
        [Setting]
        public int UpdateInterval
        {
            get { return updateInterval; }
            set
            {
                if (value >= 0)
                {
                    updateInterval = value;
                    OnPropertyChanged("UpdateInterval");
                }
            }
        }

        [XmlElement("UseProfilePosition")]
        [DefaultValue(false)]
        [Setting]
        public bool UseProfilePosition
        {
            get { return useProfilePosition; }
            set
            {
                useProfilePosition = value;
                OnPropertyChanged("UseProfilePosition");
            }
        }

        [XmlElement("StayInParty")]
        [DefaultValue(true)]
        [Setting]
        public bool StayInParty
        {
            get { return stayInParty; }
            set
            {
                stayInParty = value;
                OnPropertyChanged("StayInParty");
            }
        }

        [XmlElement("BindAddress")]
        [DefaultValue("localhost")]
        [Setting]
        public string BindAddress
        {
            get { return bindAddress; }
            set
            {
                if (bindAddress != value)
                {
                    ServiceBase.Initialized = false;
                    bindAddress = value;
                    OnPropertyChanged("BindAddress");
                }
            }
        }

        [XmlElement("WaitForFollowers")]
        [DefaultValue(true)]
        [Setting]
        public bool WaitForFollowers
        {
            get { return waitForFollowers; }
            set
            {
                if (waitForFollowers != value)
                {
                    waitForFollowers = value;
                    OnPropertyChanged("WaitForFollowers");
                }
            }
        }
    }
}