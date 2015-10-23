using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Coroutines;
using TrinityCoroutines;
using TrinityCoroutines.Resources;
using Trinity.Helpers;
using Trinity.Technicals;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Game;
using Zeta.Game.Internals;

namespace Trinity.DbProvider
{
    public static class VendorHook
    {
        /// <summary>
        /// Injected to TownRun Composite at Step3 (just after identifying legendaries).
        /// </summary>
        public async static Task<bool> ExecutePreVendor()
        {

            // Task returning True = We're done, move on.
            // Task returning False = Re-execute from start.

            if (!ZetaDia.IsInTown)
                return false;

            Logger.LogVerbose("PreVendor Hook Started");

            try
            {
                var quest = new RiftQuest();
                if (ZetaDia.IsInTown && quest.State == QuestState.NotStarted && quest.Step == RiftStep.Completed && ZetaDia.Me.IsParticipatingInTieredLootRun)
                {
                    Logger.Log("Waiting...");
                    await Coroutine.Sleep(500);
                }

                if (ZetaDia.Me.IsParticipatingInTieredLootRun)
                    return false;

                // Learn some recipies.
                if (!await UseCraftingRecipes.Execute())
                    return true;

                // Destroy white/blue/yellow items to convert crafting materials.
                if (!await CubeItemsToMaterials.Execute())
                    return true;

                // Gamble first for cube legendary rares, bag space permitting.
                if (!await Gamble.Execute())
                    return true;

                // Run this before vendoring to use the rares we picked up.
                if (!await CubeRaresToLegendary.Execute())
                    return true;

            }
            catch (Exception ex)
            {
                Logger.LogError("Exception in VendorHook {0}", ex);

                if (ex is CoroutineStoppedException)
                    throw;
            }

            return false;
        }

        /// <summary>
        /// Injected to TownRun Composite after Stash/Sell/Salvage
        /// </summary>
        public async static Task<bool> ExecutePostVendor()
        {
            // Task returning True = We're done, move on.
            // Task returning False = Re-execute from start.

            if (!ZetaDia.IsInTown)
                return false;

            Logger.LogVerbose("PostVendor Hook Started");

            try
            {                
                if (ZetaDia.Me.IsParticipatingInTieredLootRun)
                    return false;

                // Run again in case we missed first time due to full bags.
                if (!await Gamble.Execute())
                    return true;

                // Destroy white/blue/yellow items to convert crafting materials.
                if (!await CubeItemsToMaterials.Execute())
                    return true;

                // Run again in case we just gambled
                if (!await CubeRaresToLegendary.Execute())
                    return true;


            }
            catch (Exception ex)
            {
                Logger.LogError("Exception in VendorHook {0}", ex);

                if (ex is CoroutineStoppedException)
                    throw;
            }

            return false;
        }
    }
}
