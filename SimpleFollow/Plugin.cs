using System;
using System.Linq;
using System.Windows;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Network;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Common.Plugins;

namespace SimpleFollow
{
    public class Plugin : IPlugin
    {
        internal static bool Enabled = false;

        public Version Version
        {
            get { return SimpleFollow.PluginVersion; }
        }

        public void OnPulse()
        {
            SimpleFollow.Pulse();
        }

        public string Author
        {
            get { return "rrrix, TarasBulba"; }
        }

        public string Description
        {
            get { return "Co-op made Simple"; }
        }

        public Window DisplayWindow
        {
            get { return Config.GetDisplayWindow(); }
        }

        public const string PluginName = "SimpleFollow";

        public string Name
        {
            get { return PluginName; }
        }

        public void OnDisabled()
        {
            Logr.Log("Plugin disabled! ");
            Enabled = false;

            BotMain.OnStart -= BotMain_OnStart;
            BotMain.OnStop -= BotMain_OnStop;
            GameEvents.OnGameLeft -= GameEvents_OnGameLeft;
            GameEvents.OnGameJoined -= GameEvents_OnGameJoined;
            GameEvents.OnWorldTransferStart -= GameEvents_OnWorldTransferStart;
            GameEvents.OnWorldChanged -= GameEvents_OnWorldChanged;
            TreeHooks.Instance.OnHooksCleared -= OnHooksCleared;

            ServiceBase.Initialized = false;
            SharedComposites.CheckReplaceOutOfGameHook();

            try
            {
                if (ServiceBase.Host != null)
                {
                    ServiceBase.Host.Close();
                }
            }
            catch
            {
            }
        }

        public void OnEnabled()
        {
            Logr.Log("Plugin v{0} Enabled", Version);
            Enabled = true;

            BotMain.OnStart += BotMain_OnStart;
            BotMain.OnStop += BotMain_OnStop;
            GameEvents.OnGameLeft += GameEvents_OnGameLeft;
            GameEvents.OnGameJoined += GameEvents_OnGameJoined;
            GameEvents.OnWorldTransferStart += GameEvents_OnWorldTransferStart;
            GameEvents.OnWorldChanged += GameEvents_OnWorldChanged;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            SharedComposites.OutOfGameHookReplaced = false;
            SharedComposites.CheckReplaceOutOfGameHook();
            LeaderService.LeaderOutOfGameUpdate();

            ServiceBase.Communicate();
        }

        public void OnInitialize()
        {
        }

        public void OnShutdown()
        {
        }

        public bool Equals(IPlugin other)
        {
            return (other.Name == Name) && (other.Version == Version);
        }

        private void OnHooksCleared(object sender, EventArgs e)
        {
            LeaderComposite.ReplaceBotBehavior();
        }

        private void GameEvents_OnWorldChanged(object sender, EventArgs e)
        {
            SimpleFollow.LastChangedWorld = DateTime.UtcNow;
            SharedComposites.CheckReplaceOutOfGameHook();
            LeaderService.LeaderOutOfGameUpdate();

            ServiceBase.Communicate();
        }

        private void GameEvents_OnWorldTransferStart(object sender, EventArgs e)
        {
            SimpleFollow.LastChangedWorld = DateTime.UtcNow;
        }

        private void GameEvents_OnGameJoined(object sender, EventArgs e)
        {
            SimpleFollow.LastJoinedGame = DateTime.Now;
            SharedComposites.CheckReplaceOutOfGameHook();
            LeaderService.LeaderOutOfGameUpdate();

            ServiceBase.Communicate();
        }

        private void GameEvents_OnGameLeft(object sender, EventArgs e)
        {
            SharedComposites.OutOfGameHookReplaced = false;
            SharedComposites.CheckReplaceOutOfGameHook();
            LeaderService.LeaderOutOfGameUpdate();

            ServiceBase.Communicate();
        }

        private void BotMain_OnStop(IBot bot)
        {
        }

        /// <summary>
        /// Handles bot-start
        /// </summary>
        /// <param name="bot"></param>
        private void BotMain_OnStart(IBot bot)
        {
            LeaderComposite.ReplaceBotBehavior();
            if (Enabled)
            {
                Logr.Log("Bot Starting");
                SharedComposites.OutOfGameHookReplaced = false;
                SharedComposites.CheckReplaceOutOfGameHook();
                LeaderService.LeaderOutOfGameUpdate();

                ServiceBase.Communicate();
            }
        }

        internal static void DisablePlugin()
        {
            PluginManager.Plugins.Where(p => p.Plugin.Name == PluginName).ForEach(p => p.Enabled = false);
        }
    }
}