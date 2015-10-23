using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Threading.Tasks;
using System.Windows.Documents;
using QuestTools;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Party;
using SimpleFollow.UI;
using QuestTools = QuestTools.QuestTools;

namespace SimpleFollow.Network
{
    internal class FollowerService : ServiceBase
    {
        /// <summary>
        /// Initializes the Follower connection to the Leader
        /// </summary>
        private static void StartClient()
        {
            try
            {
                if (!Initialized && Enabled)
                {
                    int serverPort = Settings.Instance.ServerPort;
                    ServerUri = new Uri(ServerUri.AbsoluteUri.Replace(BasePort.ToString(), serverPort.ToString()));

                    SharedComposites.CheckReplaceOutOfGameHook();

                    Logr.Log("Initializing Client Service connection to {0}", ServerUri.AbsoluteUri + "Follow");

                    BasicHttpBinding binding = new BasicHttpBinding
                    {
                        OpenTimeout = TimeSpan.FromMilliseconds(5000),
                        SendTimeout = TimeSpan.FromMilliseconds(5000),
                        CloseTimeout = TimeSpan.FromMilliseconds(5000)
                    };

                    EndpointAddress endPointAddress = new EndpointAddress(ServerUri.AbsoluteUri + "Follow");

                    HttpFactory = new ChannelFactory<IFollowService>(binding, endPointAddress);

                    HttpProxy = HttpFactory.CreateChannel();

                    Initialized = true;
                }
            }
            catch (Exception ex)
            {
                Logr.Log("Exception in ClientInitialize() {0}", ex);
            }
        }

        internal static Message LastLeaderUpdateMessage;

        private static bool _updateRunning;

        private static Message _lastMessage;

        internal static void AsyncClientUpdate()
        {
            _lastMessage = Message.GetMessage();
            if (!_updateRunning)
                new Task(ClientUpdate).Start();
        }

        /// <summary>
        /// Called by followers through FollowTag->Communicate()
        /// </summary>
        private static void ClientUpdate()
        {
            if (Enabled)
            {
                _updateRunning = true;
                try
                {
                    if (Host != null && Host.State == CommunicationState.Opened)
                    {
                        Logr.Log("Shutting down Server Service");
                        Host.Close();
                        Host = null;
                    }
                }
                catch (Exception ex)
                {
                    Logr.Error("Error shutting down server service: " + ex);
                }

                StartClient();

                try
                {
                    if (Initialized && Leader.GetMillisecondsSinceLastUpdate() >= 250)
                    {
                        // Get the leader message and store it 
                        Leader = HttpProxy.GetUpdate();

                        // Send our follower message to the leader
                        HttpProxy.SendUpdate(_lastMessage);

                        if (Settings.Instance.UseHotSpots && Leader.HotSpot != null)
                            Trinity.SetTrinityHotSpot(Leader.HotSpot);

                        if (LastLeaderUpdateMessage == null || LastLeaderUpdateMessage != Leader)
                        {
                            LastLeaderUpdateMessage = Leader;

                            SetQuestToolsOptionsFromLeader();

                            if (Settings.Instance.DebugLogging)
                                Logr.Debug("Leader {0}", Leader.ToString());
                        }
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    Logr.Error("Error 201: Could not get an update from the leader using {0}. Is the leader running? ({1})", HttpFactory.Endpoint.Address.Uri.AbsoluteUri, ex.Message);
                    Initialized = false;
                }
                catch (CommunicationException ex)
                {
                    Logr.Error("Error 202: Could not get an update from the leader using {0}. Is the leader running? ({1})", HttpFactory.Endpoint.Address.Uri.AbsoluteUri, ex.Message);
                    Initialized = false;
                }
                catch (Exception ex)
                {
                    Logr.Error("Error 203: Could not get an update from the leader using {0}. Is the leader running?", HttpFactory.Endpoint.Address.Uri.AbsoluteUri);
                    Initialized = false;
                    Logr.Log(ex.ToString());
                }
                _updateRunning = false;
            }
        }

        private static void SetQuestToolsOptionsFromLeader()
        {
            bool questToolsSettingsChanged = false;

            if (Leader.RiftKeyPriority == null || Leader.MinimumGemChance == 0f || Leader.LimitRiftLevel == 0 || Leader.TrialRiftMaxLevel == 0)
                return;
            string thingsThatChanged = "";
            // Force followers to use leader's rift key priority
            if (!QuestToolsSettings.Instance.RiftKeyPriority.SequenceEqual(Leader.RiftKeyPriority))
            {
                QuestToolsSettings.Instance.RiftKeyPriority = Leader.RiftKeyPriority;
                questToolsSettingsChanged = true;
                thingsThatChanged += "RiftKeyPriority,";
                thingsThatChanged = Leader.RiftKeyPriority.Aggregate(thingsThatChanged, (current, k) => current + (k + "-"));
                thingsThatChanged += " ";
                thingsThatChanged = QuestToolsSettings.Instance.RiftKeyPriority.Aggregate(thingsThatChanged, (current, k) => current + (k + "-"));
            }

            if (QuestToolsSettings.Instance.UseHighestKeystone != Leader.UseHighestKeystone)
            {
                QuestToolsSettings.Instance.UseHighestKeystone = Leader.UseHighestKeystone;
                questToolsSettingsChanged = true;
                thingsThatChanged += "UseHighestKeystone,";
            }

            if (QuestToolsSettings.Instance.MinimumGemChance != Leader.MinimumGemChance)
            {
                QuestToolsSettings.Instance.MinimumGemChance = Leader.MinimumGemChance;
                questToolsSettingsChanged = true;
                thingsThatChanged += "MinimumGemChance,";
            }

            if (QuestToolsSettings.Instance.EnableLimitRiftLevel != Leader.EnableLimitRiftLevel)
            {
                QuestToolsSettings.Instance.EnableLimitRiftLevel = Leader.EnableLimitRiftLevel;
                questToolsSettingsChanged = true;
                thingsThatChanged += "EnableLimitRiftLevel,";
            }

            if (QuestToolsSettings.Instance.LimitRiftLevel != Leader.LimitRiftLevel)
            {
                QuestToolsSettings.Instance.LimitRiftLevel = Leader.LimitRiftLevel;
                questToolsSettingsChanged = true;
                thingsThatChanged += "LimitRiftLevel,";
            }

            if (QuestToolsSettings.Instance.EnableTrialRiftMaxLevel != Leader.EnableTrialRiftMaxLevel)
            {
                QuestToolsSettings.Instance.EnableTrialRiftMaxLevel = Leader.EnableTrialRiftMaxLevel;
                questToolsSettingsChanged = true;
                thingsThatChanged += "EnableTrialRiftMaxLevel,";
            }

            if (QuestToolsSettings.Instance.TrialRiftMaxLevel != Leader.TrialRiftMaxLevel)
            {
                QuestToolsSettings.Instance.TrialRiftMaxLevel = Leader.TrialRiftMaxLevel;
                questToolsSettingsChanged = true;
                thingsThatChanged += "TrialRiftMaxLevel,";
            }

            if (questToolsSettingsChanged)
            {
                Logr.Log("Updated QuestTools Settings From Leader: " + thingsThatChanged);
                QuestToolsSettings.Instance.Save();
            }
        }
    }
}