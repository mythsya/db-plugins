using System;
using System.Collections.Generic;
using Buddy.Coroutines;
using Trinity.DbProvider;
using Zeta.Bot;
using Zeta.Game.Internals.Actors;

namespace Trinity.Items
{
    public class ItemEvents
    {
        internal static void TrinityOnItemStashed(object sender, ItemEventArgs e)
        {
            ResetTownRun();

            try
            {
                ACDItem i = e.Item;

                if (i == null || !i.IsValid || i.IsDisposed)
                    return;

                var cachedItem = CachedACDItem.GetCachedItem(i);

                switch (i.ItemBaseType)
                {
                    case ItemBaseType.Gem:
                    case ItemBaseType.Misc:
                        break;
                    default:
                        TownRun.LogGoodItems(cachedItem, cachedItem.TrinityItemBaseType, cachedItem.TrinityItemType, ItemValuation.ValueThisItem(cachedItem, cachedItem.TrinityItemType));
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is CoroutineStoppedException)
                    throw;
            }
        }

        internal static void TrinityOnItemSalvaged(object sender, ItemEventArgs e)
        {
            ResetTownRun();

            try
            {
                ACDItem i = e.Item;

                if (i == null || !i.IsValid || i.IsDisposed)
                    return;

                var cachedItem = CachedACDItem.GetCachedItem(i);                
                switch (i.ItemBaseType)
                {
                    case ItemBaseType.Gem:
                    case ItemBaseType.Misc:
                        break;
                    default:
                        TownRun.LogJunkItems(cachedItem, cachedItem.TrinityItemBaseType, cachedItem.TrinityItemType, ItemValuation.ValueThisItem(cachedItem, cachedItem.TrinityItemType));
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is CoroutineStoppedException)
                    throw;
            }

        }

        internal static void TrinityOnItemSold(object sender, ItemEventArgs e)
        {
            ResetTownRun();

            try
            {
                ACDItem i = e.Item;

                if (i == null || !i.IsValid || i.IsDisposed)
                    return;

                var cachedItem = CachedACDItem.GetCachedItem(i);
                switch (i.ItemBaseType)
                {
                    case ItemBaseType.Gem:
                    case ItemBaseType.Misc:
                        break;
                    default:
                        TownRun.LogJunkItems(cachedItem, cachedItem.TrinityItemBaseType, cachedItem.TrinityItemType, ItemValuation.ValueThisItem(cachedItem, cachedItem.TrinityItemType));
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is CoroutineStoppedException)
                    throw;
            }
        }

        internal static void TrinityOnOnItemIdentificationRequest(object sender, ItemIdentifyRequestEventArgs e)
        {
            if (Trinity.Settings.Loot.TownRun.DropInTownOption == Settings.Loot.DropInTownOption.All)
            {
                ItemDropper.Drop(e.Item);
            }                

            e.IgnoreIdentification = !TrinityItemManager.ItemRulesIdentifyValidation(e.Item);
        }

        internal static void ResetTownRun()
        {
            ItemValuation.ResetValuationStatStrings();
            TownRun.TownRunCheckTimer.Reset();
            Trinity.ForceVendorRunASAP = false;
            Trinity.WantToTownRun = false;
        }

        internal static void TrinityOnItemDropped(object sender, ItemEventArgs e)
        {
            //Logger.Log("Dropped {0} ({1})", e.Item.Name, e.Item.ActorSNO);          
        }
    }


}
