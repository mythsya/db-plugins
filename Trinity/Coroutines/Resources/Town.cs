using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace TrinityCoroutines.Resources
{
    public static class Town
    {
        public static class ActorIds
        {
            // [1CD8B0CC] Type: Monster Name: PT_Jeweler_AddSocketShortcut-40 ActorSNO: 212519, Distance: 6.778345

            public const int Stash = 130400;
            public const int TheQuaterMaster = 309718;
            public const int ThePeddler = 180783;
            public const int TheMiner = 178396;
            public const int Tyrael = 114622;
            public const int BookOfCain = 297813;
            public const int Jeweler = 212519; // NPC=56949;
            public const int BlackSmith = 195171; // NPC=56947
            public const int Kadala = 361241;
            public const int KanaisCube = 439975;
            public const int RiftObelisk = 364715;
            public const int TheCollector = 178362;
            public const int TheMystic = 212511;
            public const int TheFence = 178388;
            public const int ZultonKule = 429005;
        }

        public static HashSet<int> VendorIds = new HashSet<int>
        {
            ActorIds.TheFence,
            ActorIds.TheQuaterMaster,
            ActorIds.ThePeddler,
            ActorIds.TheMiner,
            ActorIds.TheCollector,
        };

        public static DiaObject NearestVendor
        {
            get { return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).Where(o => VendorIds.Contains(o.ActorSNO)).OrderBy(o => o.Distance).FirstOrDefault(); }
        }

        public static class Actors
        {
            public static DiaObject Stash 
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.Stash); }
            }

            public static DiaObject TheQuaterMaster
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.TheQuaterMaster); }
            }
            
            public static DiaObject ThePeddler
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.ThePeddler); }
            }

            public static DiaObject TheMiner
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.TheMiner); }
            }

            public static DiaObject Tyrael
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.Tyrael); }
            }

            public static DiaObject BookOfCain
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.BookOfCain); }
            }

            public static DiaObject Jeweler
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.Jeweler); }
            }

            public static DiaObject BlackSmith
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.BlackSmith); }
            }

            public static DiaObject Kadala
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.Kadala); }
            }

            public static DiaObject KanaisCube
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.KanaisCube); }
            }

            public static DiaUnit ZultonKule
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.ZultonKule && o.IsValid && o.CommonData != null && o.CommonData.IsValid && !o.CommonData.IsDisposed); }
            }

            public static DiaObject RiftObelisk
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.RiftObelisk); }
            } 

            public static DiaObject TheCollector
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.TheCollector); }
            }
 
            public static DiaObject TheMystic
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.TheMystic); }
            } 

            public static DiaObject TheFence
            {
                get { return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(o => o.ActorSNO == ActorIds.TheFence); }
            }    
        }

        /// <summary>
        /// Moving to fixed locations is the most reliable way to navigate around town.
        /// If you try to move to the actor directly, depending on where you are in town 
        /// the actor might not be found and your code will fail.
        /// </summary>
        public static class Locations
        {
            public static Vector3 Stash
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return new Vector3(2968.16f, 2789.63f, 23.94531f);
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(388.16f, 509.63f, 23.94531f);
                        case 168314: // A2 Hub
                            return new Vector3(323.0558f, 222.7048f, 0f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(387.6834f, 382.0295f, 0f);
                        case 270011: // A5 Hub
                            return new Vector3(502.8296f, 739.7472f, 2.598635f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 Kadala
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(379.0819f, 574.2164f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(339.3643f, 263.1351f, 0.1000038f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(471.7942f, 411.3196f, 0.498693f);
                        case 270011: // A5 Hub
                            return new Vector3(605.2736f, 774.6306f, 2.745199f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 BlackSmith
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(359.7123f, 567.8089f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(274.3481f, 220.0129f, 0.1f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(327.7397f, 420.6676f, 0.1382296f);
                        case 270011: // A5 Hub
                            return new Vector3(539.5216f, 720.2468f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 Jeweler
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(338.3618f, 505.4174f, 24.04532f);
                        case 168314: // A2 Hub
                            return new Vector3(376.7611f, 224.5412f, 0.1f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(404.3205f, 322.8709f, 0.1000005f);
                        case 270011: // A5 Hub
                            return new Vector3(610.3714f, 722.965f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 Enchanter
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(256.3538f, 296.8587f, 0.100003f);
                        case 168314: // A2 Hub
                            return new Vector3(376.7611f, 224.5412f, 0.1f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(313.3967f, 308.8749f, 0.1000005f);
                        case 270011: // A5 Hub
                            return new Vector3(532.546f, 804.4268f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }
            
            public static Vector3 KanaisCube
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(419.0077f, 581.7107f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(290.3684f, 323.3933f, 0.1000038f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(427.6537f, 493.0599f, 0.4094009f);
                        case 270011: // A5 Hub
                            return new Vector3(574.3948f, 807.548f, 2.620763f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 ClosestMerchantToCube
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(430.3188f, 577.7206f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(291.7876f, 278.9904f, 0.1000038f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(392.6478f, 513.2823f, 0.1f);
                        case 270011: // A5 Hub
                            return new Vector3(624.8824f, 819.2529f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 TheCollector
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(430.3188f, 577.7206f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(359.6478f, 133.995f, -16.39508f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(444.4589f, 321.4779f, 0.1000005f);
                        case 270011: // A5 Hub
                            return new Vector3(625.013f, 815.3127f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 TheFence
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(331.6506f, 301.1718f, 0.599789f);
                        case 168314: // A2 Hub
                            return new Vector3(337.4676f, 135.7881f, -16.39509f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(444.4589f, 321.4779f, 0.1000005f);
                        case 270011: // A5 Hub
                            return new Vector3(621.2771f, 797.3148f, 2.620764f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 TheMiner
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 19947: // Campaign A1 Hub
                            return Vector3.Zero;
                        case 332339: // OpenWorld A1 Hub
                            return new Vector3(430.3188f, 577.7206f, 24.04533f);
                        case 168314: // A2 Hub
                            return new Vector3(348.0609f, 153.5159f, -16.39508f);
                        case 92945: // A3/A4 Hub
                            return new Vector3(441.0552f, 513.9581f, 0.1f);
                        case 270011: // A5 Hub
                            return new Vector3(425.7704f, 820.938f, 7.421445f);
                        default:
                            throw new ValueUnavailableException("Unknown LevelArea Id " + levelAreaId);
                    }
                }
            }

            public static Vector3 TheQuarterMaster
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 270011: // A5 Hub
                            return new Vector3(545.3922f, 775.2578f, 2.782243f);
                        default:
                            throw new ValueUnavailableException(String.Format("There is no QuarterMaster merchant for this act ({0})", levelAreaId));
                    }
                }
            }

            public static Vector3 ThePeddler
            {
                get
                {
                    var levelAreaId = ZetaDia.CurrentLevelAreaId;
                    switch (levelAreaId)
                    {
                        case 168314: // A2 Hub
                            return new Vector3(291.3571f, 276.9437f, 0.1000038f);
                        default:
                            throw new ValueUnavailableException(String.Format("There is no QuarterMaster merchant for this act ({0})", levelAreaId));
                    }
                }
            }

            public static Vector3 GetLocationFromActorId(int vendorId)
            {
                switch (vendorId)
                {
                    case ActorIds.Stash:
                        return Stash;
                    case ActorIds.TheFence:
                        return TheFence;
                    case ActorIds.TheMiner:
                        return TheMiner;
                    case ActorIds.ThePeddler:
                        return ThePeddler;
                    case ActorIds.TheQuaterMaster:
                        return TheQuarterMaster;
                    case ActorIds.Kadala:
                        return Kadala;
                    default:
                        throw new ValueUnavailableException(String.Format("There is no position available for this actorId ({0})", vendorId));
                }
            }

        }

        public enum VendorSlot
        {
            None = 0,
            OneHandItem,
            TwoHandItem,
            Quiver,
            Orb,
            Mojo,
            Helm,
            Gloves,
            Boots,
            Chest,
            Belt,
            Shoulder,
            Pants,
            Bracers,
            Shield,
            Ring,
            Amulet,
        }

        public static Dictionary<VendorSlot, int> MysterySlotTypeAndId = new Dictionary<VendorSlot, int>
            {
                {VendorSlot.OneHandItem,377355},
                {VendorSlot.TwoHandItem,377356},
                {VendorSlot.Quiver,377360},
                {VendorSlot.Orb,377358},
                {VendorSlot.Mojo,377359},
                {VendorSlot.Helm,377344},
                {VendorSlot.Gloves,377346},
                {VendorSlot.Boots,377347},
                {VendorSlot.Chest,377345},
                {VendorSlot.Belt,377349},
                {VendorSlot.Pants,377350},
                {VendorSlot.Bracers,377351},
                {VendorSlot.Shield,377357},
                {VendorSlot.Ring,377352},
                {VendorSlot.Amulet,377353},
                {VendorSlot.Shoulder,377348}
            };

        public static Dictionary<VendorSlot, int> MysterySlotTypeAndPrice = new Dictionary<VendorSlot, int>
            {
                {VendorSlot.OneHandItem,75},
                {VendorSlot.TwoHandItem,75},
                {VendorSlot.Quiver,25},
                {VendorSlot.Orb,25},
                {VendorSlot.Mojo,25},
                {VendorSlot.Helm,25},
                {VendorSlot.Gloves,25},
                {VendorSlot.Boots,25},
                {VendorSlot.Chest,25},
                {VendorSlot.Belt,25},
                {VendorSlot.Pants,25},
                {VendorSlot.Bracers,25},
                {VendorSlot.Shield,25},
                {VendorSlot.Ring,50},
                {VendorSlot.Amulet,100},
                {VendorSlot.Shoulder,25}
            };

    }
}
