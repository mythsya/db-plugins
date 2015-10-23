using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using UberBot.Classes;

namespace UberBot.Helpers
{
    class ProfileHelper
    {
        public static HashSet<int> MyUsedProfiles = new HashSet<int>();
		public static HashSet<int> MyDisableProfiles = new HashSet<int>();
		
		public static string DataPath = string.Empty;
		public static string SThisProfileString = string.Empty;
        public static string XmlLeoricsRegretProfile = string.Empty;
        public static string XmlIdolofTerrorProfile = string.Empty;
        public static string XmlVialofPutridnessProfile = string.Empty;
        public static string XmlHeartofEvilProfile = string.Empty;
        public static string XmlLoaderProfile = string.Empty;

        public static void UberBotProfileManager()
        {			
            ZetaDia.Actors.Update();

            InfernalMachines.Refresh();
            bool machinesInStash = ZetaDia.Me.Inventory.StashItems.Any(i => InfernalMachines.IsInfernalMachineSNO(i.ActorSNO));
            if (ZetaDia.IsInTown && InfernalMachines.InfernalMachinesCount.Sum() <= 0)
            {
                if (machinesInStash && !ProfileHelper.IsCurrentProfile("UberBotGetMachines.xml"))
                {
                    Logging.Log(false, true, "No more Infernal Machines in Backpack, Get machines");
                    ProfileHelper.LoadProfile("UberBotGetMachines.xml");
                }
                else
                {
                    Logging.Log(false, true, "No more Infernal Machines, Stop bot");
                    BotMain.Stop();
                }
                return;
            }

            UberOrgans.Refresh();

			SThisProfileString = "";
            string logRun = "";
            bool isChooseProfile = false;
            for (int iChooseProfile = 1; iChooseProfile <= 6; iChooseProfile++)
            {
                if (iChooseProfile >= 5)
                    break;
                if (IsProfileAvailableToRun(iChooseProfile))
                {
                    switch (iChooseProfile)
                    {
                        case 1:
                            SThisProfileString = XmlLeoricsRegretProfile;
                            logRun = "Realm of Discord";
                            break;
                        case 2:
                            SThisProfileString = XmlVialofPutridnessProfile;
                            logRun = "Realm of Chaos";
                            break;
                        case 3:
                            SThisProfileString = XmlIdolofTerrorProfile;
                            logRun = "Realm of Turmoil";
                            break;
                        case 4:
                            SThisProfileString = XmlHeartofEvilProfile;
                            logRun = "Realm of Fright";
                            break;
                    }
                    if (!MyUsedProfiles.Contains(iChooseProfile) && !MyDisableProfiles.Contains(iChooseProfile))
                    {
                        UberBot.MyRunInfos.CurrentProfile = iChooseProfile;
                        isChooseProfile = true;
                        break;
                    }
                }
            }
			
            if (isChooseProfile)
            {
                Logging.Log("Profile Manager, Next run on " + logRun);	
                LoadProfile(SThisProfileString);
            }
            else
            {
				Logging.Log("Profile Manager, Leave Game: All available profiles are used");
                LeaveGame();
            }
        }

        public static bool IsProfileAvailableToRun(int act)
        {
            int maxOrgans = UberOrgans.OrgansCount.IndexOf(UberOrgans.OrgansCount.Max()) + 1;
            bool equalOrgans = UberOrgans.OrgansCount.Min() == UberOrgans.OrgansCount.Max();

            if ((UberOrgans.OrgansCount[act - 1] != UberOrgans.OrgansCount[maxOrgans - 1] || equalOrgans) && InfernalMachines.InfernalMachinesCount[act - 1] >= 1)
                return true;

            return false;
        }

		public static void LeaveGame()
        {
            DebugLogging.Log("LeaveGame");

            LoadProfile("UberBotLeaveGame.xml");

            MyUsedProfiles.Clear();
			UberBot.MyRunInfos.GameCount++;
			UberBot.MyRunInfos.LastProfile = 0;
		}

        public static void LoadProfile(string profile)
        {
            if (IsCurrentProfile(profile))
                return;

            string sProfilePath = string.Empty;
            if (profile.Contains("Loader"))
                sProfilePath = XmlLoaderProfile;
            else
                sProfilePath = DataPath + @"\" + profile;

            if (sProfilePath == null || profile == null)
            {
                DebugLogging.Log("[LoadProfile] Failed to Load Profile, file: " + sProfilePath);
                return;
            }

            try
            {
                DebugLogging.Log("[LoadProfile] Load Profile, file: " + sProfilePath);
                ProfileManager.Load(sProfilePath);
            }
            catch { }
        }

        public static bool IsCurrentProfile(string profile)
        {
            try { return ProfileManager.CurrentProfile.Path.Contains(profile); }
            catch { return false; }
        }

		public static void ScanDisableProfile()
		{
			MyDisableProfiles.Clear();
			
			if (UberBotSettings.Instance.IgnoreLeoricsRegretEnabled || InfernalMachines.InfernalMachinesCount[0] <= 0)
			{
				MyDisableProfiles.Add(1);
                UberOrgans.OrgansCount[0] = UberOrgans.OrgansCount[UberOrgans.OrgansCount.IndexOf(UberOrgans.OrgansCount.Max())];
			}
			if (UberBotSettings.Instance.IgnoreIdolofTerrorEnabled || InfernalMachines.InfernalMachinesCount[1] <= 0)
			{
				MyDisableProfiles.Add(2);
                UberOrgans.OrgansCount[1] = UberOrgans.OrgansCount[UberOrgans.OrgansCount.IndexOf(UberOrgans.OrgansCount.Max())];
			}
			if (UberBotSettings.Instance.IgnoreVialofPutridnessEnabled || InfernalMachines.InfernalMachinesCount[2] <= 0)
			{
				MyDisableProfiles.Add(3);
                UberOrgans.OrgansCount[2] = UberOrgans.OrgansCount[UberOrgans.OrgansCount.IndexOf(UberOrgans.OrgansCount.Max())];
			}
			if (UberBotSettings.Instance.IgnoreHeartofEvilEnabled || InfernalMachines.InfernalMachinesCount[3] <= 0)
			{
				MyDisableProfiles.Add(4);
                UberOrgans.OrgansCount[3] = UberOrgans.OrgansCount[UberOrgans.OrgansCount.IndexOf(UberOrgans.OrgansCount.Max())];
			}
		}
    }
}
