using System;
using System.Collections.Generic;
using System.ServiceModel;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Party;

namespace SimpleFollow.Network
{
    internal class ServiceBase
    {
        internal static Message Leader
        {
            get { return SimpleFollow.Leader; }
            set { SimpleFollow.Leader = value; }
        }

        internal static Dictionary<int, Message> Followers
        {
            get { return SimpleFollow.Followers; }
            set { SimpleFollow.Followers = value; }
        }

        internal static bool IsFollower
        {
            get { return SimpleFollow.IsFollower; }
        }

        private static int _basePort = 10920;

        public static int BasePort
        {
            get { return _basePort; }
            set { _basePort = value; }
        }

        private static Uri _serverUri = new Uri("http://localhost:10920");

        public static Uri ServerUri
        {
            get { return _serverUri; }
            set { _serverUri = value; }
        }

        public static ServiceHost Host { get; set; }

        // Client
        private static ChannelFactory<IFollowService> _httpFactory;

        internal static ChannelFactory<IFollowService> HttpFactory
        {
            get { return ServiceBase._httpFactory; }
            set { ServiceBase._httpFactory = value; }
        }

        private static IFollowService _httpProxy;

        internal static IFollowService HttpProxy
        {
            get { return ServiceBase._httpProxy; }
            set { ServiceBase._httpProxy = value; }
        }

        private static bool _initialized;

        public static bool Initialized
        {
            get { return _initialized; }
            internal set { _initialized = value; }
        }

        public static bool Enabled
        {
            get { return Plugin.Enabled; }
        }

        /// <summary>
        /// Pulsing this will update both the follower and leader messages.
        /// </summary>
        internal static void Communicate()
        {
            if (Enabled)
            {
                if (!IsFollower)
                {
                    LeaderService.BehaviorWatcher();
                    LeaderService.ServerUpdate();
                }
                else
                {
                    FollowerService.AsyncClientUpdate();
                }
            }
            else
            {
                Logr.Log("Error - Main pulsed but plugin is disabled!");
                SharedComposites.CheckReplaceOutOfGameHook();
            }
        }
    }
}