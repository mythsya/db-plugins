﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adventurer.Game.Exploration;
using Adventurer.Game.Rift;
using Adventurer.Util;
using Zeta.Bot.Settings;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.Game.Actors
{
    public static class ActorFinder
    {
        private static int _radiusChangeCount;
        public static int LowerSearchRadius(int currentSearchRadius)
        {
            if (_radiusChangeCount >= 2)
            {
                Logger.Info(
                    "[ActorFinder] It looks like we couldn't path to the destination, lowering the search radius.");
                _radiusChangeCount = 0;
                return 100;
            }
            _radiusChangeCount++;
            if (currentSearchRadius >= 1500)
            {
                return currentSearchRadius / 2;
            }
            if (currentSearchRadius >= 500)
            {
                return currentSearchRadius - 100;
            }
            if (currentSearchRadius >= 100)
            {
                return currentSearchRadius - 50;
            }
            if (currentSearchRadius > 20)
            {
                return currentSearchRadius - 10;
            }
            return currentSearchRadius;
        }

        public static Vector3 FindNearestHostileUnitInRadius(Vector3 center, float radius)
        {
            using (new PerformanceLogger())
            {
                try
                {
                    var actor = ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Where(
                        u =>
                            u.IsValid &&
                            u.CommonData != null &&
                            u.CommonData.IsValid &&
                            center.Distance2D(u.Position) <= radius &&
                            AdvDia.MyPosition.Distance2D(u.Position) > CharacterSettings.Instance.KillRadius &&
                            u.IsHostile &&
                            !u.IsDead &&
                            u.HitpointsCurrent > 1 &&
                            !u.IsInvulnerable &&
                            u.CommonData.GetAttribute<int>(ActorAttributeType.Invulnerable) <= 0 &&
                            u.CommonData.GetAttribute<int>(ActorAttributeType.Untargetable) <= 0 &&
                            !u.Name.Contains("bloodSpring") &&
                            !u.Name.Contains("firewall") &&
                            !u.Name.Contains("FireGrate")

                            )
                        .OrderBy(u => center.Distance2D(u.Position))
                        .FirstOrDefault();
                    if (actor != null)
                    {
                        return actor.Position;
                    }

                }
                catch (Exception)
                {
                    return Vector3.Zero;
                }
                return Vector3.Zero;
            }
        }




        public static DiaUnit FindUnitByGuid(int acdGuid)
        {
            return ZetaDia.Actors.GetActorByACDId(acdGuid) as DiaUnit;
        }

        public static DiaObject FindObject(int actorId)
        {
            return ZetaDia.Actors.GetActorsOfType<DiaObject>(true).Where(
            u =>
                u.IsValid &&
                u.CommonData != null &&
                u.CommonData.IsValid &&
                u.ActorSNO == actorId
            ).OrderBy(u => u.Distance).FirstOrDefault();
        }

        public static DiaUnit FindUnit(int actorId)
        {
            return ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Where(
                u =>
                    u.IsValid &&
                    u.CommonData != null &&
                    u.CommonData.IsValid &&
                    u.ActorSNO == actorId
                ).OrderBy(u => u.Distance).FirstOrDefault();
        }

        public static DiaGizmo FindGizmo(int actorId)
        {
            return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).Where(
                u =>
                    u.IsValid &&
                    u.CommonData != null &&
                    u.CommonData.IsValid &&
                    u.ActorSNO == actorId
                ).OrderBy(u => u.Position.DistanceSqr(AdvDia.MyPosition)).FirstOrDefault();
        }

        public static DiaGizmo FindGizmoByName(string actorName)
        {
            return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).Where(
                u =>
                    u.IsValid &&
                    u.CommonData != null &&
                    u.CommonData.IsValid &&
                    u.Name == actorName
                ).OrderBy(u => u.Distance).FirstOrDefault();
        }

        public static DiaGizmo FindNearestDeathGate(List<int> ignoreList)
        {
            if (ignoreList == null)
            {
                ignoreList = new List<int>();
            }
            //328830
            if (ExplorationData.FortressLevelAreaIds.Contains(AdvDia.CurrentLevelAreaId) || ExplorationData.FortressWorldIds.Contains(AdvDia.CurrentWorldId))
            {
                var gizmo =
                    ZetaDia.Actors.GetActorsOfType<DiaObject>(true)
                        //                        .Where(u => u.IsValid && u.ActorSNO == 328830 && !ignoreList.Contains(u.ACDGuid))
                        .Where(u => u.IsValid && u.ActorSNO == 328830 && u.Distance < 200)
                        .OrderBy(u => u.Distance)
                        .FirstOrDefault();
                return gizmo != null ? gizmo as DiaGizmo : null;
            }
            return null;
        }

        public static bool IsFullyValid<T>(this T actor) where T : DiaObject
        {
            return actor.IsValid && actor.CommonData != null && actor.CommonData.IsValid;
        }

        public static bool IsInteractableQuestObject<T>(this T actor) where T : DiaObject
        {
            if (actor is DiaUnit)
            {
                return IsUnitInteractable(actor as DiaUnit);
            }
            if (actor is DiaGizmo)
            {
                return IsGizmoInteractable(actor as DiaGizmo);
            }
            return false;
        }

        public static bool IsUnitInteractable(DiaUnit unit)
        {
            if (!unit.IsFullyValid())
            {
                return false;
            }
            if ((unit.CommonData.MinimapVisibilityFlags & 0x80) != 0)
            {
                return true;
            }
            if (unit.CommonData.GetAttribute<int>(ActorAttributeType.MinimapActive) == 1)
            {
                return true;
            }
            if (unit.MarkerType != MarkerType.Invalid && unit.MarkerType != MarkerType.None && unit.MarkerType != MarkerType.Asterisk)
            {
                return true;
            }
            if (InteractWhitelist.Contains(unit.ActorSNO))
            {
                return true;
            }
            return false;
        }

        private static readonly HashSet<int> GizmoInteractBlacklist = new HashSet<int>
                                                                      {
                                                                          (int)SNOActor.caOut_Boneyards_Dervish_Alter-10883,
                                                                          (int)SNOActor.caOut_Oasis_Mine_Entrance_A-26687,
                                                                      };

        private static readonly HashSet<int> GuardedGizmos = new HashSet<int>
                                                             {
                                                                 434366,
                                                                 430733,
                                                                 432259,
                                                                 432770,
                                                                 433051,
                                                                 433184,
                                                                 433385,
                                                                 433124,
                                                                 433402,
                                                                 433316,
                                                                 433246,
                                                                 433295
                                                             };

        private static readonly HashSet<int> CursedGizmos = new HashSet<int>
                                                            {
                                                                364601,
                                                                365097,
                                                                368169
                                                            };

        public static readonly HashSet<int> InteractWhitelist = new HashSet<int>
                                                            {
                                                                          RiftData.UrshiSNO,
                                                                301177,
                                                                363744,
                                                                93713,
                                                                331397,
                                                                183609,
                                                                289249,
                                                                // A5 - Bounty: Finding the Forgotten (368611)
                                                                309381,
                                                                309400,
                                                                309398,
                                                                309387,
                                                                309403,
                                                                309391,
                                                                309410,
                                                                309380,
                                                                // A5 - Bounty: A Shameful Death (368445)
                                                                321930,
                                                                // A2 - Bounty: Lost Treasure of Khan Dakab (346067)
                                                                175603,
                                                                328830, // Death Gate
                                                                114622,
                                                                // Guarded Gizmos
                                                                 434366,
                                                                 430733,
                                                                 432259,
                                                                 432770,
                                                                 433051,
                                                                 433184,
                                                                 433385,
                                                                 433124,
                                                                 433402,
                                                                 433316,
                                                                 433246

                                                            };

        public static bool IsGizmoInteractable(DiaGizmo gizmo)
        {
            if (!gizmo.IsFullyValid())
            {
                return false;
            }
            if (GuardedGizmos.Contains(gizmo.ActorSNO) && gizmo.HasBeenOperated)
            {
                return false;
            }
            if (GuardedGizmos.Contains(gizmo.ActorSNO) && !gizmo.HasBeenOperated && gizmo.CommonData.GetAttribute<int>(ActorAttributeType.Untargetable) != 1)
            {
                return true;
            }
            switch (gizmo.CommonData.GizmoType)
            {
                case GizmoType.Portal:
                case GizmoType.DungeonPortal:
                    return !AdvDia.CurrentWorldMarkers.Any(m => m.Position.Distance2D(gizmo.Position) < 10 && EntryPortals.IsEntryPortal(AdvDia.CurrentWorldDynamicId, m.NameHash));
                case GizmoType.PortalDestination:
                case GizmoType.PoolOfReflection:
                case GizmoType.Headstone:
                case GizmoType.HealingWell:
                case GizmoType.PowerUp:
                    return false;
                case GizmoType.LootRunSwitch:
                    return true;
                case GizmoType.Switch:
                    if (gizmo.ActorSNO == 328830) return true;
                    break;
            }
            if (GizmoInteractBlacklist.Contains(gizmo.ActorSNO))
            {
                return false;
            }
            //if (GizmoInteractWhitelist.Contains(gizmo.ActorSNO))
            //{
            //    return true;
            //}
            if (gizmo.IsGizmoDisabledByScript || gizmo.CommonData.GetAttribute<int>(ActorAttributeType.Untargetable) == 1)
            {
                return false;
            }

            if (gizmo.HasBeenOperated)
            {
                return false;
            }
            //if (gizmo is GizmoLootContainer || gizmo is GizmoShrine || gizmo.CommonData.GizmoType == GizmoType.Switch)
            //{
            //    return true;
            //}
            if ((gizmo.CommonData.MinimapVisibilityFlags & 0x80) != 0)
            {
                return true;
            }
            if (gizmo.CommonData.GetAttribute<int>(ActorAttributeType.MinimapActive) == 1)
            {
                return true;
            }
            if (CursedGizmos.Contains(gizmo.ActorSNO))
            {
                return true;
            }
            if (InteractWhitelist.Contains(gizmo.ActorSNO))
            {
                return true;
            }

            return false;
        }
    }
}
