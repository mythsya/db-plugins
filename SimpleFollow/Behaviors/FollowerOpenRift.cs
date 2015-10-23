using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestTools.Helpers;
using QuestTools.ProfileTags;
using QuestTools.ProfileTags.Movement;
using SimpleFollow.Helpers;
using SimpleFollow.ProfileBehaviors;
using Zeta.Bot;
using Zeta.Bot.Coroutines;
using Zeta.Bot.Profile.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.TreeSharp;

namespace SimpleFollow.Behaviors
{
    internal class FollowerOpenRift
    {
        public const int RiftPortalSNO = 345935;
        public const int GreaterRiftPortalSNO = 396751;

        private static readonly Func<ACDItem, bool> IsRiftKey = item => item.ItemType == ItemType.KeystoneFragment;
        private static readonly Func<ACDItem, bool> IsInBackPack = item => item.InventorySlot == InventorySlot.BackpackItems;
        private static readonly Func<ACDItem, bool> IsInStash = item => item.InventorySlot == InventorySlot.SharedStash;
        private static readonly Func<ACDItem, bool> IsGreaterRiftKey = item => item.TieredLootRunKeyLevel > 0;
        private static readonly Func<ACDItem, bool> IsTrialRiftKey = item => item.TieredLootRunKeyLevel == 0;
        private static readonly Func<ACDItem, bool> IsNormalKey = item => item.TieredLootRunKeyLevel < 0;
        private static readonly Func<ACDItem, long> GetBackpackStackCount = item => ZetaDia.Me.Inventory.Backpack.Where(i => i.ActorSNO == item.ActorSNO).Sum(i => i.ItemStackQuantity);
        private static readonly Func<ACDItem, long> GetStashCount = item => ZetaDia.Me.Inventory.StashItems.Where(i => i.ActorSNO == item.ActorSNO).Sum(i => i.ItemStackQuantity);

        public static Composite OpenRiftBehavior()
        {
            return new ActionRunCoroutine(ret => OpenRiftTask());
        }

        private static async Task<bool> OpenRiftTask()
        {
            // Move to and use Greater Rift Portal
            if (ZetaDia.Me.IsParticipatingInTieredLootRun && ZetaDia.IsInTown && ZetaDia.CurrentLevelAreaId == (int) SNOLevelArea.x1_Westm_Hub &&
                ConditionParser.IsActiveQuestAndStep((int) SNOQuest.X1_LR_DungeonFinder, 13))
            {
                // [202EBD3C] GizmoType: Portal Name: X1_OpenWorld_Tiered_Rifts_Portal-5041 ActorSNO: 396751 Distance: 11.58544 Position: <606.84, 750.39, 2.53> Barracade: False Radius: 8.316568
                var moveToPortal = new MoveToActorTag
                {
                    QuestId = (int) SNOQuest.X1_LR_DungeonFinder,
                    StepId = 13,
                    ActorId = GreaterRiftPortalSNO,
                    X = 606,
                    Y = 750,
                    Z = 2,
                    IsPortal = true,
                    InteractRange = 9,
                    Timeout = 10
                };
                Logr.Log("Queueing MoveToActor for interacting with Greater Rift Portal");
                BotBehaviorQueue.Queue(moveToPortal,
                    ret => ZetaDia.Me.IsParticipatingInTieredLootRun && ZetaDia.IsInTown && ZetaDia.CurrentLevelAreaId == (int) SNOLevelArea.x1_Westm_Hub &&
                           !ConditionParser.IsActiveQuestAndStep(337492, 10));
                return true;
            }

            if (!SimpleFollow.Leader.RequestOpenRift)
                return false;

            if (HighestLevelTieredRiftKey > SimpleFollow.Leader.HighestTeamRiftKey)
                return false;

            // In regular rift quests
            if (ConditionParser.IsActiveQuestAndStep(337492, 1) ||
                ConditionParser.IsActiveQuestAndStep(337492, 3) ||
                ConditionParser.IsActiveQuestAndStep(337492, 13) ||
                ConditionParser.IsActiveQuestAndStep(337492, 16))
                return false;

            // In Rift Trial
            if (ZetaDia.ActInfo.ActiveQuests.Any(q => q.QuestSNO == (int) SNOQuest.p1_TieredRift_Challenge))
                return false;

            // Our rift key is the lowest of the team, lets open a rift!

            var keyPriorityList = SimpleFollow.Leader.RiftKeyPriority;

            if (keyPriorityList.Count != 3)
                throw new ArgumentOutOfRangeException("RiftKeyPriority", "Expected 3 Rift keys, API is broken?");

            if (ZetaDia.Actors.GetActorsOfType<DiaObject>(true).Any(i => i.IsValid && i.ActorSNO == RiftPortalSNO))
            {
                Logr.Log("Rift Portal already open!");
                return false;
            }

            if (!ZetaDia.IsInTown)
                return await CommonCoroutines.UseTownPortal("Going to town to open rift");

            bool needsGreaterKeys = !ZetaDia.Me.Inventory.Backpack.Any(i => IsRiftKey(i) && IsGreaterRiftKey(i)) &&
                                    ZetaDia.Me.Inventory.StashItems.Any(i => IsRiftKey(i) && IsGreaterRiftKey(i));
            bool needsTrialKeys = !ZetaDia.Me.Inventory.Backpack.Any(i => IsRiftKey(i) && IsTrialRiftKey(i)) &&
                                  ZetaDia.Me.Inventory.StashItems.Any(i => IsRiftKey(i) && IsTrialRiftKey(i));
            bool needsNormalKeys = !ZetaDia.Me.Inventory.Backpack.Any(i => IsRiftKey(i) && IsNormalKey(i)) &&
                                   ZetaDia.Me.Inventory.StashItems.Any(i => IsRiftKey(i) && IsNormalKey(i));

            if (needsGreaterKeys)
            {
                Logr.Log("Moving to stash to get Greater Rift Keys");
                BotBehaviorQueue.Queue(new GetItemFromStashTag {GreaterRiftKey = true, StackCount = 1});
                return true;
            }

            if (needsTrialKeys)
            {
                Logr.Log("Moving to stash to get Trial Rift Keys");
                BotBehaviorQueue.Queue(new GetItemFromStashTag {ActorId = 408416, StackCount = 1});
                return true;
            }

            if (needsNormalKeys)
            {
                Logr.Log("Moving to stash to get Rift Keys");
                BotBehaviorQueue.Queue(new GetItemFromStashTag {ActorId = (int) SNOActor.LootRunKey, StackCount = 1});
                return true;
            }

            int questStepId = 1;
            var quest = ZetaDia.ActInfo.ActiveQuests.FirstOrDefault(q => q.QuestSNO == (int) SNOQuest.X1_LR_DungeonFinder);
            if (quest != null)
                questStepId = quest.QuestStep;

            if (ZetaDia.IsInTown && !ConditionParser.ActorExistsAt(RiftPortalSNO, 606, 750, 2, 50) && !ConditionParser.ActorExistsAt(GreaterRiftPortalSNO, 606, 750, 2, 50))
            {
                BotBehaviorQueue.Reset();
                Logr.Log("Queueing QTOpenRiftWrapper");
                BotBehaviorQueue.Queue(new QTOpenRiftWrapperTag());
                BotBehaviorQueue.Queue(new WaitTimerTag {QuestId = 337492, StepId = questStepId, WaitTime = 2000});
                FollowTag.LastInteractTime = DateTime.Now.AddSeconds(15);
            }

            if (ZetaDia.IsInTown && ConditionParser.ActorExistsAt(RiftPortalSNO, 606, 750, 2, 50) && ConditionParser.IsActiveQuestAndStep(337492, questStepId))
            {
                Logr.Log("Queueing MoveToActor for Rift Portal");
                BotBehaviorQueue.Reset();
                BotBehaviorQueue.Queue(new MoveToActorTag {QuestId = 337492, StepId = questStepId, ActorId = RiftPortalSNO, Timeout = 10});
                BotBehaviorQueue.Queue(new WaitTimerTag {QuestId = 337492, StepId = questStepId, WaitTime = 2000});
                FollowTag.LastInteractTime = DateTime.Now.AddSeconds(15);
            }
            if (ZetaDia.IsInTown && ConditionParser.ActorExistsAt(GreaterRiftPortalSNO, 606, 750, 2, 50) && ConditionParser.IsActiveQuestAndStep(337492, questStepId))
            {
                Logr.Log("Queueing MoveToActor for Greater Rift Portal");
                BotBehaviorQueue.Reset();
                BotBehaviorQueue.Queue(new MoveToActorTag {QuestId = 337492, StepId = questStepId, ActorId = GreaterRiftPortalSNO, Timeout = 10});
                BotBehaviorQueue.Queue(new WaitTimerTag {QuestId = 337492, StepId = questStepId, WaitTime = 2000});
                FollowTag.LastInteractTime = DateTime.Now.AddSeconds(15);
            }
            return true;
        }

        private static ACDItem GetGreaterKeystone(bool useLowest, bool useTrialStone)
        {
            var backpackItems = ZetaDia.Me.Inventory.Backpack.Where(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment).ToList();

            ACDItem greaterKeystone = backpackItems
                .OrderByDescending(o => o.TieredLootRunKeyLevel)
                .FirstOrDefault(i => i.TieredLootRunKeyLevel > 0);

            if (useLowest)
                greaterKeystone = backpackItems.
                    OrderBy(o => o.TieredLootRunKeyLevel).
                    FirstOrDefault(i => i.TieredLootRunKeyLevel > 0);

            if (useTrialStone)
                greaterKeystone = backpackItems.
                    FirstOrDefault(i => i.TieredLootRunKeyLevel == 0);

            return greaterKeystone;
        }

        public static int HighestLevelTieredRiftKey
        {
            get
            {
                List<ACDItem> riftKeys = ZetaDia.Actors.GetActorsOfType<ACDItem>(true)
                    .Where(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment).ToList();
                return riftKeys.Any() ? riftKeys.Max(i => i.TieredLootRunKeyLevel) : -1;
            }
        }

        public static bool HasGreaterRiftKeys
        {
            get { return ZetaDia.Actors.GetActorsOfType<ACDItem>().Any(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment && i.TieredLootRunKeyLevel > 0); }
        }

        public static bool HasTrialRiftKeys
        {
            get { return ZetaDia.Actors.GetActorsOfType<ACDItem>().Any(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment && i.TieredLootRunKeyLevel == 0); }
        }

        public static bool HasNormalRiftKeys
        {
            get { return ZetaDia.Actors.GetActorsOfType<ACDItem>().Any(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment && i.TieredLootRunKeyLevel < 0); }
        }
    }
}