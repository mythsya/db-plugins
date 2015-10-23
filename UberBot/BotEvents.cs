using System;
using System.Linq;
using System.Threading;
using Zeta.Game;
using Zeta.Common;
using UberBot.Classes;
using UberBot.Helpers;
using Zeta.Bot;
using Zeta.Bot.Navigation;
using Zeta.Game.Internals.Actors;
using System.Collections.Generic;

namespace UberBot
{
    public class UberBotEvents
    {

        public static void UberBotOnItemLooted(object sender, ItemLootedEventArgs e)
        {	
	        try
            {
                if (UberRun.UberOrgansSNOs.Contains(e.Item.ActorSNO))
                    UberOrgans.TotalDropCount[UberBot.MyRunInfos.CurrentProfile - 1]++;
            }
            catch { }
        }
		
		public static void UberBotOnGameJoined(object scr, EventArgs mea)
        {
            if (!ProfileHelper.IsCurrentProfile("UberBot"))
                return;

            ProfileHelper.MyUsedProfiles.Clear();
		}

		public static void UberBotOnGameLeft(object scr, EventArgs mea)
        {
            if (!ProfileHelper.IsCurrentProfile("UberBot"))
                return;

			DebugLogging.Log("[OnGameLeft] Loader");
			ProfileHelper.LoadProfile("Loader");
			ProfileHelper.MyUsedProfiles.Clear();
        }
    }
}
