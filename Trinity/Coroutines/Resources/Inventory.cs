using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Trinity.Helpers;
using Trinity.Items;
using Trinity.Technicals;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;

namespace TrinityCoroutines.Resources
{
    /// <summary>
    /// Enum InventoryItemType - this is not finalized, i don't have all the items
    /// </summary>
    public enum InventoryItemType
    {
        None = 0,
        CommonDebris = 1,
        ReusableParts = 361984,
        ArcaneDust = 361985,
        ExquisiteEssence = 3,
        ShimmeringEssence = 4,
        SubtleEssence = 5,
        WishfulEssence = 6,
        DeathsBreath = 361989,
        DemonicEssence = 8,
        EncrustedHoof = 9,
        FallenTooth = 10,
        IridescentTear = 11,
        LizardEye = 12,
        VeiledCrystal = 361986,
        FieryBrimstone = 189863,
        ForgottenSoul = 361988,
        KeyOfBones = 364694,
        KeyOfEvil = 364697,
        KeyOfGluttony = 364695,
        KeyOfWar = 364696,
        KeyOfDestruction = 255880,
        KeyOfHate = 255881,
        KeyOfTerror = 255882,
		CaldeumNightshade = 364281,
		WestmarchHolyWater = 364975,
		ArreatWarTapestry = 364290,
		CorruptedAngelFlesh = 364305,
		KhanduranRune = 365020,
        BlackSmithPlan = 192598,
        JewelerPlan = 192600,
    }

    public enum ItemLocation
    {
        Unknown = 0,
        Backpack,
        Stash,
        Ground,
        Equipped,
    }

    public static class Inventory
    {
        static Inventory()
        {
            Materials = new MaterialStore(CraftingMaterialIds);
            Pulsator.OnPulse += Pulsator_OnPulse;
        }

        private static void Pulsator_OnPulse(object sender, EventArgs e)
        {
            if (ZetaDia.IsInGame && ZetaDia.IsInTown && Materials.TimeSinceUpdate.TotalMilliseconds > 1000)
                Materials.Update();
        }

        public static MaterialStore Materials { get; set; }

        public class MaterialStore
        {
            public MaterialStore(IEnumerable<int> actorIds)
            {
                Types = actorIds.Select(i => (InventoryItemType)i).ToList();
            }

            public MaterialStore(IList<InventoryItemType> types)
            {
                Types = types;
            }

            public IList<InventoryItemType> Types { get; set; }

            public Dictionary<InventoryItemType, MaterialRecord> Source = new Dictionary<InventoryItemType, MaterialRecord>();

            public void Update(bool updateAllProperties = false)
            {
                Source = GetMaterials(Types);
                LastUpdated = DateTime.UtcNow;
            }

            public DateTime LastUpdated = DateTime.MinValue;

            public TimeSpan TimeSinceUpdate
            {
                get { return DateTime.UtcNow.Subtract(LastUpdated);  }
            }

            public MaterialRecord this[InventoryItemType i]
            {
                get { return Source[i]; }
                set { Source[i] = value; }
            }

            public void LogCounts(string msg = "", TrinityLogLevel level = TrinityLogLevel.Info)
            {
                Logger.Log(level, msg + " " + GetCountsString(Source));
            }

            public MaterialRecord HighestCountMaterial(IEnumerable<InventoryItemType> types)
            {
                return Source.OrderByDescending(pair => pair.Value.TotalStackQuantity).FirstOrDefault().Value;
            }

            public IEnumerable<KeyValuePair<InventoryItemType, MaterialRecord>> OfTypes(IEnumerable<InventoryItemType> types)
            {
                return Source.Where(m => types.Contains(m.Key));
            }

            public bool HasStackQuantityOfType(InventoryItemType type, InventorySlot location, int quantity)
            {
                return Source[type].StackQuantityByInventorySlot(location) > quantity;
            }

            public bool HasStackQuantityOfTypes(IEnumerable<InventoryItemType> types, InventorySlot location, int quantity)
            {
                return Source.Where(pair => types.Contains(pair.Key)).All(pair => HasStackQuantityOfType(pair.Key, location, quantity));
            }
        }


        public static IEnumerable<ACDItem> GetStacksUpToQuantity(List<ACDItem> materialsStacks, int maxStackQuantity)
        {
            if (materialsStacks == null || !materialsStacks.Any() || materialsStacks.Count == 1)
                return materialsStacks;

            long dbQuantity = 0;
            var overlimit = 0;            

            // First of Non-Stackable Items
            var first = materialsStacks.First();
            if (first.ItemStackQuantity == 0 && maxStackQuantity == 1 && materialsStacks.All(i => !i.IsCraftingReagent))
                return new List<ACDItem> { first };

            // Position in the cube matters; it looks like it will fail if
            // stacks are added after the required amount of ingredient is met, 
            // as the cube encounters them from top left to bottom right.

            var toBeAdded = materialsStacks.TakeWhile(db =>
            {            
                var thisStackQuantity = db.ItemStackQuantity;

                if (dbQuantity + thisStackQuantity < maxStackQuantity)
                {
                    dbQuantity += thisStackQuantity;
                    return true;
                }
                overlimit++;
                return overlimit == 1;
            });

            return toBeAdded.ToList();
        }

        public class MaterialRecord
        {
            public int ActorId { get; set; }
            public InventoryItemType Type { get; set; }

            public List<ACDItem> StashItems = new List<ACDItem>();
            public List<ACDItem> BackpackItems = new List<ACDItem>();

            public long Total
            {
                get { return StashItemCount + BackpackItemCount; }
            }

            public long StashItemCount
            {
                get { return StashItems.Count; }
            }

            public long BackpackItemCount
            {
                get { return BackpackItems.Count; }
            }

            private long? _backpackStackQuantity;
            public long BackpackStackQuantity
            {
                get { return _backpackStackQuantity ?? (_backpackStackQuantity = BackpackItems.Where(i => i.IsValid && !i.IsDisposed).Select(i => i.ItemStackQuantity).Sum()).Value; }
            }

            private long? _stashStackQuantity;
            public long StashStackQuantity
            {
                get { return _stashStackQuantity ?? (_stashStackQuantity = StashItems.Where(i => i.IsValid && !i.IsDisposed).Select(i => i.ItemStackQuantity).Sum()).Value; }
            }

            private long? _totalStackQuantity;
            public long TotalStackQuantity
            {
                get { return _totalStackQuantity ?? (_totalStackQuantity = StashStackQuantity + BackpackStackQuantity).Value; }
            }

            public long StackQuantityByInventorySlot(InventorySlot slot)
            {
                switch (slot)
                {
                    case InventorySlot.BackpackItems: return BackpackStackQuantity;
                    case InventorySlot.SharedStash: return StashStackQuantity;
                    default: return TotalStackQuantity;
                }
            }


        }

        public static string GetCountsString(Dictionary<InventoryItemType, MaterialRecord> materials)
        {
            var backpack = String.Empty;
            var stash = String.Empty;
            var total = String.Empty;

            foreach (var item in materials)
            {
                backpack += String.Format("{0}={1} ", item.Key, item.Value.BackpackStackQuantity);
                stash += String.Format("{0}={1} ", item.Key, item.Value.StashStackQuantity);
                total += String.Format("{0}={1} ", item.Key, item.Value.TotalStackQuantity);
            }
            return String.Format("Backpack: [{0}] \r\nStash: [{1}]\r\n Total: [{2}]\r\n", backpack.Trim(), stash.Trim(), total.Trim());
        }

        public static Dictionary<InventoryItemType, MaterialRecord> GetMaterials(IList<InventoryItemType> types)
        {
            var materials = types.ToDictionary(t => t, v => new MaterialRecord());
            var materialSNOs = new HashSet<int>(types.Select(m => (int)m));

            foreach (var item in ZetaDia.Me.Inventory.Backpack)
            {
                if (InvalidItemDynamicIds.Contains(item.DynamicId))
                    continue;

                if (!item.IsValid || item.IsDisposed || (item.IsCraftingReagent && item.ItemStackQuantity == 0))
                {
                    Logger.LogVerbose(LogCategory.Behavior, "Invalid item skipped: {0}", item.InternalName);
                    InvalidItemDynamicIds.Add(item.DynamicId);
                    continue;
                }
                    
                var itemSNO = item.ActorSNO;
                if (materialSNOs.Contains(itemSNO))
                {
                    var type = (InventoryItemType)itemSNO;
                    var materialRecord = materials[type];
                    materialRecord.BackpackItems.Add(item);
                    materialRecord.Type = type;
                    materialRecord.ActorId = itemSNO;
                }
            }

            foreach (var item in ZetaDia.Me.Inventory.StashItems)
            {
                if (InvalidItemDynamicIds.Contains(item.DynamicId))
                    continue;

                if (!item.IsValid || item.IsDisposed || (item.IsCraftingReagent && item.ItemStackQuantity == 0))
                {
                    Logger.LogVerbose(LogCategory.Behavior, "Invalid item skipped: {0}", item.InternalName);
                    InvalidItemDynamicIds.Add(item.DynamicId);
                    continue;
                }

                var itemSNO = item.ActorSNO;
                if (materialSNOs.Contains(itemSNO))
                {
                    var type = (InventoryItemType) itemSNO;
                    var materialRecord = materials[type];
                    materialRecord.StashItems.Add(item);
                    materialRecord.Type = type;
                    materialRecord.ActorId = itemSNO;
                }
            }

            return materials;
        }

        private static HashSet<int> _blacklistedDynamicIds;
        public static HashSet<int> InvalidItemDynamicIds
        {
            get
            {
                if(_blacklistedDynamicIds == null)
                    _blacklistedDynamicIds = new HashSet<int>();

                if (_blacklistedDynamicIds.Count > 100)
                    _blacklistedDynamicIds.Remove(_blacklistedDynamicIds.First());

                return _blacklistedDynamicIds;
            }
            set { _blacklistedDynamicIds = value; }
        }

        public static HashSet<InventoryItemType> MaterialConversionTypes = new HashSet<InventoryItemType>
        {
            InventoryItemType.ArcaneDust,
            InventoryItemType.ReusableParts,
            InventoryItemType.VeiledCrystal,
        };

        public static HashSet<InventoryItemType> CraftingMaterialTypes = new HashSet<InventoryItemType>
        {
            InventoryItemType.ArcaneDust,
            InventoryItemType.ReusableParts,
            InventoryItemType.VeiledCrystal,
            InventoryItemType.CaldeumNightshade,
            InventoryItemType.DeathsBreath,
            InventoryItemType.WestmarchHolyWater,
            InventoryItemType.ArreatWarTapestry,
            InventoryItemType.CorruptedAngelFlesh,
            InventoryItemType.KhanduranRune,
            InventoryItemType.ForgottenSoul,
        };

        public static HashSet<int> CraftingMaterialIds = new HashSet<int>
        {
            361985, //Type: Item, Name: Arcane Dust
            361984, //Type: Item, Name: Reusable Parts
            361986, //Type: Item, Name: Veiled Crystal
            364281, //Type: Item, Name: Caldeum Nightshade
            361989, //Type: Item, Name: Death's Breath
            364975, //Type: Item, Name: Westmarch Holy Water
            364290, //Type: Item, Name: Arreat War Tapestry
            364305, //Type: Item, Name: Corrupted Angel Flesh
            365020, //Type: Item, Name: Khanduran Rune
            361988, //Type: Item, Name: Forgotten Soul
        };

        public static HashSet<int> MaterialConversionIds = new HashSet<int>
        {
            361985, //Type: Item, Name: Arcane Dust
            361984, //Type: Item, Name: Reusable Parts
            361986, //Type: Item, Name: Veiled Crystal
            361989, //Type: Item, Name: Death's Breath
        };

        public static HashSet<int> RareUpgradeIds = new HashSet<int>
        {
            361985, //Type: Item, Name: Arcane Dust
            361984, //Type: Item, Name: Reusable Parts
            361986, //Type: Item, Name: Veiled Crystal
            361989, //Type: Item, Name: Death's Breath
        };

        public static HashSet<int> SetRollingIds = new HashSet<int>
        {
            361989, //Type: Item, Name: Death's Breath
            361988, //Type: Item, Name: Forgotten Soul
        };

        public static HashSet<int> PowerExtractionIds = new HashSet<int>
        {
            364281, //Type: Item, Name: Caldeum Nightshade
            361989, //Type: Item, Name: Death's Breath
            364975, //Type: Item, Name: Westmarch Holy Water
            364290, //Type: Item, Name: Arreat War Tapestry
            364305, //Type: Item, Name: Corrupted Angel Flesh
            365020, //Type: Item, Name: Khanduran Rune
            361988, //Type: Item, Name: Forgotten Soul
        };



        public static List<ACDItem> OfType(IEnumerable<InventoryItemType> types)
        {
            var typesHash = new HashSet<int>(types.Select(t => (int)t));
            return AllItems.Where(i => typesHash.Contains(i.ActorSNO)).ToList();
        }

        public static List<ACDItem> OfType(params InventoryItemType[] types)
        {
            var typesHash = new HashSet<int>(types.Select(t => (int)t));
            return AllItems.Where(i => typesHash.Contains(i.ActorSNO)).ToList();
        }

        public static List<ACDItem> OfType(InventoryItemType type)
        {
            return AllItems.Where(i => i.ActorSNO == (int)type).ToList();
        }

        public static List<ACDItem> ByItemType(ItemType type)
        {
            return AllItems.Where(i => i.ItemType == type).ToList();
        }

        public static List<ACDItem> ByActorSNO(int ActorSNO)
        {
            return AllItems.Where(i => i.ActorSNO == ActorSNO).ToList();
        }

        public static List<ACDItem> AllItems
        {
            get { return ZetaDia.Actors.GetActorsOfType<ACDItem>(true).Where(i => i.IsValid && !i.IsDisposed && 
            (i.InventorySlot == InventorySlot.BackpackItems || i.InventorySlot == InventorySlot.SharedStash) &&
            (!i.IsCraftingReagent && i.ItemStackQuantity == 0 || i.IsCraftingReagent && i.ItemStackQuantity > 0) &&
            !InvalidItemDynamicIds.Contains(i.DynamicId)).ToList(); }           
        }

        public static class Backpack
        {
            public static List<ACDItem> AllBackpackItems
            {
                get { return AllItems.Where(i => i.InventorySlot == InventorySlot.BackpackItems).ToList(); }
            }

            public static List<ACDItem> OfType(IEnumerable<InventoryItemType> types)
            {
                var typesHash = new HashSet<int>(types.Select(t => (int)t));
                return AllBackpackItems.Where(i => i.IsValid && !i.IsDisposed && typesHash.Contains(i.ActorSNO)).ToList();
            }

            public static List<ACDItem> OfType(params InventoryItemType[] types)
            {
                var typesHash = new HashSet<int>(types.Select(t => (int)t));
                return AllBackpackItems.Where(i => typesHash.Contains(i.ActorSNO)).ToList();
            }

            public static List<ACDItem> OfType(InventoryItemType type)
            {
                return AllBackpackItems.Where(i => i.ActorSNO == (int)type).ToList();
            }

            public static List<ACDItem> ByItemType(ItemType type)
            {
                return AllBackpackItems.Where(i => i.ItemType == type).ToList();
            }

            public static List<ACDItem> ByActorSNO(int ActorSNO)
            {
                return AllBackpackItems.Where(i => i.ActorSNO == ActorSNO).ToList();
            }

            public static List<ACDItem> ArcaneDust
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.ArcaneDust).ToList(); }
            }

            public static List<ACDItem> ReusableParts
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.ReusableParts).ToList(); }
            }

            public static List<ACDItem> VeiledCrystals
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.VeiledCrystal).ToList(); }
            }

            public static List<ACDItem> DeathsBreath
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.DeathsBreath).ToList(); }
            }
			
            public static List<ACDItem> ForgottenSoul
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.ForgottenSoul).ToList(); }
            }			
			
            public static List<ACDItem> CaldeumNightshade
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.CaldeumNightshade).ToList(); }
            }			

			public static List<ACDItem> WestmarchHolyWater
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.WestmarchHolyWater).ToList(); }
            }		
			
			public static List<ACDItem> ArreatWarTapestry
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.ArreatWarTapestry).ToList(); }
            }			

			public static List<ACDItem> CorruptedAngelFlesh
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.CorruptedAngelFlesh).ToList(); }
            }	
			
			public static List<ACDItem> KhanduranRune
            {
                get { return AllBackpackItems.Where(i => i.ActorSNO == (int)InventoryItemType.KhanduranRune).ToList(); }
            }
        }

        public static class Stash
        {
            public static List<ACDItem> AllStashItems
            {
                get { return AllItems.Where(i => i.InventorySlot == InventorySlot.SharedStash).ToList(); }
            }

            public static List<ACDItem> OfType(IEnumerable<InventoryItemType> types)
            {
                var typesHash = new HashSet<int>(types.Select(t => (int)t));
                return AllStashItems.Where(i => typesHash.Contains(i.ActorSNO)).ToList();
            }

            public static List<ACDItem> OfType(params InventoryItemType[] types)
            {
                var typesHash = new HashSet<int>(types.Select(t => (int)t));
                return AllStashItems.Where(i => typesHash.Contains(i.ActorSNO)).ToList();
            }

            public static List<ACDItem> OfType(InventoryItemType type)
            {
                return AllStashItems.Where(i => i.ActorSNO == (int)type).ToList();
            }

            public static List<ACDItem> ByItemType(ItemType type)
            {
                return AllStashItems.Where(i => i.ItemType == type).ToList();
            }

            public static List<ACDItem> ByActorSNO(int ActorSNO)
            {
                return AllStashItems.Where(i => i.ActorSNO == ActorSNO).ToList();
            }

            public static List<ACDItem> ArcaneDust
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.ArcaneDust).ToList(); }
            }

            public static List<ACDItem> ReusableParts
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.ReusableParts).ToList(); }
            }

            public static List<ACDItem> VeiledCrystals
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.VeiledCrystal).ToList(); }
            }

            public static List<ACDItem> DeathsBreath
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.DeathsBreath).ToList(); }
            }
			
            public static List<ACDItem> ForgottenSoul
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.ForgottenSoul).ToList(); }
            }			
			
            public static List<ACDItem> CaldeumNightshade
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.CaldeumNightshade).ToList(); }
            }			

			public static List<ACDItem> WestmarchHolyWater
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.WestmarchHolyWater).ToList(); }
            }		
			
			public static List<ACDItem> ArreatWarTapestry
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.ArreatWarTapestry).ToList(); }
            }			

			public static List<ACDItem> CorruptedAngelFlesh
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.CorruptedAngelFlesh).ToList(); }
            }	
			
			public static List<ACDItem> KhanduranRune
            {
                get { return AllStashItems.Where(i => i.ActorSNO == (int)InventoryItemType.KhanduranRune).ToList(); }
            }	            			
        }

        /// <summary>
        /// Return total stack quantity of all stacks
        /// </summary>
        public static int StackQuantity(this IEnumerable<ACDItem> items)
        {
            return items.Select(i => (int)i.ItemStackQuantity).Sum();
        }
    }
}

