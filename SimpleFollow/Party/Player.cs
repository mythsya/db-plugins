using System;
using System.Linq;
using SimpleFollow.Helpers;
using SimpleFollow.UI;
using Zeta.Bot.Logic;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace SimpleFollow.Party
{
    public class Player
    {
        private const int UpdateInterval = 300;

        public int RActorGuid { get; set; }
        public int ACDGuid { get; set; }
        public double HitpointsCurrent { get; set; }
        public double HitpointsMaxTotal { get; set; }
        public double HitpointsCurrentPct { get; set; }
        public Vector3 Position { get; set; }
        public int CurrentLevelAreaId { get; set; }
        public int CurrentWorldId { get; set; }
        public int CurrentDynamicWorldId { get; set; }
        public bool IsInTown { get; set; }
        public bool IsInGame { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsParticipatingInTieredLootRun { get; set; }
        public int InTieredLootRunLevel { get; set; }

        public bool IsValid
        {
            get { return ZetaDia.Me.IsValid; }
        }

        public bool IsVendoring { get; set; }

        private static int _cachedLevelAreaId = -1;
        private static DateTime _lastUpdatedLevelAreaId = DateTime.MinValue;

        public static int LevelAreaId
        {
            get
            {
                if (!ZetaDia.IsInGame)
                    return 0;
                if (ZetaDia.IsLoadingWorld)
                    return 0;
                if (ZetaDia.Me == null)
                    return 0;
                if (!ZetaDia.Me.IsValid)
                    return 0;

                if (_cachedLevelAreaId == -1 || DateTime.UtcNow.Subtract(_lastUpdatedLevelAreaId).TotalSeconds > 2)
                {
                    _cachedLevelAreaId = ZetaDia.CurrentLevelAreaId;
                    _lastUpdatedLevelAreaId = DateTime.UtcNow;
                    return _cachedLevelAreaId;
                }
                return _cachedLevelAreaId;
            }
        }

        public static bool IsInGreaterRift
        {
            get { return ZetaDia.Me.IsParticipatingInTieredLootRun; }
        }

        public static bool IsGreaterRiftStarted
        {
            get
            {
                int[] greaterRiftQuestSteps = {2, 13, 16, 34};
                const int riftQuest = 337492;
                if (ZetaDia.CurrentQuest.QuestSNO == riftQuest && greaterRiftQuestSteps.Contains(ZetaDia.CurrentQuest.StepId))
                    return true;
                return false;
            }
        }

        private static Player _instance;

        public static Player Instance
        {
            get
            {
                // Reconstruct if needed
                if (_instance != null && !_instance.IsValid)
                    _instance = new Player();
                return _instance ?? (_instance = new Player());
            }
        }

        public Player()
        {
            LastUpdate = DateTime.MinValue;
            RActorGuid = ZetaDia.Me.RActorGuid;
            Update();
        }

        public Player(int rActorGuid)
        {
            RActorGuid = rActorGuid;
            Update();
        }

        private static string _lastLogMessage = "";

        public void Update()
        {
            if (DateTime.UtcNow.Subtract(LastUpdate).TotalMilliseconds < Settings.Instance.UpdateInterval)
                return;

            LastUpdate = DateTime.UtcNow;
            IsInGame = ZetaDia.IsInGame;

            if (!ZetaDia.IsInGame || ZetaDia.IsLoadingWorld || !ZetaDia.Me.IsValid)
                return;

            ACDGuid = ZetaDia.Me.ACDGuid;
            HitpointsCurrent = ZetaDia.Me.HitpointsCurrent;
            HitpointsCurrentPct = ZetaDia.Me.HitpointsCurrentPct;
            HitpointsMaxTotal = ZetaDia.Me.HitpointsMaxTotal;
            Position = ZetaDia.Me.Position;
            CurrentLevelAreaId = LevelAreaId;
            CurrentWorldId = ZetaDia.CurrentWorldId;
            CurrentDynamicWorldId = ZetaDia.CurrentWorldDynamicId;
            IsInTown = ZetaDia.IsInTown;
            IsVendoring = BrainBehavior.IsVendoring;
            IsParticipatingInTieredLootRun = ZetaDia.Me.IsParticipatingInTieredLootRun;
            InTieredLootRunLevel = ZetaDia.Me.InTieredLootRunLevel;

            if (_lastLogMessage != ToString())
            {
                _lastLogMessage = ToString();
                if (Settings.Instance.DebugLogging)
                    Logr.Debug("Updated {0}", ToString());
            }
        }

        public bool UsePower(SNOPower power, Vector3 position)
        {
            return ZetaDia.Me.UsePower(power, position);
        }

        public override string ToString()
        {
            return String.Format("Player: RActorGuid={0} ACDGuid={1} HitpointsCurrent={2:0} HitpointsCurrentPct={3:0} HitpointsMaxTotal={4:0} Position={5} LevelAreaId={6} WorldId={7} DynamicWorldId={8} IsInGame={9} IsInTown={10} IsVendoring: {11}",
                RActorGuid, ACDGuid, HitpointsCurrent, HitpointsCurrentPct*100, HitpointsMaxTotal, Position, CurrentLevelAreaId, CurrentWorldId, CurrentDynamicWorldId, IsInGame, IsInTown, IsVendoring);
        }
    }
}