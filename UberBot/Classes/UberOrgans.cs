using UberBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Zeta.Common;
using Zeta.Game;
using Zeta.Bot.Navigation;
using Zeta.Game.Internals.Actors;

namespace UberBot.Classes
{
    public class UberRun
    {
        private static Vector3 BlankVector = new Vector3(0, 0, 0);

        private static readonly List<int> GoldSNOs = new List<int> { 4311, 4312 };
        private static readonly List<int> UberWorldIdSNOs = new List<int> { 256111, 256112, 256607, 365973 };
        public static readonly List<int> UberOrgansSNOs = new List<int> { 364722, 364723, 364724, 364725 };

        private static bool IsInUberWorld { get { try { return UberWorldIdSNOs.Contains(ZetaDia.CurrentWorldId); } catch { return false; } } }
        private static bool IsGoldOnFloor { get { try { return ZetaDia.Actors.GetActorsOfType<DiaItem>(true).Any(i => GoldSNOs.Contains(i.ActorSNO)); } catch { return false; } } }
        private static bool CombatIsDone { get { return IsInUberWorld && (IsGoldOnFloor || IsOrganOnFloor || IsLegendaryOnFloor); } }
        private static bool IsInTransition { get { return ProfileHelper.IsCurrentProfile("UberBotLoader.xml") || ProfileHelper.IsCurrentProfile("UberBotLeaveGame.xml") || ProfileHelper.IsCurrentProfile("UberBotGetMachines.xml"); } }

        private static DateTime IsOnLootDeclaration = DateTime.MinValue;
        private static bool WaitForLootIsFinished { get { return DateTime.Now.Subtract(IsOnLootDeclaration).TotalSeconds > 4; } }
        private static bool LootIsDone { get { return IsInUberWorld && IsOnLoot && !IsOrganOnFloor && !IsLegendaryOnFloor && WaitForLootIsFinished; } }
   
        public static void ProcessCheck()
        {
            if (IsInTransition)
                return;

            if (CombatIsDone)
                ChangeLootState(true);

            if (LootIsDone)
            {
                ChangeLootState(false);
                ProfileHelper.LoadProfile("Loader");
            }

            if (!IsInUberWorld)
                return;

            OrganCheck();
            LegendaryCheck();
        }

        private static bool IsOrganOnFloor { get { try { return ZetaDia.Actors.GetActorsOfType<DiaItem>(true).Any(i => UberOrgansSNOs.Contains(i.ActorSNO)); } catch { return false; } } }
        private static DiaItem OrganObject { get { try { return ZetaDia.Actors.GetActorsOfType<DiaItem>().FirstOrDefault(i => UberOrgansSNOs.Contains(i.ActorSNO)); } catch { return null; } } }
        private static bool OrganDropped = false;
        private static void OrganCheck()
        {
            if (IsOrganOnFloor)
            {
                if (!OrganDropped)
                {
                    OrganDropped = true;

                    Logging.Log("Uber Organ dropped !");
                    DebugLogging.Log("[OrganCheck] " + ItemState(OrganObject));
                }

                if (OrganObject != null && OrganObject.Position != BlankVector)
                    Navigator.MoveTo(OrganObject.Position, ItemState(OrganObject));
            }
            else if (OrganDropped)
            {
                Logging.Log("Uber Organ acquired !");
                OrganDropped = false;
            }
        }

        private static bool IsLegendaryOnFloor { get { try { return ZetaDia.Actors.GetActorsOfType<DiaItem>(true).Any(i => i.CommonData != null && i.CommonData.ItemQualityLevel == ItemQuality.Legendary && !UberOrgansSNOs.Contains(i.ActorSNO)); } catch { return false; } } }
        private static DiaItem LegendaryObject { get { try { return ZetaDia.Actors.GetActorsOfType<DiaItem>().FirstOrDefault(i => i.CommonData != null && i.CommonData.ItemQualityLevel == ItemQuality.Legendary && !UberOrgansSNOs.Contains(i.ActorSNO)); } catch { return null; } } }
        private static bool LegendaryDropped = false;
        private static void LegendaryCheck()
        {
            if (IsLegendaryOnFloor)
            {
                if (!LegendaryDropped)
                {
                    LegendaryDropped = true;

                    Logging.Log("Legendary dropped !");
                    DebugLogging.Log("[LegendaryCheck] " + ItemState(LegendaryObject));
                }

                if (OrganObject != null && OrganObject.Position != BlankVector)
                    Navigator.MoveTo(OrganObject.Position, ItemState(LegendaryObject));
            }
            else if (LegendaryDropped)
            {
                Logging.Log("Legendary acquired !");
                LegendaryDropped = false;
            }
        }

        private static string ItemState(DiaItem item)
        {
            try
            {
                if (item != null)
                    return "Name=" + item.Name.ToString() +
                        ", Position=" + item.Position.ToString() +
                        ", Distance=" + item.Position.Distance(OrganObject.Position).ToString();
                else
                    return "itemObject null";
            }
            catch { return "itemObject error"; }
        }

        private static bool IsOnLoot = false;
        public static void ChangeLootState(bool state)
        {
            if (state && !IsOnLoot)
            {
                Logging.Log("UberBosses vanquished !");

                ProfileHelper.MyUsedProfiles.Add(UberBot.MyRunInfos.CurrentProfile);
                UberBot.MyRunInfos.ProfileCount[UberBot.MyRunInfos.CurrentProfile - 1]++;
                IsOnLootDeclaration = DateTime.Now;

                IsOnLoot = state;
            }
            else if (!state && IsOnLoot)
                IsOnLoot = state;
        }
    }

    public class UberOrgans
    {
        public static List<int> OrgansCount = new List<int> { 0, 0, 0, 0 };
        public static List<int> TotalDropCount = new List<int> { 0, 0, 0, 0 };

        public static int LeoricsRegretCount = 0;
        public static int VialofPutridnessCount = 0;
        public static int IdolofTerrorCount = 0;
        public static int HeartofEvilCount = 0;

        public static bool IsOrganSNO(int sno)
        {
            return UberRun.UberOrgansSNOs.Any(k => k == sno);
        }

        public static void AddToOrgansCount(int sno, int increment)
        {
            if (!IsOrganSNO(sno) || increment < 1) return;

            switch (UberRun.UberOrgansSNOs.IndexOf(sno))
            {
                case 0:
					LeoricsRegretCount += increment;
					OrgansCount[0] = LeoricsRegretCount;
                    break;
                case 1:
					VialofPutridnessCount += increment;
					OrgansCount[1] = VialofPutridnessCount;
                    break;
                case 2:
					IdolofTerrorCount += increment;
					OrgansCount[2] = IdolofTerrorCount;
                    break;
                case 3:
					HeartofEvilCount += increment;
					OrgansCount[3] = HeartofEvilCount;
                    break;
            }
        }

        public static void Refresh()
        {
			try
			{
				LeoricsRegretCount = 0;
				VialofPutridnessCount = 0;
				IdolofTerrorCount = 0;
				HeartofEvilCount = 0;

				ZetaDia.Me.Inventory.StashItems
					.Where(i => IsOrganSNO(i.ActorSNO))
					.ForEach(i => AddToOrgansCount(i.ActorSNO, (int)i.ItemStackQuantity));

				ZetaDia.Me.Inventory.Backpack
					.Where(i => IsOrganSNO(i.ActorSNO))
					.ForEach(i => AddToOrgansCount(i.ActorSNO, (int)i.ItemStackQuantity));

                ProfileHelper.ScanDisableProfile();
                LogCount();
			}
			catch { }
        }

        public static void LogCount()
        {
            Logging.Log(true, false, "|  STATS          |");
            Logging.Log(true, !IsAvailableRun(0), "|  BONES         |   Organs: " + LeoricsRegretCount.ToString("D3") + "   |   Machines: " + InfernalMachines.InfernalMachinesCount[0].ToString("D3") + "   |   Runs: " + UberBot.MyRunInfos.ProfileCount[0].ToString("D3") + "   |   DropRate: " + DropRate(0));
            Logging.Log(true, !IsAvailableRun(1), "|  GLUTTONY  |   Organs: " + VialofPutridnessCount.ToString("D3") + "   |   Machines: " + InfernalMachines.InfernalMachinesCount[1].ToString("D3") + "   |   Runs: " + UberBot.MyRunInfos.ProfileCount[1].ToString("D3") + "   |   DropRate: " + DropRate(1));
            Logging.Log(true, !IsAvailableRun(2), "|  WAR            |   Organs: " + IdolofTerrorCount.ToString("D3") + "   |   Machines: " + InfernalMachines.InfernalMachinesCount[2].ToString("D3") + "   |   Runs: " + UberBot.MyRunInfos.ProfileCount[2].ToString("D3") + "   |   DropRate: " + DropRate(2));
            Logging.Log(true, !IsAvailableRun(3), "|  EVIL             |   Organs: " + HeartofEvilCount.ToString("D3") + "   |   Machines: " + InfernalMachines.InfernalMachinesCount[3].ToString("D3") + "   |   Runs: " + UberBot.MyRunInfos.ProfileCount[3].ToString("D3") + "   |   DropRate: " + DropRate(3));
        }

        private static string DropRate(int organIndex)
        {
            if (UberBot.MyRunInfos.ProfileCount != null && UberBot.MyRunInfos.ProfileCount[organIndex] > 0)
                return (((double)UberOrgans.TotalDropCount[organIndex] / UberBot.MyRunInfos.ProfileCount[organIndex]) * 100).ToString("F0") + "%";
            
            return "-";
        }

        private static bool IsAvailableRun(int runIndex)
        {
            switch (runIndex)
            {
                case 0:
                    if (UberBotSettings.Instance.IgnoreLeoricsRegretEnabled || InfernalMachines.InfernalMachinesCount[0] <= 0)
                        return false;
                    else
                        return ProfileHelper.IsProfileAvailableToRun(runIndex + 1) && !ProfileHelper.MyUsedProfiles.Contains(runIndex + 1) && !ProfileHelper.MyDisableProfiles.Contains(runIndex + 1);
                case 1:
                    if (UberBotSettings.Instance.IgnoreIdolofTerrorEnabled || InfernalMachines.InfernalMachinesCount[1] <= 0)
                        return false;
                    else
                        return ProfileHelper.IsProfileAvailableToRun(runIndex + 1) && !ProfileHelper.MyUsedProfiles.Contains(runIndex + 1) && !ProfileHelper.MyDisableProfiles.Contains(runIndex + 1);
                case 2:
                    if (UberBotSettings.Instance.IgnoreVialofPutridnessEnabled || InfernalMachines.InfernalMachinesCount[2] <= 0)
                        return false;
                    else
                        return ProfileHelper.IsProfileAvailableToRun(runIndex + 1) && !ProfileHelper.MyUsedProfiles.Contains(runIndex + 1) && !ProfileHelper.MyDisableProfiles.Contains(runIndex + 1);
                case 3:
                    if (UberBotSettings.Instance.IgnoreHeartofEvilEnabled || InfernalMachines.InfernalMachinesCount[3] <= 0)
                        return false;
                    else
                        return ProfileHelper.IsProfileAvailableToRun(runIndex + 1) && !ProfileHelper.MyUsedProfiles.Contains(runIndex + 1) && !ProfileHelper.MyDisableProfiles.Contains(runIndex + 1);
            }
            return false;
        }
    }
}
