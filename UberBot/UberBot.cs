using UberBot.Classes;
using UberBot.Helpers;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Zeta.TreeSharp;
using Zeta.Bot;
using Zeta.Bot.Dungeons;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
using Zeta.Bot.Profile;
using Zeta.Common;
using Zeta.Common.Helpers;
using Zeta.Common.Plugins;
using System;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Zeta.XmlEngine;

namespace UberBot
{
    public class UberBot : IPlugin
    {
        private const string NAME = "UberBot";
        private const string AUTHOR = "Harvest";
        static readonly Version VERSION = new Version(0, 0, 0, 2);
        private const string DESCRIPTION = "暗黑兄弟网WWW.Demonbuddy.CN";

        public string Author { get { return AUTHOR; } }
        public string Description { get { return DESCRIPTION; } }
        public string Name { get { return NAME; } }
        public Version Version { get { return VERSION; } }
		public Window DisplayWindow { get { return UberBotConfig.GetDisplayWindow(); } }

        public bool Equals(IPlugin other)
        {
            return Name.Equals(other.Name) && Author.Equals(other.Author) && Version.Equals(other.Version);
        }
		
		public static bool IsPluginEnable = false;
		
        public static RunInfos MyRunInfos = new RunInfos();
		
        public void OnDisabled()
        {
			Logging.Log("v" + Version + " Disabled");

			GameEvents.OnGameLeft -= UberBotEvents.UberBotOnGameLeft;
			GameEvents.OnGameJoined -= UberBotEvents.UberBotOnGameJoined;
            GameEvents.OnItemLooted -= UberBotEvents.UberBotOnItemLooted;
			
			IsPluginEnable = false;
        }

        public void OnEnabled()
        {
			Logging.Log("v" + Version + " Enabled");

            GameEvents.OnGameLeft += UberBotEvents.UberBotOnGameLeft;
			GameEvents.OnGameJoined += UberBotEvents.UberBotOnGameJoined;
            GameEvents.OnItemLooted += UberBotEvents.UberBotOnItemLooted;
			
			IsPluginEnable = true;
        }

        public void OnInitialize()
        {		
			Logging.Log("Initialized v" + Version);
        }

        public void OnPulse()
        {
            if (!ZetaDia.IsInGame || !ZetaDia.Me.IsValid || !ZetaDia.CPlayer.IsValid || ZetaDia.Me.IsDead || ZetaDia.IsLoadingWorld)
                return;

            if (!ProfileHelper.IsCurrentProfile("UberBot"))
                return;

            if (!Timers.ProcessCheckInterval.IsFinished)
                return;

            UberRun.ProcessCheck();
            Timers.ProcessCheckInterval.Reset();
        }

        public void OnShutdown()
        {
			IsPluginEnable = false;
        }
    }

    class Timers
    {
        public static WaitTimer ProcessCheckInterval = new WaitTimer(TimeSpan.FromSeconds(0.2));
    }

	class UberBotConfig
    {
        private static Window _configWindow;
        public static void CloseWindow()
        {
            _configWindow.Close();
        }
        public static Window GetDisplayWindow()
        {
            if (_configWindow == null)
            {
                _configWindow = new Window {DataContext = UberBotSettings.Instance};
            }
            string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            if (assemblyPath != null)
            {
                string xamlPath = Path.Combine(assemblyPath, "Plugins", "UberBot", "UberBot.xaml");
                string xamlContent = File.ReadAllText(xamlPath);
                var mainControl = (UserControl)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)));
                _configWindow.Content = mainControl;
            }
            _configWindow.Width = 450;
            _configWindow.Height = 240;
            _configWindow.ResizeMode = ResizeMode.NoResize;
            _configWindow.Background = Brushes.DarkGray;
            _configWindow.Title = "UberBot - An advanced Organs Framer";
            _configWindow.Closed += ConfigWindowClosed;
            Application.Current.Exit += ConfigWindowClosed;
            return _configWindow;
        }
        static void ConfigWindowClosed(object sender, EventArgs e)
        {
			UberBotSettings.Instance.Save();
            if (_configWindow != null)
            {
				Logging.Log("Saving UberBot Settings");
				
                _configWindow.Closed -= ConfigWindowClosed;
                _configWindow = null;
            }
        }
    }
}
