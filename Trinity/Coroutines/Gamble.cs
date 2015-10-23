#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using TrinityCoroutines.Resources;
using Trinity.DbProvider;
using Trinity.Items;
using Trinity.Technicals;
using Zeta.Bot.Logic;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;

#endregion

namespace TrinityCoroutines
{
    public class Gamble
    {
        public static int TimeoutSeconds = 60;
        public static DateTime LastTimeStarted = DateTime.MinValue;
        private static DateTime _lastGambleTime = DateTime.MinValue;
        private static List<Town.VendorSlot> _gambleRotation = new List<Town.VendorSlot>();
        private static readonly Random Rnd = new Random();
        public static DateTime LastCanRunCheck = DateTime.MinValue;
        public static bool LastCanRunResult;

        public static void CheckShouldTownRunForGambling()
        {
            if (!ZetaDia.IsInTown)
                IsDumpingShards = false;

            if (Trinity.Trinity.Settings.Gambling.ShouldTownRun && ZetaDia.CPlayer.BloodshardCount >= Math.Min(Trinity.Trinity.Settings.Gambling.SaveShardsThreshold, Trinity.Trinity.Player.MaxBloodShards))
            {
                if (CanRun() && !ShouldSaveShards && !TownRun.IsTryingToTownPortal() && !BrainBehavior.IsVendoring)
                {
                    BrainBehavior.ForceTownrun("Bloodshard Spending Threshold");
                }
            }
        }

        /// <summary>
        /// If bot can actually purchase something from vendor right now.
        /// </summary>
        private static bool CanBuyItems
        {
            get
            {
                try
                {                                  
                    if (!ZetaDia.IsInTown || ZetaDia.WorldType != Act.OpenWorld || Trinity.Trinity.Player.IsCastingOrLoading)
                        return false;

                    if (!UIElements.VendorWindow.IsVisible || Town.Actors.Kadala == null)
                    {
                        LogVerbose("Vendor window is not open or can't find kadala or shes not close enough");
                        return false;
                    }

                    if (!TrinityItemManager.IsAnyTwoSlotBackpackLocation)
                    {
                        LogVerbose("No bag space");
                        return false;
                    }

                    if (ZetaDia.CPlayer.BloodshardCount < Trinity.Trinity.Settings.Gambling.MinimumBloodShards || !CanAffordMostExpensiveItem)
                    {
                        LogVerbose("Not enough shards!");
                        return false;
                    }

                }
                catch(Exception ex)
                {
                    Logger.LogError("Exception in Gamble.Execute, {0}", ex);

                    if (ex is CoroutineStoppedException)
                        throw;

                    return false;
                }

                LogVerbose("Can buy items!");
                    return true;
            }
        }

        public static async Task<bool> Execute()
        {
            if (!ZetaDia.IsInTown)
                IsDumpingShards = false;

            try
            {
                while (CanRun() && (!ShouldSaveShards || IsDumpingShards))
                {


                    IsDumpingShards = true;

                    var distance = Town.Locations.Kadala.Distance(ZetaDia.Me.Position);
                    if (distance > 8f && !await MoveToAndInteract.Execute(Town.Locations.Kadala, Town.ActorIds.Kadala, 3f))
                    {
                        Logger.Log("[Gamble] Failed to move to Kadala, quite unfortunate.");
                        break;
                    }

                    if (CanBuyItems)
                        await BuyItem();
                    else
                        Resources.GameUI.CloseVendorWindow();

                    if (!TrinityItemManager.IsAnyTwoSlotBackpackLocation)
                    {
                        BrainBehavior.ForceTownrun();
                    }

                    await Coroutine.Sleep(100);
                    await Coroutine.Yield();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError("Exception in Gamble.Execute, {0}", ex);

                if (ex is CoroutineStoppedException)
                    throw;
            }

            return true;           
        }

        private static async Task<bool> BuyItem()
        {
            try
            {

                if (!PurchaseDelayPassed)
                return false;

                if (!_gambleRotation.Any())
                    _gambleRotation = Trinity.Trinity.Settings.Gambling.SelectedGambleSlots;

                var slot = _gambleRotation[Rnd.Next(_gambleRotation.Count)];
                var itemId = Town.MysterySlotTypeAndId[slot];
                var item = ZetaDia.Actors.GetActorsOfType<ACDItem>(true).FirstOrDefault(a => a.ActorSNO == itemId);

                if (item == null)
                {
                    Logger.LogError("[Gamble] DB Error ACDItem == null Slot={0} Now buying random item to spend shards", slot);
                    var randomItem = ZetaDia.Actors.GetActorsOfType<ACDItem>().FirstOrDefault(a => a.InternalName.StartsWith("PH_"));
                    if (randomItem == null)
                        return true;

                    item = randomItem;
                }

                _gambleRotation.Remove(slot);
                ZetaDia.Me.Inventory.BuyItem(item.DynamicId);
                Logger.Log("[Gamble] Buying: {0}", slot);
                _lastGambleTime = DateTime.UtcNow;

            }
            catch(Exception ex)
            {
                Logger.LogError("Exception in Gamble.BuyItems, {0}", ex);

                if (ex is CoroutineStoppedException)
                    throw;
            }

            return false;
        }

        public static bool CanRun(bool ignoreSaveThreshold = false)
        {
            if (!ZetaDia.IsInGame)
                return false;

            try
            {
                if (ZetaDia.WorldType != Act.OpenWorld || Trinity.Trinity.Player.IsCastingOrLoading)
                {
                    return false;
                }

                if (Trinity.Trinity.Player.ParticipatingInTieredLootRun)
                {
                    LogVerbose("No gambling during greater rift due to backpack items being disabled ");
                    return false;
                }

                if (Trinity.Trinity.Settings.Gambling.SelectedGambleSlots.Count <= 0)
                {
                    LogVerbose("Select at least one thing to buy in settings");
                    return false;
                }

                if (BelowMinimumShards)
                {
                    LogVerbose("Not enough shards!");
                    return false;
                }

                if (!CanAffordMostExpensiveItem)
                {
                    LogVerbose("Can't afford desired items!");
                    return false;
                }

                if (!TrinityItemManager.IsAnyTwoSlotBackpackLocation || ZetaDia.Me.Inventory.NumFreeBackpackSlots < 5)
                {
                    LogVerbose("No Backpack space!");
                    return false;
                }


            }
            catch(Exception ex)
            {
                Logger.LogError("Exception in Gamble.BuyItems, {0}", ex);

                if (ex is CoroutineStoppedException)
                    throw;

                return false;
            } 

            LogVerbose("Should Gamble!");
            return true;
        }

        private static void LogVerbose(string msg, params object[] args)
        {
            var debugInfo = string.Format(" Shards={0} SaveShards={1} SaveThreshold={2} CanAffordItem={3} SelectedSlots={4}",
                ZetaDia.CPlayer.BloodshardCount,
                Trinity.Trinity.Settings.Gambling.ShouldSaveShards,
                Math.Min(Trinity.Trinity.Settings.Gambling.SaveShardsThreshold, Trinity.Trinity.Player.MaxBloodShards),
                CanAffordMostExpensiveItem,
                Trinity.Trinity.Settings.Gambling.SelectedGambleSlots.Count);

            Logger.LogVerbose("[Gamble]" + msg + debugInfo, args);
        }

        public static bool PurchaseDelayPassed
        {
            get
            {
                var timeSinceGamble = DateTime.UtcNow.Subtract(_lastGambleTime).TotalMilliseconds;
                return (_lastGambleTime == DateTime.MinValue || timeSinceGamble > Rnd.Next(50, 350));
            }
        }

        public static bool CanAffordMostExpensiveItem
        {
            get
            {
                var slotAndPrice = Town.MysterySlotTypeAndPrice.Where(pair => Trinity.Trinity.Settings.Gambling.SelectedGambleSlots.Contains(pair.Key)).ToList();
                return slotAndPrice.Any() && slotAndPrice.Max(pair => pair.Value) <= ZetaDia.CPlayer.BloodshardCount;
            }
        }

        private static bool IsDumpingShards { get; set; }

        private static bool BelowMinimumShards
        {
            get { return ZetaDia.CPlayer.BloodshardCount < Trinity.Trinity.Settings.Gambling.MinimumBloodShards; }
        }

        private static bool ShouldSaveShards
        {
            get
            {
                if (Trinity.Trinity.Settings.Gambling.ShouldSaveShards && ZetaDia.CPlayer.BloodshardCount < Math.Min(Trinity.Trinity.Settings.Gambling.SaveShardsThreshold, Trinity.Trinity.Player.MaxBloodShards))
                {
                    LogVerbose("Should Save Shards!");
                    return true;
                }
                return false;
            }
        }


    }
}