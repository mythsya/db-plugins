using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Trinity;
using TrinityCoroutines.Resources;
using Trinity.DbProvider;
using Trinity.Helpers;
using Trinity.Items;
using Zeta;
using Zeta.Bot;
using Zeta.Common;
using Zeta.TreeSharp;
using Zeta.Common;
using Zeta.Bot;
using Zeta.Bot.Navigation;
using Zeta.Game;
using Trinity.Technicals;
using Zeta.Bot.Coroutines;
using Zeta.Bot.Logic;
using Zeta.Game.Internals.Actors;
using Logger = Trinity.Technicals.Logger;

namespace TrinityCoroutines
{
    public class BuyItemsFromVendor
    {
        public static bool CanRun(ItemQualityColor qualityColor, List<ItemType> types = null, int totalAmount = -1, int vendorId = -1)
        {      
            return true;
        }

        public static async Task<bool> Execute(ItemQualityColor qualityColor, List<ItemType> types = null, int totalAmount = -1, int vendorId = -1)
        {
            Logger.Log("BuyItemsFromVendor Started!");

            //if (ZetaDia.Me.Inventory.NumFreeBackpackSlots < totalAmount * 2)
            //{
            //    Logger.Log("Not enough bag space to buy {0} items", totalAmount);
            //    await BrainBehavior.CreateVendorBehavior().ExecuteCoroutine();
            //}

            foreach (var item in ZetaDia.Me.Inventory.MerchantItems)
            {
                item.PrintEFlags();
            }

            var items = ZetaDia.Me.Inventory.MerchantItems.ToList();

            var vendorLocation = Town.Locations.GetLocationFromActorId(vendorId);
            if (!await MoveToAndInteract.Execute(vendorLocation, vendorId, 5f))
                return false;

            Logger.Log("BuyItemsFromVendor Finished!");
            return true;
        }

    }
}
