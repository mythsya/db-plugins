using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using TrinityCoroutines.Resources;
using Trinity.Helpers;
using Trinity.Technicals;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace TrinityCoroutines
{
    /// <summary>
    /// Converts crafting materials into other types of crafting materials
    /// </summary>
    public class ConvertMaterials
    {
        public static List<ACDItem> GetBackpackItemsOfQuality(List<ItemQuality> qualities)
        {
            // DB is reporting items as being still there after transmution, add a bunch of checks :(
            return ZetaDia.Me.Inventory.Backpack.Where(i =>
            {
                if (!i.IsValid || i.IsDisposed)
                {
                    //Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Invalid item '{0}' IsValid/Disposed", i.InternalName);
                    return false;
                }

                if (i.ItemBaseType != ItemBaseType.Armor && i.ItemBaseType != ItemBaseType.Weapon && i.ItemBaseType != ItemBaseType.Jewelry)
                {
                    //Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Invalid item '{0}' BaseType={1}", i.InternalName, i.ItemBaseType);
                    return false;
                }

                var stackQuantity = i.ItemStackQuantity;
                var isVendor = i.IsVendorBought;
                if (!isVendor && stackQuantity != 0 || isVendor && stackQuantity > 1)
                {
                    //Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Invalid item '{0}' stackQuantity={1}", i.InternalName, stackQuantity);
                    return false;
                }

                if (!qualities.Contains(i.GetItemQuality()))
                {
                    //Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Invalid item '{0}' Quality LinkColor={1} DBQuality={2}", i.InternalName, i.GetItemQuality(), i.ItemQualityLevel);
                    return false;
                }

                return true;

            }).ToList();
        }

        public static bool CanRun(InventoryItemType from, InventoryItemType to, bool excludeLegendaryUpgradeRares = false, bool checkStash = false)
        {
            if (!ZetaDia.IsInGame || !ZetaDia.IsInTown)
                return false;

            if (!Inventory.MaterialConversionTypes.Contains(to) || !Inventory.MaterialConversionTypes.Contains(from))
            {
                Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Unable to convert from {0} to {1}", from, to);
                return false;
            }

            if (!GetSacraficialItems(to, excludeLegendaryUpgradeRares).Any())
            {
                Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] You dont enough valid weapon/armor/jewellery in backpack", from, to);
                return false;
            }

            Inventory.Materials.Update();
            Inventory.Materials.LogCounts();
    
            if (checkStash)
            {
                if (Inventory.Materials[InventoryItemType.DeathsBreath].TotalStackQuantity >= 1 && Inventory.Materials[from].TotalStackQuantity > 100)
                {
                    Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Enough materials to convert from {0} ({1}) to {2}",
                        from, Inventory.Materials[from].TotalStackQuantity, to);

                    return true;
                }                                                    
            }

            if (Inventory.Materials[from].BackpackStackQuantity > 100)
            {
                Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] We have enough Backpack materials to convert from {0} ({1}) to {2}",
                    from, Inventory.Materials[from].BackpackStackQuantity, to);

                return true;
            }

            Logger.LogVerbose(LogCategory.Behavior, "[ConvertMaterials] Not Enough Backpack materials to convert from {0} ({1}) to {2}, Deaths={3}",
                from, Inventory.Materials[from].BackpackStackQuantity, to, Inventory.Materials[InventoryItemType.DeathsBreath].BackpackStackQuantity);

            return false;
        }

        /// <summary>
        /// Converts crafting materials into other types of crafting materials
        /// </summary>
        /// <param name="from">the type of material you will consume</param>        
		/// <param name="to">the type of material you will get more of</param>        
        public static async Task<bool> Execute(InventoryItemType from, InventoryItemType to)
        {
            Logger.Log("[ConvertMaterials] Wooo! Lets convert some {0} to {1}", from, to);

            if (!ZetaDia.IsInGame || !ZetaDia.IsInTown)
                return true;

            if (!Inventory.MaterialConversionTypes.Contains(to) || !Inventory.MaterialConversionTypes.Contains(from))
            {
                Logger.Log("[Cube] Unable to convert from {0} to {1}", from, to);
                return true;
            }

            var backpackDeathsBreathAmount = Inventory.Backpack.DeathsBreath.Select(i => i.ItemStackQuantity).Sum();
			var backpackFromMaterialAmount = Inventory.Backpack.OfType(from).Select(i => i.ItemStackQuantity).Sum();
			var backpackToMaterialAmount = Inventory.Backpack.OfType(to).Select(i => i.ItemStackQuantity).Sum();

            Inventory.Materials.Update();
            var sacraficialItems = GetSacraficialItems(to);

            Logger.LogVerbose("[ConvertMaterials] Starting Material Counts DeathsBreath={0} {1}={2} {3}={4} SacraficialItems={5}", 
                backpackDeathsBreathAmount, from, backpackFromMaterialAmount, to, backpackToMaterialAmount, sacraficialItems.Count);

            while (CanRun(from, to))
            {
                Inventory.Materials.Update();
                sacraficialItems = GetSacraficialItems(to);               

                var item = sacraficialItems.First();
                var transmuteGroup = new List<ACDItem>
				{
                    Inventory.Backpack.DeathsBreath.First(),
                    item
                };
                sacraficialItems.Remove(item);

                // Make sure we include enough materials by adding multiple stacks if nessesary.
                var materialStacks = Inventory.GetStacksUpToQuantity(Inventory.Backpack.OfType(from), 50).ToList();
                if (materialStacks.Any(m => !m.IsValid || m.IsDisposed) || !item.IsValid || item.IsDisposed)
                {
                    Logger.LogError("[ConvertMaterials] something is terribly wrong our items are not valid");
                    return true;
                }

                transmuteGroup.AddRange(materialStacks);

                await Transmute.Execute(transmuteGroup);
                await Coroutine.Sleep(1500);
                await Coroutine.Yield();

                var newToAmount = Inventory.Backpack.OfType(to).Select(i => i.ItemStackQuantity).Sum();
				if(newToAmount > backpackToMaterialAmount)
				{
					Logger.Log("[ConvertMaterials] Converted materials '{0}' ---> '{1}'", from, to);
					backpackToMaterialAmount = newToAmount;
					backpackFromMaterialAmount = Inventory.Backpack.OfType(from).Select(i => i.ItemStackQuantity).Sum();
					backpackDeathsBreathAmount = Inventory.Backpack.DeathsBreath.Select(i => i.ItemStackQuantity).Sum();
				    ConsecutiveFailures = 0;
				}
				else
				{
				    ConsecutiveFailures++;
				    if (ConsecutiveFailures > 3)
				    {
                        Inventory.InvalidItemDynamicIds.Add(item.ACDGuid);
				    }

					Logger.LogError("[ConvertMaterials] Failed to convert materials");
					return true;
				}
                
                await Coroutine.Sleep(100);
                await Coroutine.Yield();
            }

            Logger.LogVerbose("[ConvertMaterials] Finishing Material Counts DeathsBreath={0} {1}={2} {3}={4} SacraficialItems={5}",
                backpackDeathsBreathAmount, from, backpackFromMaterialAmount, to, backpackToMaterialAmount, sacraficialItems.Count);

            return true;
        }

        public static int ConsecutiveFailures { get; set; }

        public static List<ACDItem> GetSacraficialItems(InventoryItemType to, bool excludeLegendaryUpgradeRares = false)
        {
            List<ACDItem> sacraficialItems = new List<ACDItem>();

            switch (to)
            {
                case InventoryItemType.ReusableParts:
                    sacraficialItems = GetBackpackItemsOfQuality(new List<ItemQuality>
                    {
                        ItemQuality.Inferior,
                        ItemQuality.Normal,
                        ItemQuality.Superior
                    });
                    break;

                case InventoryItemType.ArcaneDust:
                    sacraficialItems = GetBackpackItemsOfQuality(new List<ItemQuality>
                    {
                        ItemQuality.Magic1,
                        ItemQuality.Magic2,
                        ItemQuality.Magic3
                    });
                    break;

                case InventoryItemType.VeiledCrystal:
                    sacraficialItems = GetBackpackItemsOfQuality(new List<ItemQuality>
                    {
                        ItemQuality.Rare4,
                        ItemQuality.Rare5,
                        ItemQuality.Rare6
                    });
                    break;
            }

            if (excludeLegendaryUpgradeRares)
            {
                var upgradeRares = CubeRaresToLegendary.GetBackPackRares();
                sacraficialItems.RemoveAll(i => upgradeRares.Contains(i));
            }

            sacraficialItems.RemoveAll(i => Inventory.InvalidItemDynamicIds.Contains(i.ACDGuid));
            return sacraficialItems;
        }

        public static HashSet<int> BlacklistedACDGuids = new HashSet<int>();
    }
}
