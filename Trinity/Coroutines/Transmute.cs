using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Coroutines;
using TrinityCoroutines.Resources;
using Trinity.Helpers;
using Trinity.Technicals;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;

namespace TrinityCoroutines
{
    public static class Transmute
    {
        public static async Task<bool> Execute(List<ACDItem> transmuteGroup)
        {
            if (!ZetaDia.IsInGame)
                return false;

            if (transmuteGroup.Count > 9)
            {
                Logger.Log(" --> Can't convert more than 9 items!");
                return false;
            }

            Logger.Log("Transmuting:");

            foreach (var item in transmuteGroup)
            {
                if (item == null || !item.IsValid || item.IsDisposed)
                {
                    Logger.Log(" --> Invalid Item Found {0}");
                    return false;
                }

                Logger.Log(" --> {0} StackQuantity={1} Quality={2} CraftingMaterial={3}", 
                    item.Name, item.ItemStackQuantity, item.GetItemQuality(), item.IsCraftingReagent);
            }

            await Coroutine.Yield();

            if (!UIElements.TransmuteItemsDialog.IsVisible)
            {
                await Coroutine.Sleep(500);

                await MoveToAndInteract.Execute(Town.Locations.KanaisCube, Town.ActorIds.KanaisCube, 8f);

                await Coroutine.Sleep(1000);

                if (!UIElements.TransmuteItemsDialog.IsVisible)
                {
                    Logger.Log("Cube window needs to be open before you can transmute anything.");
                    return false;
                }
            }

            Logger.Log("Zip Zap!");
            ZetaDia.Me.Inventory.TransmuteItems(transmuteGroup);
            return true;
        }
    }
}

