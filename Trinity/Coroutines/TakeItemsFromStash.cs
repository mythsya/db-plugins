using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using TrinityCoroutines.Resources;
using Zeta;
using Zeta.Bot;
using Zeta.Common;
using Zeta.TreeSharp;
using Zeta.Common;
using Zeta.Bot;
using Zeta.Bot.Navigation;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Logger = Trinity.Technicals.Logger;

namespace TrinityCoroutines
{
    public class TakeItemsFromStash
    {
        /// <summary>
        /// Moves items from the Stash to the Backpack
        /// </summary>
        /// <param name="itemIds">list of items to withdraw</param>
        /// <param name="maxAmount">amount to withdraw up to (including counts already in backpack)</param>
        /// <returns></returns>
        public static async Task<bool> Execute(IEnumerable<int> itemIds, int maxAmount)
        {
            Logger.Log("TakeItemsFromStash Started!");

            if (!ZetaDia.IsInGame || !ZetaDia.IsInTown)
                return true;

            if (Town.Locations.Stash.Distance(ZetaDia.Me.Position) > 3f)
            {
                await MoveToAndInteract.Execute(Town.Locations.Stash, Town.ActorIds.Stash, 8f);
            }

            var stash = Town.Actors.Stash;
            if (stash == null)
            {
                Logger.Log("Unable to find Stash");
                return false;
            }

            if (!UIElements.StashWindow.IsVisible && Town.Locations.Stash.Distance(ZetaDia.Me.Position) <= 10f)
            {                
                Logger.Log("Stash window not open, interacting");
                stash.Interact();
            }
                
            var itemIdsHashSet = new HashSet<int>(itemIds);
            var amountWithdrawn = itemIdsHashSet.ToDictionary(k => k, v => (long)0);
            var overageTaken = itemIdsHashSet.ToDictionary(k => k, v => false);
            var lastStackTaken = itemIdsHashSet.ToDictionary(k => k, v => default(ACDItem));

            foreach (var item in ZetaDia.Me.Inventory.Backpack.Where(i => i.ACDGuid != 0 && i.IsValid && itemIdsHashSet.Contains(i.ActorSNO)).ToList())
            {
                amountWithdrawn[item.ActorSNO] += item.ItemStackQuantity;
                lastStackTaken[item.ActorSNO] = item;
            }
            
            foreach (var item in ZetaDia.Me.Inventory.StashItems.Where(i => i.ACDGuid != 0 && i.IsValid && itemIdsHashSet.Contains(i.ActorSNO)).ToList())
            {
                try
                {
                    if (!item.IsValid || item.IsDisposed)
                        continue;

                    var stackSize = item.ItemStackQuantity;
                    var numTakenAlready = amountWithdrawn[item.ActorSNO];

                    // We have enough of this material already
                    var alreadyTakenEnough = numTakenAlready >= maxAmount;
                    if (alreadyTakenEnough)
                        continue;

                    // We have enough of everything already.
                    if (amountWithdrawn.All(i => i.Value >= maxAmount))
                        break;

                    // Only take up to the required amount.
                    var willBeOverMax = numTakenAlready + stackSize > maxAmount;                        
                    if (!willBeOverMax || !overageTaken[item.ActorSNO])
                    {
                        var lastItem = lastStackTaken[item.ActorSNO];
                        var amountRequiredToMax = maxAmount - numTakenAlready;

                        if (willBeOverMax && lastItem != null && lastItem.IsValid && !lastItem.IsDisposed && stackSize > amountRequiredToMax)
                        {
                            // Tried InventoryManager.SplitStack but it didnt work, reverting to moving onto existing stacks.

                            var amountToSplit = stackSize - lastItem.ItemStackQuantity;                        
                            Logger.Log("Merging Stash Stack {0} onto Backpack Stack. StackSize={1} WithdrawnAlready={2}", item.Name, amountToSplit, numTakenAlready);
                            ZetaDia.Me.Inventory.MoveItem(item.DynamicId, ZetaDia.Me.CommonData.DynamicId, InventorySlot.BackpackItems, lastItem.InventoryColumn, lastItem.InventoryRow);

                            amountWithdrawn[item.ActorSNO] += amountToSplit;
                            overageTaken[item.ActorSNO] = true;
                        }
                        else
                        {
                            Logger.Log("Removing {0} ({3}) from stash. StackSize={1} WithdrawnAlready={2}", item.Name, stackSize, numTakenAlready, item.ActorSNO);
                            if (item.IsValid && !item.IsDisposed)
                            {
                                ZetaDia.Me.Inventory.QuickWithdraw(item);
                                amountWithdrawn[item.ActorSNO] += stackSize;
                                lastStackTaken[item.ActorSNO] = item;
                            }
                        }
                                                    
                        await Coroutine.Sleep(25);
                        await Coroutine.Yield();                        
                    }


                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }
            }

            await Coroutine.Sleep(1000);
            Logger.Log("TakeItemsFromStash Finished!");
            return true;
        }
    
    }
}
