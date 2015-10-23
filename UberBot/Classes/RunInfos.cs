using UberBot.Classes;
using UberBot.Helpers;
using Zeta.Game;
using Zeta.Bot;
using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace UberBot.Classes
{
    public class RunInfos
    {
        public int GameCount { get; set; }
        public List<int> ProfileCount = new List<int> { 0, 0, 0, 0 };
		public int DeathCount { get; set; }
		public int DeathByRunCount { get; set; }
        public DateTime RunTimer { get; set; }
        public TimeSpan RunElapsed { get; set; }
        public int CurrentLevelID { get; set; }
        public int CurrentWorldID { get; set; }
        public int CurrentProfile = 0;
		public int LastProfile { get; set; }

        public RunInfos()
        {
            RunTimer = DateTime.MinValue;
            RunElapsed = TimeSpan.MinValue;
            CurrentLevelID = 0;
		    CurrentWorldID = 0;
		    CurrentProfile = 0;
			LastProfile = 0;
        }
    }
}
