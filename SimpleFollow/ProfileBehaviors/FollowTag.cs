using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using QuestTools.Helpers;
using QuestTools.ProfileTags;
using QuestTools.ProfileTags.Movement;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Network;
using SimpleFollow.Party;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Bot.Coroutines;
using Zeta.Bot.Dungeons;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
using Zeta.Bot.Profile;
using Zeta.Common;
using Zeta.Common.Plugins;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.Actors.Gizmos;
using Zeta.TreeSharp;
using Zeta.XmlEngine;
using Action = Zeta.TreeSharp.Action;

namespace SimpleFollow.ProfileBehaviors
{
    [XmlElement("Follow")]
    internal class FollowTag : ProfileBehavior
    {
        [XmlAttribute("distance")]
        private float Distance { get; set; }

        public int LeaderDistance
        {
            get
            {
                return (int)Leader.Position.Distance2D(Me.Position);
            }
        }

        private CachedValue<bool> _cachedLootRunCheck;
        public bool IsParticipatingInTieredLootRun
        {
            get
            {
                if (_cachedLootRunCheck == null)
                    _cachedLootRunCheck = new CachedValue<bool>(GetIsParticipatingInTieredLootRun, TimeSpan.FromSeconds(1));
                return _cachedLootRunCheck.Value;
            }
        }
        public bool ShouldUseProfilePosition
        {
            get
            {
                return
                    !Leader.IsInCombat &&
                    Settings.Instance.UseProfilePosition &&
                    Leader.ProfilePosition != Vector3.Zero &&
                    Leader.ProfilePosition.Distance2D(Me.Position) < 300f &&
                    !ProfilePositionCache.Contains(Leader.ProfilePosition);
            }
        }

        public bool ShouldTownRun
        {
            get
            {
                try
                {
                    bool townrun = FreeBagSlotsPercent <= 0.90;
                    if (!townrun && ZetaDia.Me.Inventory.Equipped.Any(i => i.IsValid && i.DurabilityPercent < 0.99))
                        townrun = true;

                    return townrun;
                }
                catch (Exception ex)
                {
                    Logr.Error("Error getting ShouldTownRun " + ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Banners in the Actor list
        /// </summary>
        private List<GizmoBanner> _banners = new List<GizmoBanner>();

        private Vector3 _playerLastSeenPos = Vector3.Zero;
        private DateTime _playerLastSeenTime = DateTime.UtcNow;
        private Vector2 _playerLastSeenMarker = Vector2.Zero;
        private bool _initialized;
        private ActorCache _cachedActor;

        public ActorCache LeaderPortal
        {
            get
            {
                if (_cachedActor != null && _cachedActor.IsValid)
                {
                    return _cachedActor;
                }

                DiaGizmo portal = null;
                var portals = ZetaDia.Actors.GetActorsOfType<GizmoPortal>(true).Where(p => p.IsValid && p.CommonData != null && p.CommonData.IsValid).ToList();

                if (!portals.Any())
                    return new ActorCache();

                if (PlayerTookPortal() && !IsParticipatingInTieredLootRun)
                    portal = portals.OrderBy(p => p.Position.Distance(_playerLastSeenPos)).FirstOrDefault();
                else
                {
                    MinimapMarker exitMarker = ZetaDia.Minimap.Markers.CurrentWorldMarkers
                        .Where(m => m.IsValid && m.IsPortalExit && m.Position != Vector3.Zero)
                        .OrderBy(m => m.Position.Distance2DSqr(Me.Position))
                        .FirstOrDefault();
                    if (exitMarker != null)
                        portal = portals.OrderBy(p => p.Position.Distance2DSqr(exitMarker.Position)).FirstOrDefault();
                }
                if (portal != null && portal.IsValid && portal.CommonData.IsValid)
                    _cachedActor = new ActorCache(portal);
                return _cachedActor;
            }
        }

        private DiaPlayer _diaLeader;
        private IEnumerable<DiaGizmo> _headstones = new List<DiaGizmo>();
        private static readonly DateTime _lastLocalDataUpdate = DateTime.MinValue;
        private static DateTime _lastInteractTime = DateTime.MinValue;
        private int _lastWorldId = -1;

        public static DateTime LastInteractTime
        {
            get { return _lastInteractTime; }
            set { _lastInteractTime = value; }
        }

        public static DateTime LastLocalDataUpdate
        {
            get { return _lastLocalDataUpdate; }
        }

        private static HashSet<Vector3> _profilePositionCache = new HashSet<Vector3>();

        public static HashSet<Vector3> ProfilePositionCache
        {
            get { return _profilePositionCache; }
            set { _profilePositionCache = value; }
        }

        private static int _currentInteractAttempts;

        /// <summary>
        /// Returns the Composite behavior of this tag
        /// </summary>
        /// <returns></returns>
        protected override Composite CreateBehavior()
        {
            return
                new Decorator(ctx => ZetaDia.IsInGame && Me.IsValid,
                    new Sequence(
                        InitSequence(),
                        MainSequence()
                        )
                    );
        }

        private float GetDistanceToLeader()
        {
            return Vector3.Distance(Leader.Position, Me.Position);
        }

        private List<DiaPlayer> _diaPlayers = new List<DiaPlayer>();

        /// <summary>
        /// Gets the leader from the RActorList with known data
        /// </summary>
        /// <returns></returns>
        private Composite LocalDataUpdateSequence()
        {
            return new Action(ret =>
                {
                    ServiceBase.Communicate();
                    Me.Update();

                    if (LeaderPortal != null)
                        LeaderPortal.Update();

                    bool readyForUpdate = DateTime.UtcNow.Subtract(LastLocalDataUpdate).TotalMilliseconds > Settings.Instance.UpdateInterval;
                    if (ZetaDia.IsInTown)
                        GetLeaderIndex();
                    if (readyForUpdate || _diaLeader == null || (_diaLeader != null && !_diaLeader.IsValid))
                    {
                        _diaPlayers = ZetaDia.Actors.GetActorsOfType<DiaPlayer>().Where(ActorIsLeader).ToList();

                        if (_diaPlayers != null && _diaPlayers.Count > 0)
                            _diaLeader = _diaPlayers.FirstOrDefault();

                        _headstones = ZetaDia.Actors.GetActorsOfType<DiaGizmo>().Where(h => h.ActorSNO == 4860).OrderBy(h => h.Distance);
                    }
                }
            );
        }

        private bool ActorIsLeader(DiaPlayer p)
        {
            if (p == null)
                return false;
            if (!p.IsValid)
                return false;
            if (!p.IsACDBased)
                return false;
            if (p.CommonData == null)
                return false;

            double hitPointsmaxTotal;
            int actorId;
            try
            {
                hitPointsmaxTotal = p.HitpointsMaxTotal;
                actorId = p.ActorSNO;
            }
            catch
            {
                return false;
            }

            return hitPointsmaxTotal == Leader.HitpointsMaxTotal && actorId == Leader.ActorSNO;
        }

        /// <summary>
        /// Make sure the navigation grid is reset
        /// </summary>
        /// <returns></returns>
        private RunStatus CheckResetWorldGrid()
        {
            if (!ZetaDia.IsInGame)
                return RunStatus.Success;
            if (ZetaDia.IsLoadingWorld)
                return RunStatus.Success;
            if (ZetaDia.Me == null)
                return RunStatus.Success;
            if (!ZetaDia.Me.IsValid)
                return RunStatus.Success;

            int currentWorldId = Me.CurrentWorldId;

            if (currentWorldId == _lastWorldId)
                return RunStatus.Success;

            _lastWorldId = currentWorldId;
            Logr.Log("Resetting Grid and clearing Navigator");
            GridSegmentation.Reset();
            Navigator.Clear();
            return RunStatus.Success;
        }

        /// <summary>
        /// The main following logic
        /// </summary>
        /// <returns>Zeta.TreeSharp.Composite.</returns>
        private Composite MainSequence()
        {
            return
                new PrioritySelector(
                    new Decorator(ret => !ZetaDia.Service.Platform.IsConnected,
                        new Action(ret => RunStatus.Failure)
                        ),
                    new Decorator(ret => !SimpleFollow.Enabled,
                        new Sequence(
                            new Action(ret => Logr.Log("ERROR: Plugin is not enabled!")),
                            new Action(ret => ForceEnablePlugin())
                            )
                        ),
                    new Decorator(ret => SimpleFollow.Enabled,
                        new Sequence(
                            new Action(ret => CheckResetWorldGrid()),
                            LocalDataUpdateSequence(),
                            new Action(ret => FollowerService.AsyncClientUpdate()),
                            new PrioritySelector(
                                new Decorator(ret => Leader.LastTimeUpdated == DateTime.MinValue, // generic message, don't do anything
                                    new Action(ret => RunStatus.Success)
                                    ),
                                new Decorator(ret => ZetaDia.IsLoadingWorld || Leader.IsLoadingWorld, // Changing worlds, wait a second :)
                                    new Action(ret => RunStatus.Success)
                                    ),
                                new Decorator(ret => Social.IsPartyleader,
                                    new Sequence(
                                        new Action(ret => Logr.Log("We're party leader - leaving game and party!")),
                                        CommonBehaviors.LeaveGame(retriever => "Follower is Party Leader")
                                        )
                                    ),
                                new ActionRunCoroutine(ret => LeaveGameLeaderNotInGame()),
                                UsePortalToBastionsKeepFromArmory(),
                                UsePortalOutOfCaldeumBazaar(),
                                new Decorator(ret => Leader.IsVendoring && ShouldTownRun,
                                    new PrioritySelector(
                                        new Decorator(ret => Me.IsInTown,
                                            new Action(ret => BrainBehavior.ForceTownrun("Leader is doing townrun, lets join... "))
                                            ),
                                        UseTownPortal("for TownRun")
                                        )
                                    ),
                                new Decorator(ret => Leader.IsVendoring && !ShouldTownRun,
                                    new Action(ret => Logr.Log("Leader is doing townrun, we don't need one. Lets wait... "))
                                    ),
                                new Decorator(ret => Leader.IsTakingPortalBack && Me.IsInTown && IsTownPortalNearby,
                                    new Sequence(
                                        new Action(ret => Logr.Log("Taking town portal back")),
                                        CommonBehaviors.TakeTownPortalBack(true),
                                        new Action(ret => GameEvents.FireWorldTransferStart()),
                                        new Sleep(500)
                                        )
                                    ),
                                new Decorator(ret => ZetaDia.WorldType == Act.OpenWorld && Me.IsInTown && ZetaDia.CurrentLevelAreaId != 270011,
                                    new Sequence(
                                        new Action(ret => Logr.Log("Using Waypoint to A5 Hub (Adventure Mode!)")),
                                        new Action(ret => ZetaDia.Me.UseWaypoint(49)),
                                        new PrioritySelector(
                                            new Decorator(ret => Me.IsInTown,
                                                new Sleep(500)
                                                ),
                                            new Sleep(3000)
                                            )
                                        )
                                    ),
                                FollowerOpenRift.OpenRiftBehavior(),
                                FollowerTakeWaypoint.TakeWaypointBehavior(),
                                new Decorator(ret => Leader.IsInTown && Me.IsInTown,
                                    new Action(ret => Logr.Log("Leader is in town, waiting... "))
                                    ),
                                new Decorator(ret => Leader.IsInTown && !Me.IsInTown,
                                    UseTownPortal("Leader is in Town, We are not")
                                    ),
                                new Decorator(ret => Leader.IsLoadingWorld,
                                    new Action(ret => Logr.Log("Leader is loading world... waiting"))
                                    ),
                                new ActionRunCoroutine(ret => FollowPlayerThroughPortalTask()),
                                new Decorator(ret => IsParticipatingInTieredLootRun && NoRecentInteraction() && Leader.WorldId != Me.CurrentWorldId && !Me.IsInTown,
                                    new ActionRunCoroutine(ret => FindDungeonExit())
                                    ),
                                new Decorator(ret => !IsParticipatingInTieredLootRun,
                                    new PrioritySelector(
                                        InTownTeleportLeader(),
                                        new Decorator(ret => NoRecentInteraction() && Leader.WorldId != Me.CurrentWorldId && !Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader has different WorldID"))
                                            ),
                                        new Decorator(ret => NoRecentInteraction() && Leader.LevelAreaId != Me.CurrentLevelAreaId && GetDistanceToLeader() > 300 && !Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader Level Area Id is different and distance is > 250"))
                                            ),
                                        new Decorator(ret => NoRecentInteraction() && Leader.Position.Distance(Me.Position) > 300 && !Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader distance is over 300"))
                                            ),
                                        new Decorator(ret => NoRecentInteraction() && Leader.WorldId != Me.CurrentWorldId && Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader has different WorldId"))
                                            ),
                                        new Decorator(ret => NoRecentInteraction() && Leader.LevelAreaId != Me.CurrentLevelAreaId && GetDistanceToLeader() > 300 && Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader Level Area Id is different and distance is > 250"))
                                            ),
                                        new Decorator(ret => NoRecentInteraction() && Leader.Position.Distance(Me.Position) > 300 && Me.IsInTown,
                                            new ActionRunCoroutine(ret => TeleportToLeader("Leader distance is over 300"))
                                            ),
                                        new Decorator(ret => Navigator.StuckHandler.IsStuck,
                                            new ActionRunCoroutine(ret => TeleportToLeader("We're stuck!"))
                                            )
                                        )
                                    ),
                                new Decorator(ret => IsParticipatingInTieredLootRun && Me.IsInTown,
                                    CommonBehaviors.TakeTownPortalBack(true)
                                ),
                                new ActionRunCoroutine(ret => CompleteGreaterRift()),
                                new Decorator(ctx => Leader.WorldId == Me.CurrentWorldId,
                                    FollowLeaderPrioritySelector()
                                    ),
                                new Action(ret => Logr.Log("Unexpected error 1 - could not follow."))
                                )
                            )
                        )
                    );
        }

        private async Task<bool> FindDungeonExit()
        {
            var exitMarker = ZetaDia.Minimap.Markers.AllMarkers.Where(m => m.IsPortalExit).OrderBy(m => m.Position.Distance2D(Me.Position)).FirstOrDefault();

            int questStepId = 1;
            var quest = ZetaDia.ActInfo.ActiveQuests.FirstOrDefault(q => q.QuestSNO == (int)SNOQuest.X1_LR_DungeonFinder);
            if (quest != null)
                questStepId = quest.QuestStep;

            if (exitMarker != null)
            {
                if (exitMarker.Position.Distance2D(Me.Position) > 300)
                {
                    Logr.Debug("Queueing Explore tag for Dungeon Exit");
                    BotBehaviorQueue.Queue(new ExploreDungeonTag
                    {
                        QuestId = (int)SNOQuest.X1_LR_DungeonFinder,
                        StepId = questStepId,
                        EndType = ExploreDungeonTag.ExploreEndType.RiftComplete,
                        PathPrecision = 30,
                        BoxSize = 45,
                        BoxTolerance = 0.01f,
                        MarkerDistance = 30,
                        PriorityScenes = new List<ExploreDungeonTag.PrioritizeScene> { new ExploreDungeonTag.PrioritizeScene { SceneName = "Exit" } }
                    }, "ExploreDungeon for Dungeon exit");
                }
                else
                {
                    Logr.Debug("Queueing MoveToMapmarker for Dungeon Exit");
                    BotBehaviorQueue.Queue(new MoveToMapMarkerTag
                    {
                        QuestId = (int)SNOQuest.X1_LR_DungeonFinder,
                        StepId = questStepId,
                        IsPortal = true,
                        DestinationWorldId = -1
                    }, "MoveToMapMarker for Dungeon Exit");
                }
                return true;
            }
            else
            {
                Logr.Debug("Queueing MoveToMapmarker for Dungeon Exit");
                BotBehaviorQueue.Queue(new MoveToMapMarkerTag
                {
                    QuestId = (int)SNOQuest.X1_LR_DungeonFinder,
                    StepId = questStepId,
                    IsPortal = true,
                    DestinationWorldId = -1
                }, "MoveToMapMarker for Dungeon Exit");
            }
            return false;
        }

        private async Task<bool> CompleteGreaterRift()
        {
            if (ZetaDia.IsInTown)
                return false;
            if (!IsActiveQuestAndStep(337492, 34))
                return false;
            if (!ZetaDia.Me.IsParticipatingInTieredLootRun)
                return false;

            string worldName = Enum.GetName(typeof(SNOWorld), ZetaDia.CurrentWorldId);

            if (worldName != null && !worldName.StartsWith("X1_LR_"))
                return false;

            if (ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Any(u => u.IsValid && u.ActorSNO == 398682 && !CompleteGreaterRiftTag.VendorDialog.IsVisible))
            {
                BotBehaviorQueue.Queue(new MoveToActorTag
                {
                    QuestId = 337492,
                    StepId = 34,
                    ActorId = 398682,
                    InteractRange = 9,
                    ExitWithVendorWindow = true,
                    ExitWithConversation = true
                }, "MoveToActor - Urshi");
                return true;
            }
            if (CompleteGreaterRiftTag.VendorDialog.IsVisible)
            {
                BotBehaviorQueue.Queue(new CompleteGreaterRiftTag
                {
                    QuestId = 337492,
                    StepId = 34
                }, "CompleteGreaterRift");
                return true;
            }
            return false;
        }

        private bool IsActiveQuestAndStep(int questId, int stepId)
        {
            return ZetaDia.ActInfo.ActiveQuests.Any(q => q.QuestSNO == questId && q.QuestStep == stepId);
        }

        private bool NoRecentInteraction()
        {
            return DateTime.UtcNow.Subtract(LastInteractTime).TotalMilliseconds > 10000;
        }

        private DateTime _lastTeleportAttempt = DateTime.MinValue;

        private async Task<bool> TeleportToLeader(string reason = "")
        {
            if (Player.IsGreaterRiftStarted)
                return false;
            if (ZetaDia.Me.LoopingAnimationEndTime > 0)
                return false;
            if (Leader.IsInGreaterRift)
                return false;
            if (IsParticipatingInTieredLootRun)
                return false;
            if (ZetaDia.Me.IsInCombat)
                return false;

            int leaderIndex = GetLeaderIndex();
            if (leaderIndex == -1)
                return await UseTownPortalTask("Invalid leader index");
            if (DateTime.UtcNow.Subtract(_lastTeleportAttempt).TotalMilliseconds > 6250)
            {
                Logr.Log("Teleporting to leader: {0}, {1}", GetLeaderIndex(), reason);
                GameEvents.FireWorldTransferStart();
                await Coroutine.Sleep(250);
                ZetaDia.Me.TeleportToPlayerByIndex(leaderIndex);
                _lastTeleportAttempt = DateTime.UtcNow;
                if (ZetaDia.IsInTown)
                    await Coroutine.Sleep(500);
                else
                    await Coroutine.Sleep(6250);
                return true;
            }
            return false;
        }

        private int _lastLeaderIndex = -1;
        private readonly HashSet<int> _knownFalseIndexes = new HashSet<int>();

        private readonly Random _rand = new Random((int)DateTime.UtcNow.Ticks);

        private int GetLeaderIndex()
        {
            if (SimpleFollow.Leader.CPlayerIndex != -1)
                return SimpleFollow.Leader.CPlayerIndex;

            if (_lastLeaderIndex != -1)
                return _lastLeaderIndex;

            if (Social.NumPartyMembers == 0)
                return 0;

            if (!ZetaDia.IsInTown)
                return _rand.Next(0, Social.NumPartyMembers);

            GizmoBanner bestBanner = null;
            if (ZetaDia.IsInTown)
            {
                List<GizmoBanner> gizmoBanners = ZetaDia.Actors.GetActorsOfType<GizmoBanner>()
                    .Where(b => b.IsValid && b.IsBannerUsable && b.BannerPlayer.ACDGuid != Me.ACDGuid && !_knownFalseIndexes.Contains(b.BannerPlayerIndex)).ToList();
                if (gizmoBanners.Any())
                {
                    gizmoBanners = gizmoBanners
                        .OrderByDescending(b => b.IsBannerPlayerInCombat).ToList();

                    bestBanner = gizmoBanners.FirstOrDefault();
                }
            }
            if (bestBanner == null)
                return _rand.Next(0, Social.NumPartyMembers);

            _lastLeaderIndex = bestBanner.BannerPlayerIndex;
            return _lastLeaderIndex;
            // Failsaife.. I guess
        }

        private bool IsTownPortalNearby
        {
            get { return ZetaDia.Actors.GetActorsOfType<DiaObject>(true).Any(o => o.ActorSNO == 191492); }
        }

        private Decorator UsePortalOutOfCaldeumBazaar()
        {
            return new
                Decorator(ret => Me.CurrentLevelAreaId == 55313 && Leader.LevelAreaId != Me.CurrentLevelAreaId, // Caldeum Bazaar hax
                    UseTownPortal("In Caldeum Bazaar or The Armory")
                );
        }

        private Decorator UsePortalToBastionsKeepFromArmory()
        {
            return new
                Decorator(ret => Me.CurrentLevelAreaId == 185228 && Leader.LevelAreaId != Me.CurrentLevelAreaId,
                    new Sequence(
                        CommonBehaviors.MoveAndStop(ret => new Vector3(351.2602f, 278.7404f, 10.1f), 5f, true, "Portal to Bastions Keep Stronghold"),
                        new Action(ret => ZetaDia.Actors.GetActorsOfType<GizmoPortal>(true).OrderBy(p => p.Distance).FirstOrDefault().Interact())
                        )
                );
        }

        private Composite UseHeadstone()
        {
            return new DecoratorContinue(ret => _headstones.Any(),
                new PrioritySelector(
                    new Decorator(ret => _headstones.FirstOrDefault().Distance > 10f,
                        new Action(ret => NavigateMove(_headstones.FirstOrDefault().Position))
                        ),
                    new Action(ret => _headstones.FirstOrDefault().Interact())
                    )
                );
        }

        private MoveResult _lastMoveResult;

        /// <summary>
        /// The main follower logic, responsible for moving to the leader when nearby
        /// </summary>
        /// <returns></returns>
        private Composite FollowLeaderPrioritySelector()
        {
            return
                new PrioritySelector(
                    new Decorator(ret => _lastMoveResult == MoveResult.ReachedDestination && Leader.Position.Distance2D(Me.Position) > 45f,
                        new ActionRunCoroutine(ret => TeleportToLeader("Reached Destination"))
                        ),
                    new Decorator(ret => ShouldUseProfilePosition && Leader.Position.Distance2D(Me.Position) <= 45f,
                        new Sequence(
                            SetLeaderLastSeen(),
                            new DecoratorContinue(ret => Leader.ProfilePosition.Distance2D(Me.Position) <= Leader.ProfilePathPrecision,
                                new Sequence(
                                    new Action(ret => Logr.Log("Successfully Moved to to Profile Coordinates from Leader (Profile)")),
                                    new Action(ret => ProfilePositionCache.Add(Leader.ProfilePosition))
                                    )
                                ),
                            new DecoratorContinue(ret => Leader.ProfilePosition.Distance2D(Me.Position) > Leader.ProfilePathPrecision,
                                new Action(ret => _lastMoveResult = NavigateMove(Leader.ProfilePosition, "Moving to Profile Coordinates from Leader (Profile)"))
                                )
                            )
                        ),
                    new Decorator(ret => _diaLeader != null && _diaLeader.IsValid,
                        new Sequence(
                            SetLeaderLastSeen(),
                            new DecoratorContinue(ret => _diaLeader.Position.Distance(Me.Position) > Distance,
                                new Action(ret => _lastMoveResult = NavigateMove(_diaLeader.Position, "Following Leader (Actor)"))
                                )
                            )
                        ),
                    new Sequence(
                        SetLeaderLastSeen(),
                        new DecoratorContinue(ret => Leader.Position.Distance(Me.Position) > Distance,
                            new Action(ret => _lastMoveResult = NavigateMove(Leader.Position, "Following Leader (WCF)"))
                            ),
                        UseHeadstone()
                        ),
                    new Action(ret => Logr.Log("Unexpected error 2 - could not follow"))
                    );
        }

        private Composite SetLeaderLastSeen()
        {
            return new
                DecoratorContinue(ret => Leader.LevelAreaId == Me.CurrentLevelAreaId,
                    new Sequence(
                        new Action(ret => _currentInteractAttempts = 0),
                        new Action(ret => _playerLastSeenPos = Leader.Position),
                        new Action(ret => _playerLastSeenTime = DateTime.UtcNow)
                        )
                );
        }

        private Vector3 lastDestination = Vector3.Zero;



        private MoveResult NavigateMove(Vector3 destination, string name = "", bool raycast = true)
        {
            try
            {
                if (lastDestination != destination)
                {
                    lastDestination = destination;
                    //Navigator.Clear();
                }
                const float pathPointLimit = 250;
                if (!ZetaDia.IsInTown && destination.Distance2D(Me.Position) > pathPointLimit)
                    destination = MathEx.CalculatePointFrom(Me.Position, destination, destination.Distance2D(Me.Position) - pathPointLimit);

                if (string.IsNullOrEmpty(name))
                {
                    name = destination.ToString();
                }
                //Logr.Debug(("Moving to {0}, distance: {1:0}", name, destination.Distance2D(ZetaDia.Me.Position));

                return NavHelper.NavigateTo(destination, name);
            }
            catch (Exception ex)
            {
                Logr.Log("DB Navigation Exception: {0}", ex);
                GridSegmentation.Reset();
                Navigator.Clear();

                return MoveResult.Failed;
            }
        }

        private int leaderNotInGameCount = 0;

        private async Task<bool> LeaveGameLeaderNotInGame()
        {
            var lastJoinedGame = DateTime.Now.Subtract(SimpleFollow.LastJoinedGame).TotalMilliseconds;
            if (SimpleFollow.LastJoinedGame != DateTime.MinValue && lastJoinedGame > 0 && lastJoinedGame < 10000)
                return false;
            var lastinteract = DateTime.Now.Subtract(LastInteractTime).TotalMilliseconds;
            if (LastInteractTime != DateTime.MinValue && lastinteract > 0 && lastinteract < 10000)
                return false;
            if (Leader.IsLeavingGame || !Leader.IsInGame || !Leader.IsInSameGame)
            {
                leaderNotInGameCount++;
            }

            if (leaderNotInGameCount > 10)
            {
                leaderNotInGameCount = 0;
                Logr.Log("Leader is not in game, exiting (Recent Interact={0:0}, IsLeavingGame={1} !IsInGame={2} !IsInSameGame={3})",
                    DateTime.UtcNow.Subtract(LastInteractTime).TotalMilliseconds < 10000, Leader.IsLeavingGame, !Leader.IsInGame, !Leader.IsInSameGame);
                await SafeLeaveGame();
                return true;
            }


            return false;
        }

        private static async Task<bool> SafeLeaveGame()
        {
            if (!ZetaDia.IsInGame)
                return true;

            if (ZetaDia.IsLoadingWorld)
                return true;

            if (GameUI.LeaveGameButton.IsVisible)
                return true;

            // Leave party if we're the leader (we should be a follower)
            ZetaDia.Service.Party.LeaveGame(Settings.Instance.StayInParty);

            return true;
        }

        private Composite InTownTeleportLeader()
        {
            return
                new Decorator(ctx => !Leader.IsTakingPortalBack && !Leader.IsInTown && (Me.IsInTown && Me.CurrentLevelAreaId != 55313), // A2 Caldeum Bazaar
                    new Sequence(
                        new Action(ret => ProfilePositionCache.Clear()),
                        new ActionRunCoroutine(ret => TeleportToLeader("Leader is not in town"))
                        )
                    );
        }

        private async Task<bool> FollowPlayerThroughPortalTask()
        {
            const int a5HubHash = 1877684886;

            if (Me.IsInTown)
                return false;

            var exitMarker = ZetaDia.Minimap.Markers.CurrentWorldMarkers.Where(m => m.IsPortalExit && m.NameHash != a5HubHash).OrderBy(m => m.Position.Distance2DSqr(Me.Position)).FirstOrDefault();

            // Leader is in the same world, levelarea, or is on the object manager
            if ((Leader.WorldId == Me.CurrentWorldId || Leader.LevelAreaId == Me.CurrentLevelAreaId || (_diaLeader != null && _diaLeader.IsValid)))
                return false;

            // There is no exit portal and the player did not take a known portal
            if (IsParticipatingInTieredLootRun && exitMarker == null && !PlayerTookPortal())
                return false;

            // Not in Rift, player did not take portal
            if (!IsParticipatingInTieredLootRun && !PlayerTookPortal())
                return false;

            // LevelArea: A1_trOut_TristramWilderness, Id: 19954 - Cemetery of the Foresaken (no way to know which portal leader actually took)
            if (Me.CurrentLevelAreaId == 19954)
                return false;

            if (!IsParticipatingInTieredLootRun && PlayerTookPortal() && LeaderPortal.IsValid && (LeaderPortal.Distance - LeaderPortal.Radius) > 2f)
            {
                Logr.Log("Moving to Regular Portal {0} at {1}", LeaderPortal.Name, LeaderPortal.Position);
                NavigateMove(LeaderPortal.Position, LeaderPortal.Name);
                return true;
            }

            if (LeaderPortal != null && exitMarker != null && exitMarker.Position.Distance(LeaderPortal.Position) < 2f && LeaderPortal.IsValid && (LeaderPortal.Distance - LeaderPortal.Radius) > 2f)
            {
                Logr.Log("Moving to Exit portal {0} at {1}", LeaderPortal.Name, LeaderPortal.Position);
                NavigateMove(LeaderPortal.Position, LeaderPortal.Name);
                return true;
            }

            if (GameUI.ElementIsVisible(GameUI.GenericOK))
            {
                GameUI.SafeClick(GameUI.GenericOK, ClickDelay.NoDelay, "Generic OK Button", 0, true);
                await Coroutine.Sleep(3000);
                return true;
            }

            if (GameUI.ElementIsVisible(UIElements.ConfirmationDialogOkButton))
            {
                GameUI.SafeClick(UIElements.ConfirmationDialogOkButton, ClickDelay.NoDelay, "ConfirmationDialogOkButton", 0, true);
                await Coroutine.Sleep(3000);
                return true;
            }

            while (ZetaDia.IsInGame && !ZetaDia.IsLoadingWorld && ZetaDia.Me != null && ZetaDia.Me.IsValid && ZetaDia.Me.LoopingAnimationEndTime > 0)
            {
                await Coroutine.Sleep(50);
                await Coroutine.Yield();
            }

            if (exitMarker != null && exitMarker.IsValid && exitMarker.Position.Distance2DSqr(Me.Position) > (20f * 20f))
            {
                Logr.Log("Moving to exit marker {0} at {1}", exitMarker.NameHash, exitMarker.Position, exitMarker.Position.Distance2D(Me.Position));
                switch (NavigateMove(exitMarker.Position))
                {
                    case MoveResult.Moved:
                        return true;
                    default:
                        return false;
                }
            }

            if (LeaderPortal != null && LeaderPortal.IsReturnPortal && ZetaDia.Me.LoopingAnimationEndTime == 0)
            {
                LeaderPortal.Interact();
                await Coroutine.Sleep(250);
                return true;
            }

            if (LeaderPortal != null && (LeaderPortal.Distance <= 20f || LeaderPortal.RadiusDistance <= 10f) && LeaderPortal.IsValid && ZetaDia.Me.LoopingAnimationEndTime == 0)
            {
                Logr.Log("Interacting with portal {0} at {1}", LeaderPortal.Name, LeaderPortal.Position, LeaderPortal.Distance);
                await InteractSequence().ExecuteCoroutine();
                return true;
            }

            return false;
        }

        private Composite FollowPlayerThruPortal()
        {
            return
                new Decorator(ret => (Leader.WorldId != Me.CurrentWorldId || Leader.LevelAreaId != Me.CurrentLevelAreaId || (_diaLeader != null && !_diaLeader.IsValid)) && PlayerTookPortal(),
                    new Sequence(
                        new DecoratorContinue(ctx => LeaderPortal.IsValid && (LeaderPortal.Distance - LeaderPortal.Radius) > 2f,
                            new Sequence(
                                new Action(ret => Logr.Log("Moving to portal {0} at {1} distance: {2:0}", LeaderPortal.Name, LeaderPortal.Position, LeaderPortal.Distance)),
                                new Action(ret => NavigateMove(LeaderPortal.Position, LeaderPortal.Name))
                                )
                            ),
                        new PrioritySelector(
                            new Decorator(ret => GameUI.ElementIsVisible(GameUI.GenericOK),
                                new Sequence(
                                    new Action(ret => GameUI.SafeClick(GameUI.GenericOK, ClickDelay.NoDelay, "Generic OK Button", 0, true)),
                                    new Sleep(3000)
                                    )
                                ),
                            new Decorator(ret => GameUI.ElementIsVisible(UIElements.ConfirmationDialogOkButton),
                                new Sequence(
                                    new Action(ret => GameUI.SafeClick(UIElements.ConfirmationDialogOkButton, ClickDelay.NoDelay, "ConfirmationDialogOkButton", 0, true)),
                                    new Sleep(3000)
                                    )
                                ),
                            new Decorator(ret => ZetaDia.Me.LoopingAnimationEndTime > 0,
                                new Sleep(250)
                                ),
                            new Decorator(ret => LeaderPortal.IsReturnPortal && ZetaDia.Me.LoopingAnimationEndTime == 0,
                                new Sequence(
                                    new Action(ret => LeaderPortal.Interact()),
                                    new Sleep(250)
                                    )
                                ),
                            new DecoratorContinue(ctx => (LeaderPortal.Distance <= 20f || LeaderPortal.RadiusDistance <= 10f) && LeaderPortal.IsValid && ZetaDia.Me.LoopingAnimationEndTime == 0,
                                new Sequence(
                                    new Action(ret => Logr.Log("Interacting with portal {0} at {1} distance: {2:0}", LeaderPortal.Name, LeaderPortal.Position, LeaderPortal.Distance)),
                                    InteractSequence()
                                    )
                                )
                            )
                        )
                    );
        }

        /// <summary>
        /// Interacts with a portal after the leader went through it
        /// </summary>
        /// <returns></returns>
        private Composite InteractSequence()
        {
            return
                new Decorator(ret => ZetaDia.IsInGame && !ZetaDia.IsLoadingWorld && LeaderPortal != null && LeaderPortal.IsValid && Me != null && Me.IsValid,
                    new Sequence(
                        new WaitContinue(1, canRun => !ZetaDia.Me.Movement.IsMoving,
                            new Sleep(250)
                            ),
                        new Action(ret => Logr.Log("Interact attempt {0} with Gizmo: {1} {2} {3} ",
                            _currentInteractAttempts, LeaderPortal.ActorSNO, LeaderPortal.Name, LeaderPortal.Position)),
                        new Action(ret => GameEvents.FireWorldTransferStart()),
                        new PrioritySelector(
                            new Decorator(ret => LeaderPortal.IsMonster,
                                new Action(ret => Me.UsePower(SNOPower.Axe_Operate_NPC, LeaderPortal.Position))
                                ),
                            new Action(ret => LeaderPortal.Interact())
                            ),
                        new Sleep(250),
                        new Action(ret => _currentInteractAttempts++),
                        new Action(ret => LastInteractTime = DateTime.UtcNow)
                        )
                    );
        }

        /// <summary>
        /// Initializes FollowTag
        /// </summary>
        /// <returns></returns>
        private Composite InitSequence()
        {
            return new DecoratorContinue(ctx => !_initialized,
                new Action(ret =>
                {
                    Logr.Log("Disabling Inactivity Timer");
                    Zeta.Bot.Settings.GlobalSettings.Instance.LogoutInactivityTime = 0;
                    if (Distance == 0)
                        Distance = 5f;

                    _initialized = true;
                })
                );
        }

        private bool PlayerTookPortal()
        {
            return ZetaDia.Actors.GetActorsOfType<GizmoPortal>(true)
                .Any(p => p.IsValid && p.Position.Distance(_playerLastSeenPos) <= 30f) &&
                   CheckPlayerTookPortalTime();
        }

        private bool CheckPlayerTookPortalTime()
        {
            return DateTime.UtcNow.Subtract(_playerLastSeenTime).TotalMilliseconds < 10000;
        }

        private async Task<bool> UseTownPortalTask(string reason = "")
        {
            if (ZetaDia.Me.IsInCombat)
                return false;

            if (IsParticipatingInTieredLootRun)
                return false;

            if (Me.IsInTown)
                return false;

            if (DateTime.UtcNow.Subtract(SimpleFollow.LastChangedWorld).TotalMilliseconds <= 7000)
                return false;

            if (Player.LevelAreaId == 19947)
                await CommonCoroutines.MoveTo(new Vector3(2980.927f, 2833.887f, 24.89782f), "Town entrance");

            if (ZetaDia.Actors.GetActorsOfType<GizmoWaypoint>(true).Any())
                await MoveToAndUseWaypoint();

            if (ZetaDia.Me.LoopingAnimationEndTime > 0)
                return true;

            if (ZetaDia.Me.LoopingAnimationEndTime == 0)
            {
                Logr.Log("Teleporting to player: {0}", reason);
                GameUI.SafeClick(UIElements.BackgroundScreenPCButtonRecall, ClickDelay.NoDelay, "Town Portal Button", 0, true);
                Coroutine.Sleep(50);
                return true;
            }

            return false;
        }

        private Composite UseTownPortal(string reason = "")
        {
            return
                new PrioritySelector(
                    new Decorator(ret => IsParticipatingInTieredLootRun,
                        new Action(ret => RunStatus.Failure)
                        ),
                    new Decorator(ret => Me.IsInTown,
                        new Action(ret => RunStatus.Failure)
                        ),
                    new Decorator(ret => DateTime.UtcNow.Subtract(LastInteractTime).TotalMilliseconds <= 2000,
                        new Action()
                        ),
                    new Decorator(ret => DateTime.UtcNow.Subtract(SimpleFollow.LastChangedWorld).TotalMilliseconds <= 7000,
                        new Action()
                        ),
                    new Decorator(ret => Player.LevelAreaId == 19947, // A1 quest 1 start area (can't use town portal here, need to run...)
                        new Action(ret => NavigateMove(new Vector3(2980.927f, 2833.887f, 24.89782f)))
                        ),
                //new Decorator(ret => ZetaDia.Actors.GetActorsOfType<GizmoWaypoint>(true).Any(),
                //    new Action(ret => MoveToAndUseWaypoint())
                //    ),
                    new Decorator(ret => ZetaDia.Me.LoopingAnimationEndTime > 0,
                        new Action(ret => RunStatus.Success)
                        ),
                    new Decorator(ret => ZetaDia.Me.LoopingAnimationEndTime == 0,
                        new Sequence(
                            new Action(ret => Logr.Log("Teleporting to player: {0}", reason)),
                            new Sequence(
                                new Action(ret => GameUI.SafeClick(UIElements.BackgroundScreenPCButtonRecall, ClickDelay.NoDelay, "Town Portal Button", 0, true)),
                                new Sleep(6250)
                                )
                            )
                        )
                    );
        }

        private async Task<bool> MoveToAndUseWaypoint()
        {
            var waypoint = ZetaDia.Actors.GetActorsOfType<GizmoWaypoint>(true).FirstOrDefault();

            if (waypoint.Distance >= 10f)
            {
                Logr.Log("Waypoint is nearby, using waypoint instead of Town Portal");
                NavigateMove(waypoint.Position);
                return true;
            }
            if (!GameUI.ElementIsVisible(UIElements.WaypointMap))
            {
                Logr.Log("Opening waypoint window");
                waypoint.Interact();
                await Coroutine.Sleep(50);
                return true;
            }
            if (GameUI.ElementIsVisible(UIElements.WaypointMap))
            {
                Logr.Log("Waypoint Town button is visible, clicking");
                GameEvents.FireWorldTransferStart();
                waypoint.UseWaypoint(0);
                await Coroutine.Sleep(50);
                return true;
            }
            Logr.Log("Unknown error in using Waypoint");
            return true;
        }

        private Vector3 GetStashLocation()
        {
            switch (ZetaDia.CurrentAct)
            {
                case Act.A1:
                    return new Vector3(2965.15f, 2834.933f, 23.95f);
                case Act.A2:
                    return new Vector3(326, 274, 0);
                case Act.A3:
                case Act.A4:
                    return new Vector3(442, 325, 0);
                default:
                    return Vector3.Zero;
            }
        }

        public bool ForceEnablePlugin()
        {
            try
            {
                var plugin = PluginManager.Plugins.FirstOrDefault(p => p.Plugin.Name == "SimpleFollow");
                if (plugin != null)
                    plugin.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                Logr.Log("Could not force enable SimpleFollow plugin");
                Logr.Log(ex.ToString());
                return false;
            }
        }

        private bool GetIsParticipatingInTieredLootRun()
        {
            bool isInLootRun = ZetaDia.Me.IsParticipatingInTieredLootRun;
            bool urshiQuestStep = ZetaDia.ActInfo.AllQuests.Any(q => q.QuestSNO == 337492 && (q.QuestStep == 34 || q.QuestStep == 10) && q.State == QuestState.InProgress);
            return isInLootRun && !urshiQuestStep;
        }

        private bool _isDone;

        public override bool IsDone
        {
            get { return _isDone; }
        }

        public override void ResetCachedDone()
        {
            _isDone = false;
            base.ResetCachedDone();
        }

        public Message Leader
        {
            get { return SimpleFollow.Leader; }
        }

        public Player Me
        {
            get { return Player.Instance; }
        }

        public float FreeBagSlotsPercent
        {
            get
            {
                try
                {
                    if (ZetaDia.Me == null)
                        return 0;
                    if (ZetaDia.Me.Inventory == null)
                        return 0;
                    if (!ZetaDia.IsInGame)
                        return 0;
                    float result = ((float)ZetaDia.Me.Inventory.NumFreeBackpackSlots) / 50;
                    return result;
                }
                catch (Exception ex)
                {
                    Logr.Error("Error getting FreeBagSlotsPercent " + ex);
                    return 0;
                }
            }
        }


    }
}