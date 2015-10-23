﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventurer.Coroutines.CommonSubroutines;
using Adventurer.Game.Actors;
using Adventurer.Game.Combat;
using Adventurer.Game.Exploration;
using Adventurer.Game.Quests;
using Adventurer.Game.Rift;
using Adventurer.Game.Stats;
using Adventurer.Settings;
using Adventurer.Util;
using Buddy.Coroutines;
using Zeta.Bot;
using Zeta.Bot.Coroutines;
using Zeta.Bot.Logic;
using Zeta.Common;
using Zeta.Common.Helpers;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.Coroutines.RiftCoroutines
{

    public class RiftCoroutine : IDisposable
    {
        private RiftType _RiftType;
        private bool _runningNephalemInsteadOfGreaterRift;
        private int _level;
        private States _state;
        private bool _townRunInitiated;
        private int _portalScanRange = 150;

        private int _currentWorldDynamicId;
        private int _previusWorldDynamicId;
        private int _prePortalWorldDynamicId;
        private int _nextLevelPortalSNO;

        private bool _possiblyCowLevel;
        private bool _holyCowEventCompleted;

        private Vector3 _bossLocation = Vector3.Zero;
        private Vector3 _holyCowLocation = Vector3.Zero;
        private Vector3 _urshiLocation = Vector3.Zero;
        private Vector3 _nextLevelPortalLocation = Vector3.Zero;
        private DateTime _riftStartTime;
        private DateTime _riftEndTime;
        private ExperienceTracker _experienceTracker = new ExperienceTracker();

        private readonly MoveToPositionCoroutine _moveToRiftStoneCoroutine = new MoveToPositionCoroutine(ExplorationData.ActHubWorldIds[Act.A1], RiftData.Act1RiftStonePosition);
        private readonly InteractionCoroutine _interactWithRiftStoneInteractionCoroutine = new InteractionCoroutine(RiftData.RiftStoneSNO, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1));
        private readonly InteractionCoroutine _interactWithUrshiCoroutine = new InteractionCoroutine(RiftData.UrshiSNO, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1), 3);

        private readonly MoveToPositionCoroutine _moveToOrekCoroutine = new MoveToPositionCoroutine(ExplorationData.ActHubWorldIds[Act.A1], RiftData.Act1OrekPosition);
        private readonly InteractionCoroutine _talkToOrekCoroutine = new InteractionCoroutine(RiftData.OrekSNO, TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(400), 3);
        private readonly InteractionCoroutine _talkToHolyCowCoroutine = new InteractionCoroutine(RiftData.HolyCowSNO, TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(1000), 3);

        #region State
        public enum States
        {
            NotStarted,
            GoingToAct1Hub,
            ReturningToTown,
            InTown,
            MoveToOrek,
            TalkToOrek,
            TownRun,
            WaitForRiftCountdown,
            MoveToRiftStone,
            OpeningRift,
            EnteringRift,
            EnteringGreaterRift,
            SearchingForExitPortal,
            MovingToExitPortal,
            EnteringExitPortal,
            OnNewRiftLevel,
            BossSpawned,
            SearchingForBoss,
            MovingToBoss,
            KillingBoss,
            UrshiSpawned,
            SearchingForUrshi,
            MovingToUrshi,
            InteractingWithUrshi,
            UpgradingGems,
            SearchingForTownstoneOrExitPortal,
            TownstoneFound,
            SearchingForHolyCow,
            MovingToHolyCow,
            InteractingWithHolyCow,
            Completed,
            Failed
        }


        public States State
        {
            get { return _state; }
            protected set
            {
                if (_state == value) return;
                if (value != States.NotStarted)
                {
                    Logger.Debug("[Rift] " + value);
                }
                _state = value;
            }
        }

        #endregion

        public RiftCoroutine(RiftType RiftType)
        {
            _RiftType = RiftType;
            if (RiftType == RiftType.Nephalem)
            {
                _level = -1;
            }
            else
            {
                _level = RiftData.GetGreaterRiftLevel();
            }
        }

        public virtual async Task<bool> GetCoroutine()
        {
            if (_isPulsing)
            {
                PulseChecks();
            }

            switch (State)
            {

                case States.NotStarted:
                    return NotStarted();
                case States.GoingToAct1Hub:
                    return await GoingToAct1Hub();
                case States.ReturningToTown:
                    return await ReturningToTown();
                case States.InTown:
                    return InTown();
                case States.MoveToOrek:
                    return await MoveToOrek();
                case States.TalkToOrek:
                    return await TalkToOrek();
                case States.TownRun:
                    return TownRun();
                case States.WaitForRiftCountdown:
                    return WaitForRiftCountdown();
                case States.MoveToRiftStone:
                    return await MoveToRiftStone();
                case States.OpeningRift:
                    return await OpeningRift();
                case States.EnteringRift:
                    return await EnteringRift();
                case States.EnteringGreaterRift:
                    return EnteringGreaterRift();
                case States.SearchingForExitPortal:
                    return await SearchingForExitPortal();
                case States.MovingToExitPortal:
                    return await MovingToExitPortal();
                case States.EnteringExitPortal:
                    return await EnteringExitPortal();
                case States.OnNewRiftLevel:
                    return OnNewRiftLevel();
                case States.BossSpawned:
                    return BossSpawned();
                case States.SearchingForBoss:
                    return await SearchingForBoss();
                case States.MovingToBoss:
                    return await MovingToBoss();
                case States.KillingBoss:
                    return KillingBoss();
                case States.UrshiSpawned:
                    return UrshiSpawned();
                case States.SearchingForUrshi:
                    return await SearchingForUrshi();
                case States.MovingToUrshi:
                    return await MovingToUrshi();
                case States.InteractingWithUrshi:
                    return await InteractingWithUrshi();
                case States.UpgradingGems:
                    return await UpgradingGems();
                case States.SearchingForTownstoneOrExitPortal:
                    return await SearchingForTownstoneOrExitPortal();
                case States.TownstoneFound:
                    return TownstoneFound();
                case States.SearchingForHolyCow:
                    return await SearchingForHolyCow();
                case States.MovingToHolyCow:
                    return await MovingToHolyCow();
                case States.InteractingWithHolyCow:
                    return await InteractingWithHolyCow();
                case States.Completed:
                    return Completed();
                case States.Failed:
                    return Failed();
            }
            return false;
        }

        private bool NotStarted()
        {
            if(!_experienceTracker.IsStarted) _experienceTracker.Start();
            SafeZerg.Instance.DisableZerg();
            if (_RiftType == RiftType.Greater)
            {
                _level = RiftData.GetGreaterRiftLevel();
            }
            if (_runningNephalemInsteadOfGreaterRift && AdvDia.StashAndBackpackItems.Any(i => i.IsValid && i.ActorSNO == RiftData.GreaterRiftKeySNO))
            {
                _level = RiftData.GetGreaterRiftLevel();
                _RiftType = RiftType.Greater;
                _runningNephalemInsteadOfGreaterRift = false;
                return false;
            }
            if (AdvDia.RiftQuest.State == QuestState.NotStarted && _RiftType == RiftType.Greater && !AdvDia.StashAndBackpackItems.Any(i => i.IsValid && i.ActorSNO == RiftData.GreaterRiftKeySNO))
            {
                if (PluginSettings.Current.GreaterRiftRunNephalem)
                {
                    _level = -1;
                    _RiftType = RiftType.Nephalem;
                    _runningNephalemInsteadOfGreaterRift = true;
                    return false;
                }
                else
                {
                    Logger.Error("You have no Greater Rift Keys. Stopping the bot.");
                    BotMain.Stop();
                    return true;
                }
            }
            _currentWorldDynamicId = AdvDia.CurrentWorldDynamicId;
            if (AdvDia.RiftQuest.State == QuestState.InProgress && RiftData.RiftWorldIds.Contains(AdvDia.CurrentWorldId))
            {
                State = States.SearchingForExitPortal;
                return false;
            }
            State = AdvDia.CurrentWorldId == ExplorationData.ActHubWorldIds[Act.A1] ? States.InTown : States.GoingToAct1Hub;
            if (AdvDia.RiftQuest.State == QuestState.NotStarted)
            {
                ScenesStorage.Reset();
                RiftData.EntryPortals.Clear();
                _currentWorldDynamicId = 0;
                _previusWorldDynamicId = 0;
                _bossLocation = Vector3.Zero;
                _nextLevelPortalLocation = Vector3.Zero;
                _holyCowLocation = Vector3.Zero;
                _holyCowEventCompleted = false;
                _possiblyCowLevel = false;
            }
            return false;
        }

        private WaitTimer _goingToAct1HubWaitTimer;
        private async Task<bool> GoingToAct1Hub()
        {
            DisablePulse();
            if (_goingToAct1HubWaitTimer == null)
            {
                _goingToAct1HubWaitTimer = new WaitTimer(TimeSpan.FromSeconds(5));
                _goingToAct1HubWaitTimer.Reset();
            }
            if (!_goingToAct1HubWaitTimer.IsFinished) return false;
            if (!await WaypointCoroutine.UseWaypoint(WaypointFactory.ActHubs[Act.A1])) return false;
            _goingToAct1HubWaitTimer = null;
            State = States.InTown;
            return false;
        }

        private WaitTimer _returningToTownbWaitTimer;
        private async Task<bool> ReturningToTown()
        {
            DisablePulse();
            if (_returningToTownbWaitTimer == null)
            {
                _returningToTownbWaitTimer = new WaitTimer(TimeSpan.FromSeconds(5));
                _returningToTownbWaitTimer.Reset();
            }
            if (!_returningToTownbWaitTimer.IsFinished) return false;
            if (!await TownPortalCoroutine.UseWaypoint()) return false;
            _returningToTownbWaitTimer = null;
            State = States.InTown;
            return false;
        }

        private bool InTown()
        {
            DisablePulse();
            if (AdvDia.CurrentWorldId != ExplorationData.ActHubWorldIds[Act.A1])
            {
                State = States.GoingToAct1Hub;
                return false;
            }
            if (AdvDia.RiftQuest.State == QuestState.Completed)
            {
                State = States.TownRun;
                return false;
            }
            switch (AdvDia.RiftQuest.Step)
            {
                case RiftStep.Cleared:
                    Logger.Info("[Rift] I think I should go and brag to Orek about my success.");
                    State = States.MoveToOrek;
                    return false;
                case RiftStep.BossSpawned:
                    Logger.Info("[Rift] I wonder why am I in town while the boss is spawned in the rift, I'm taking my chances with this portal.");
                    State = States.MoveToRiftStone;
                    return false;
                case RiftStep.UrshiSpawned:
                    Logger.Info("[Rift] I wonder why am I in town while Urshi spawned in the rift, I'm taking my chances with this portal.");
                    State = States.MoveToRiftStone;
                    return false;
                case RiftStep.KillingMobs:
                    Logger.Info("[Rift] I wonder why am I in town while there are many mobs to kill in the rift, I'm taking my chances with this portal.");
                    State = States.MoveToRiftStone;
                    return false;
                case RiftStep.NotStarted:
                    State = States.MoveToRiftStone;
                    _moveToRiftStoneCoroutine.Reset();
                    Logger.Info("[Rift] Time to kill some scary monsters. Chop chop!");
                    return false;
                default:
                    Logger.Info("[Rift] I really don't know what to do now.");
                    State = States.Failed;
                    return false;
            }
        }

        private async Task<bool> MoveToOrek()
        {
            DisablePulse();
            if (!await _moveToOrekCoroutine.GetCoroutine()) return false;
            _moveToOrekCoroutine.Reset();
            State = States.TalkToOrek;
            return false;
        }

        private async Task<bool> TalkToOrek()
        {
            DisablePulse();
            if (AdvDia.RiftQuest.State == QuestState.InProgress && AdvDia.RiftQuest.Step == RiftStep.Cleared)
            {
                if (!await _talkToOrekCoroutine.GetCoroutine()) return false;
                _talkToOrekCoroutine.Reset();
                if (AdvDia.RiftQuest.Step == RiftStep.Cleared)
                {
                    State = States.MoveToOrek;
                    return false;
                }
                await Coroutine.Sleep(1200);
                await Coroutine.Wait(TimeSpan.FromSeconds(5), () => !ZetaDia.Me.IsParticipatingInTieredLootRun);
                if (ZetaDia.Me.IsParticipatingInTieredLootRun)
                {
                    Logger.Info("[Rift] Oh well, I seem to think that the rift is still active, that means I'll not be able to clear out my packs properly, sorry in advance.");
                }
            }

            _experienceTracker.StopAndReport("Rift");
            _experienceTracker.Start();
            State = States.TownRun;
            return false;
        }

        private bool TownRun()
        {
            Logger.Debug("[TownRun] BrainBehavior.IsVendoring is {0}", BrainBehavior.IsVendoring);
            Logger.Debug("[TownRun] ZetaDia.Me.IsParticipatingInTieredLootRun is {0}", ZetaDia.Me.IsParticipatingInTieredLootRun);
            Logger.Debug("[TownRun] AdvDia.RiftQuest.State is {0}", AdvDia.RiftQuest.State);
            Logger.Debug("[TownRun] AdvDia.RiftQuest.Step is {0}", AdvDia.RiftQuest.Step);
            DisablePulse();
            if (BrainBehavior.IsVendoring)
            {
                return false;
            }
            if (!_townRunInitiated)
            {
                _townRunInitiated = true;
                BrainBehavior.ForceTownrun(" We need it.", true);
                return false;
            }
            _townRunInitiated = false;
            if (AdvDia.RiftQuest.Step == RiftStep.Completed)
            {
                State = States.WaitForRiftCountdown;
                Logger.Info("[Rift] Tick tock, tick tock...");
            }
            else
            {
                State = States.NotStarted;
                _moveToRiftStoneCoroutine.Reset();
            }
            return false;
        }

        private bool WaitForRiftCountdown()
        {
            DisablePulse();
            if (AdvDia.RiftQuest.State != QuestState.NotStarted)
            {
                return false;
            }
            State = States.NotStarted;
            return false;
        }

        private async Task<bool> MoveToRiftStone()
        {
            DisablePulse();
            if (!await _moveToRiftStoneCoroutine.GetCoroutine()) return false;
            _moveToRiftStoneCoroutine.Reset();
            if (IsRiftPortalOpen)
            {
                _prePortalWorldDynamicId = AdvDia.CurrentWorldDynamicId;
                State = States.EnteringRift;
            }
            else
            {
                State = States.OpeningRift;
            }
            return false;
        }

        private async Task<bool> OpeningRift()
        {
            if (RiftData.RiftWorldIds.Contains(AdvDia.CurrentWorldId))
            {
                State = States.OnNewRiftLevel;
                return false;
            }
            DisablePulse();
            if (!await _interactWithRiftStoneInteractionCoroutine.GetCoroutine()) return false;
            await Coroutine.Wait(2500, () => UIElements.RiftDialog.IsVisible);
            _interactWithRiftStoneInteractionCoroutine.Reset();
            if (!UIElements.RiftDialog.IsVisible)
            {
                return false;
            }
            _riftStartTime = DateTime.UtcNow;
            ZetaDia.Me.OpenRift(_level);
            if (_level == -1)
            {
                await Coroutine.Sleep(5000);
                if (IsRiftPortalOpen)
                {
                    _prePortalWorldDynamicId = AdvDia.CurrentWorldDynamicId;
                    State = States.EnteringRift;
                }
            }
            else
            {
                await Coroutine.Wait(30000, () => ZetaDia.CurrentRift.IsStarted && RiftData.RiftWorldIds.Contains(AdvDia.CurrentWorldId) && !ZetaDia.IsLoadingWorld);
                DisablePulse();
                State = States.EnteringGreaterRift;
            }
            return false;
        }

        private bool IsRiftPortalOpen
        {
            get
            {
                if (AdvDia.RiftQuest.State == QuestState.Completed || AdvDia.RiftQuest.State == QuestState.NotStarted)
                {
                    return false;
                }
                if (_RiftType == RiftType.Nephalem)
                {
                    return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).Any(g => g.ActorSNO == RiftData.RiftEntryPortalSNO);
                }
                else
                {
                    return ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true).Any(g => g.ActorSNO == RiftData.GreaterRiftEntryPortalSNO);

                }
            }
        }

        private async Task<bool> EnteringRift()
        {
            EnablePulse();
            if (_RiftType == RiftType.Nephalem)
            {
                if (!await UsePortalCoroutine.UsePortal(RiftData.RiftEntryPortalSNO, _prePortalWorldDynamicId))
                    return false;
            }
            else
            {
                if (!await UsePortalCoroutine.UsePortal(RiftData.GreaterRiftEntryPortalSNO, _prePortalWorldDynamicId))
                    return false;
            }
            State = States.OnNewRiftLevel;
            return false;
        }


        private bool EnteringGreaterRift()
        {
            if (ZetaDia.IsLoadingWorld || AdvDia.CurrentWorldId == ExplorationData.ActHubWorldIds[Act.A1])
            {
                return false;
            }
            State = States.OnNewRiftLevel;
            return false;
        }

        private bool OnNewRiftLevel()
        {
            EnablePulse();
            RiftData.AddEntryPortal();

            if (AdvDia.CurrentLevelAreaId == 276150 && !_holyCowEventCompleted)
            {
                _possiblyCowLevel = true;
            }
            if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore && AdvDia.RiftQuest.Step == RiftStep.Cleared)
            {
                State = States.SearchingForTownstoneOrExitPortal;
            }
            else
            {
                State = States.SearchingForExitPortal;
            }
            if (Randomizer.GetRandomNumber(1, 10) > 5)
            {
                Logger.Info("[Rift] Let the massacre continue!");
            }
            else
            {
                Logger.Info("[Rift] Crom, Count the Dead!");
            }
            return false;
        }

        private async Task<bool> SearchingForExitPortal()
        {
            if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore && AdvDia.RiftQuest.Step == RiftStep.Cleared)
            {
                State = States.SearchingForTownstoneOrExitPortal;
                return false;
            }
            EnablePulse();
            if (_nextLevelPortalLocation != Vector3.Zero)
            {
                State = States.MovingToExitPortal;
                return false;
            }

            if (!await ExplorationCoroutine.Explore(new HashSet<int> { AdvDia.CurrentLevelAreaId })) return false;
            ScenesStorage.Reset();
            return false;
        }


        private async Task<bool> SearchingForTownstoneOrExitPortal()
        {
            EnablePulse();
            if (_nextLevelPortalLocation != Vector3.Zero)
            {
                State = States.MovingToExitPortal;
                return false;
            }

            if (!await ExplorationCoroutine.Explore(new HashSet<int> { AdvDia.CurrentLevelAreaId })) return false;
            ScenesStorage.Reset();
            return false;
        }


        private bool TownstoneFound()
        {
            Logger.Info("[Rift] That's it folks, returning to town.");
            State = States.ReturningToTown;
            return false;
        }


        private async Task<bool> MovingToExitPortal()
        {
            EnablePulse();
            if (!await NavigationCoroutine.MoveTo(_nextLevelPortalLocation, 15)) return false;
            _nextLevelPortalLocation = Vector3.Zero;
            if (NavigationCoroutine.LastResult == CoroutineResult.Failure)
            {
                _portalScanRange = ActorFinder.LowerSearchRadius(_portalScanRange);
                if (_portalScanRange <= 100)
                {
                    _portalScanRange = 100;
                }
                if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore &&
                    AdvDia.RiftQuest.Step == RiftStep.Cleared)
                {
                    State = States.SearchingForTownstoneOrExitPortal;
                }
                else
                {
                    State = States.SearchingForExitPortal;
                }
                return false;
            }
            var portal =
                ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true)
                    .Where(g => g.IsFullyValid() && g.IsPortal)
                    .OrderBy(g => g.Distance)
                    .FirstOrDefault();
            if (portal == null)
            {
                portal = BountyHelpers.GetPortalNearMarkerPosition(_nextLevelPortalLocation);
                if (portal == null)
                {
                    if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore &&
                        AdvDia.RiftQuest.Step == RiftStep.Cleared)
                    {
                        State = States.SearchingForTownstoneOrExitPortal;
                    }
                    else
                    {
                        State = States.SearchingForExitPortal;
                    }
                    return false;
                }
            }
            State = States.EnteringExitPortal;
            _nextLevelPortalSNO = portal.ActorSNO;
            _prePortalWorldDynamicId = AdvDia.CurrentWorldDynamicId;
            return false;
        }

        private async Task<bool> EnteringExitPortal()
        {
            EnablePulse();
            if (await UsePortalCoroutine.UsePortal(_nextLevelPortalSNO, _prePortalWorldDynamicId))
            {
                State = States.OnNewRiftLevel;
                return false;
            }
            return false;
        }


        private bool BossSpawned()
        {
            EnablePulse();
            if (AdvDia.RiftQuest.Step != RiftStep.Cleared)
            {
                State = States.SearchingForBoss;
            }
            else
            {
                if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore)
                {
                    State = States.SearchingForTownstoneOrExitPortal;
                }
                else
                {
                    State = States.Completed;
                }
            }
            return false;
        }

        private async Task<bool> SearchingForBoss()
        {
            EnablePulse();
            if (_bossLocation != Vector3.Zero)
            {
                State = States.MovingToBoss;
                return false;
            }
            if (!await ExplorationCoroutine.Explore(new HashSet<int> { AdvDia.CurrentLevelAreaId })) return false;
            Logger.Info("[Rift] The Boss must be scared, but we will find him!");
            ScenesStorage.Reset();
            return false;
        }

        private async Task<bool> MovingToBoss()
        {
            if (AdvDia.RiftQuest.Step == RiftStep.Cleared)
            {
                if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore)
                {
                    State = States.SearchingForTownstoneOrExitPortal;
                }
                else
                {
                    State = States.Completed;
                }
            }
            EnablePulse();
            if (!await NavigationCoroutine.MoveTo(_bossLocation, 5)) return false;
            if (AdvDia.MyPosition.Distance(_bossLocation) > 50)
            {
                _bossLocation = Vector3.Zero;
                State = States.SearchingForBoss;
                return false;
            }
            _bossLocation = Vector3.Zero;
            if (AdvDia.RiftQuest.Step != RiftStep.Cleared)
            {
                Logger.Info("[Rift] You will suffer and die, ugly creature!");
                State = States.KillingBoss;
            }
            return false;
        }

        private bool KillingBoss()
        {
            if (AdvDia.RiftQuest.Step != RiftStep.Cleared)
            {
                return false;
            }
            State = _level == -1 ? States.Completed : States.UrshiSpawned;
            return false;
        }

        private bool UrshiSpawned()
        {
            EnablePulse();
            State = States.SearchingForUrshi;
            return false;
        }

        private async Task<bool> SearchingForUrshi()
        {
            EnablePulse();
            if (_urshiLocation != Vector3.Zero)
            {
                State = States.MovingToUrshi;
                return false;
            }
            if (!await ExplorationCoroutine.Explore(new HashSet<int> { AdvDia.CurrentLevelAreaId })) return false;
            Logger.Info("[Rift] Where are you, my dear Urshi!");
            ScenesStorage.Reset();
            return false;
        }

        private async Task<bool> MovingToUrshi()
        {
            EnablePulse();
            if (!await NavigationCoroutine.MoveTo(_urshiLocation, 5)) return false;
            _urshiLocation = Vector3.Zero;
            State = States.InteractingWithUrshi;
            return false;
        }

        private async Task<bool> InteractingWithUrshi()
        {
            DisablePulse();
            if (RiftData.VendorDialog.IsVisible)
            {
                State = States.UpgradingGems;
                return false;
            }
            if (!await _interactWithUrshiCoroutine.GetCoroutine()) return false;
            await Coroutine.Wait(2500, () => RiftData.VendorDialog.IsVisible);
            _interactWithUrshiCoroutine.Reset();
            if (!RiftData.VendorDialog.IsVisible)
            {
                return false;
            }

            _gemUpgradesLeft = 3;
            _enableGemUpgradeLogs = false;
            State = States.UpgradingGems;
            return false;
        }

        private int _gemUpgradesLeft;
        private bool _enableGemUpgradeLogs;
        private async Task<bool> UpgradingGems()
        {

            if (RiftData.VendorDialog.IsVisible && RiftData.ContinueButton.IsVisible && RiftData.ContinueButton.IsEnabled)
            {
                Logger.Debug("[Rift] Clicking to Continue button.");
                RiftData.ContinueButton.Click();
                RiftData.VendorCloseButton.Click();
                await Coroutine.Sleep(250);
                return false;
            }

            var gemToUpgrade = GetUpgradeTarget(_enableGemUpgradeLogs);
            if (gemToUpgrade == null)
            {
                Logger.Info("[Rift] I couldn't find any gems to upgrade, failing.");
                State = States.Failed;
                return false;

            }
            _enableGemUpgradeLogs = false;
            if (AdvDia.RiftQuest.Step == RiftStep.Cleared)
            {
                Logger.Debug("[Rift] Rift Quest is completed, returning to town");
                State = States.Completed;
                return false;
            }

            Logger.Debug("[Rift] Gem upgrades left before the attempt: {0}", ZetaDia.Me.JewelUpgradesLeft);
            if (!await CommonCoroutines.AttemptUpgradeGem(gemToUpgrade))
            {
                Logger.Debug("[Rift] Gem upgrades left after the attempt: {0}", ZetaDia.Me.JewelUpgradesLeft);
                return false;
            }
            var gemUpgradesLeft = ZetaDia.Me.JewelUpgradesLeft;
            if (_gemUpgradesLeft != gemUpgradesLeft)
            {
                _gemUpgradesLeft = gemUpgradesLeft;
                _enableGemUpgradeLogs = true;
            }
            if (gemUpgradesLeft == 0)
            {
                Logger.Debug("[Rift] Finished all upgrades, returning to town.");
                State = States.Completed;
                return false;
            }


            return false;
        }

        private ACDItem GetUpgradeTarget(bool enableLog)
        {
            ZetaDia.Actors.Update();
            var gems = PluginSettings.Current.Gems;
            gems.UpdateGems(ZetaDia.Me.InTieredLootRunLevel + 1, PluginSettings.Current.GreaterRiftPrioritizeEquipedGems);

            var minChance = PluginSettings.Current.GreaterRiftGemUpgradeChance;
            var upgradeableGems =
                gems.Gems.Where(g => g.UpgradeChance >= minChance && !g.MaxRank).ToList();
            if (upgradeableGems.Count == 0)
            {
                if (enableLog) Logger.Info("[Rift] Couldn't find any gems which is over the minimum upgrade change, upgrading the gem with highest upgrade chance");
                upgradeableGems = gems.Gems.Where(g => !g.MaxRank).OrderByDescending(g => g.UpgradeChance).ToList();
            }
            if (upgradeableGems.Count == 0)
            {
                if (enableLog) Logger.Info("[Rift] Looks like you have no legendary gems, failing.");
                State = States.Failed;
                return null;
            }
            var gemToUpgrade = upgradeableGems.First();
            if (enableLog) Logger.Info("[Rift] Attempting to upgrade {0}", gemToUpgrade.DisplayName);
            var acdGem =
                ZetaDia.Actors.GetActorsOfType<ACDItem>()
                    .FirstOrDefault(
                        i =>
                            i.ItemType == ItemType.LegendaryGem && i.ActorSNO == gemToUpgrade.SNO &&
                            i.JewelRank == gemToUpgrade.Rank);
            return acdGem;
        }

        private async Task<bool> SearchingForHolyCow()
        {
            EnablePulse();
            if (_holyCowLocation != Vector3.Zero)
            {
                Logger.Info("[Rift] Mooooo!");
                State = States.MovingToHolyCow;
                return false;
            }
            if (!await ExplorationCoroutine.Explore(new HashSet<int> { AdvDia.CurrentLevelAreaId })) return false;
            Logger.Info("[Rift] I am no butcher, where is this cow?");
            ScenesStorage.Reset();
            return false;
        }

        private async Task<bool> MovingToHolyCow()
        {
            EnablePulse();
            if (!await NavigationCoroutine.MoveTo(_holyCowLocation, 5)) return false;
            _holyCowLocation = Vector3.Zero;
            Logger.Info("[Rift] Mooooo?");
            State = States.InteractingWithHolyCow;
            return false;
        }

        private async Task<bool> InteractingWithHolyCow()
        {
            EnablePulse();
            if (!await _talkToHolyCowCoroutine.GetCoroutine()) return false;
            _talkToHolyCowCoroutine.Reset();
            Logger.Info("[Rift] Mooo moooo....");
            State = States.SearchingForExitPortal;
            return false;
        }


        private bool Completed()
        {
            State = States.ReturningToTown;
            return false;
        }

        private bool Failed()
        {
            Logger.Error("[Rift] Failed to complete the rift.");
            return true;
        }


        #region Pulse Checks & Scans

        private static readonly HashSet<States> BossSpawnedStates = new HashSet<States> { States.BossSpawned, States.SearchingForBoss, States.KillingBoss, States.MovingToBoss };
        private static readonly HashSet<States> UrshiSpawnedStates = new HashSet<States> { States.UrshiSpawned, States.SearchingForUrshi, States.InteractingWithUrshi, States.MovingToUrshi };
        private static readonly HashSet<States> ClearedStates = new HashSet<States> { States.InTown, States.GoingToAct1Hub, States.ReturningToTown, States.MoveToOrek, States.TalkToOrek, States.TownstoneFound };
        private static readonly HashSet<States> EnteringRiftStates = new HashSet<States> { States.MoveToRiftStone, States.EnteringRift, States.OpeningRift };

        private void Scans()
        {
            switch (State)
            {
                case States.SearchingForExitPortal:
                    if (_possiblyCowLevel)
                    {
                        ScanForHolyCow();
                    }
                    ScanForExitPortal();
                    return;
                case States.SearchingForBoss:
                    ScanForBoss();
                    return;
                case States.SearchingForHolyCow:
                    ScanForHolyCow();
                    return;
                case States.SearchingForUrshi:
                    ScanForUrshi();
                    return;
                case States.SearchingForTownstoneOrExitPortal:
                    if (_possiblyCowLevel)
                    {
                        ScanForHolyCow();
                    }
                    ScanForExitPortal();
                    ScanForTownstone();
                    return;

            }
        }


        private void PulseChecks()
        {
            if (BrainBehavior.IsVendoring || !ZetaDia.IsInGame || ZetaDia.IsLoadingWorld || ZetaDia.IsPlayingCutscene)
            {
                DisablePulse();
                return;
            }

            //AdvDia.Update(true);

            if (AdvDia.CurrentWorldId == ExplorationData.ActHubWorldIds[Act.A1] && (AdvDia.RiftQuest.Step == RiftStep.KillingMobs || AdvDia.RiftQuest.Step == RiftStep.BossSpawned || AdvDia.RiftQuest.Step == RiftStep.UrshiSpawned))
            {
                if (!EnteringRiftStates.Contains(State))
                {
                    Logger.Info(
                        "[Rift] Oh darn, I managed to return to town, I better go back in the rift before anyone notices.");
                    State = States.MoveToRiftStone;
                    return;
                }
            }

            if (AdvDia.CurrentWorldId == ExplorationData.ActHubWorldIds[Act.A1] && State != States.EnteringRift)
            {
                DisablePulse();
                return;
            }


            if (_currentWorldDynamicId != AdvDia.CurrentWorldDynamicId && _previusWorldDynamicId != AdvDia.CurrentWorldDynamicId && AdvDia.CurrentWorldId != ExplorationData.ActHubWorldIds[Act.A1])
            {
                RiftData.AddEntryPortal();
                _previusWorldDynamicId = _currentWorldDynamicId;
                _currentWorldDynamicId = AdvDia.CurrentWorldDynamicId;
            }


            switch (AdvDia.RiftQuest.Step)
            {
                case RiftStep.BossSpawned:
                    if (!BossSpawnedStates.Contains(State))
                    {
                        Logger.Info("[Rift] Behold the Rift Boss!");
                        State = States.BossSpawned;
                    }
                    break;
                case RiftStep.UrshiSpawned:
                    if (!UrshiSpawnedStates.Contains(State))
                    {
                        _riftEndTime = DateTime.UtcNow;
                        var totalTime = _riftEndTime - _riftStartTime;
                        if (totalTime.TotalSeconds < 3600 && totalTime.TotalSeconds > 0)
                        {
                            Logger.Info("[Rift] All done. (Total Time: {0} mins {1} seconds)", totalTime.Minutes, totalTime.Seconds);
                            Logger.Info("[Rift] Level: {0}", ZetaDia.Me.InTieredLootRunLevel + 1);
                        }
                        else
                        {
                            Logger.Info("[Rift] All done. (Partial rift, no stats available)");
                        }
                        Logger.Info("[Rift] My dear Urshi, I have some gems for you.");
                        State = States.UrshiSpawned;
                    }
                    break;
                case RiftStep.Cleared:
                    if (_RiftType == RiftType.Nephalem && PluginSettings.Current.NephalemRiftFullExplore && State != States.TownstoneFound)
                    {
                        break;
                    }
                    if (!ClearedStates.Contains(State))
                    {
                        _riftEndTime = DateTime.UtcNow;
                        var totalTime = _riftEndTime - _riftStartTime;
                        if (totalTime.TotalSeconds < 3600 && totalTime.TotalSeconds > 0)
                        {
                            Logger.Info("[Rift] All done. (Total Time: {0} mins {1} seconds)", totalTime.Minutes,
                                totalTime.Seconds);
                        }
                        else
                        {
                            Logger.Info("[Rift] All done. (Partial rift, no stats available)");
                        }
                        State = States.ReturningToTown;
                    }
                    break;
            }

        }

        private void ScanForUrshi()
        {
            var urshi =
                ZetaDia.Actors.GetActorsOfType<DiaUnit>()
                    .FirstOrDefault(a => a.IsFullyValid() && a.ActorSNO == RiftData.UrshiSNO);
            if (urshi != null)
            {
                _urshiLocation = urshi.Position;
            }
            if (_urshiLocation != Vector3.Zero)
            {
                Logger.Info("[Rift] Urshi is near.");
                State = States.MovingToUrshi;
                Logger.Debug("[Rift] Found Urshi at distance {0}", AdvDia.MyPosition.Distance2D(_urshiLocation));
            }
        }

        private void ScanForBoss()
        {
            var portalMarker = AdvDia.CurrentWorldMarkers.FirstOrDefault(m => m.Id >= 0 && m.Id <= 200);
            if (portalMarker != null)
            {
                _bossLocation = portalMarker.Position;
            }
            if (_bossLocation == Vector3.Zero)
            {
                var boss =
                    ZetaDia.Actors.GetActorsOfType<DiaUnit>()
                        .FirstOrDefault(a => a.IsFullyValid() && a.CommonData.IsUnique);
                if (boss != null)
                {
                    if (boss.IsAlive)
                    {
                        _bossLocation = boss.Position;
                    }
                }
            }
            if (_bossLocation != Vector3.Zero)
            {
                Logger.Info("[Rift] The Boss is near.");
                State = States.MovingToBoss;
                Logger.Debug("[Rift] Found the boss at distance {0}", AdvDia.MyPosition.Distance2D(_bossLocation));
            }
        }

        private void ScanForHolyCow()
        {
            var holyCow =
                ZetaDia.Actors.GetActorsOfType<DiaUnit>()
                    .FirstOrDefault(a => a.IsFullyValid() && a.ActorSNO == RiftData.HolyCowSNO && a.IsInteractableQuestObject());
            if (holyCow != null)
            {
                _holyCowLocation = holyCow.Position;
                switch (State)
                {
                    case States.SearchingForExitPortal:
                        State = States.SearchingForHolyCow;
                        break;
                }
            }

        }


        private void ScanForExitPortal()
        {
            if (_nextLevelPortalLocation != Vector3.Zero) return;
            var portal =
                ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true)
                    .Where(
                        g =>
                            g.IsFullyValid() && g.IsPortal && !RiftData.DungeonStoneSNOs.Contains(g.ActorSNO) &&
                            g.Distance < _portalScanRange && g.CommonData.GizmoType != GizmoType.HearthPortal)
                    .OrderBy(g => g.Position.Distance2DSqr(AdvDia.MyPosition))
                    .FirstOrDefault();
            if (portal != null)
            {
                _nextLevelPortalLocation = portal.Position;
                var currentWorldExitPortalHash = RiftData.GetRiftExitPortalHash(AdvDia.CurrentWorldId);
                var marker = AdvDia.CurrentWorldMarkers.FirstOrDefault(m => m.Position.Distance2D(_nextLevelPortalLocation) < 10);
                if (marker == null)
                {
                    _nextLevelPortalLocation = Vector3.Zero;
                }
                else
                {
                    if (marker.NameHash != currentWorldExitPortalHash)
                    {
                        _nextLevelPortalLocation = Vector3.Zero;
                    }
                }
            }
            if (_nextLevelPortalLocation != Vector3.Zero)
            {
                Logger.Info("[Rift] Oh look! There is a portal over there, let's see what's on the other side.");
                Logger.Debug("[Rift] Found the objective at distance {0}",
                    AdvDia.MyPosition.Distance2D(_nextLevelPortalLocation));
            }
        }

        private void ScanForTownstone()
        {
            var townStone =
                ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true)
                    .FirstOrDefault(a => a.IsFullyValid() && a.ActorSNO == RiftData.TownstoneSNO);
            if (townStone != null)
            {
                State = States.TownstoneFound;
            }

        }
        #endregion



        #region OnPulse Implementation
        private readonly WaitTimer _pulseTimer = new WaitTimer(TimeSpan.FromMilliseconds(250));
        private bool _isPulsing;

        private void EnablePulse()
        {
            if (!_isPulsing)
            {
                Logger.Debug("[Rift] Registered to pulsator.");
                Pulsator.OnPulse += OnPulse;
                _isPulsing = true;
            }
        }

        private void DisablePulse()
        {
            if (_isPulsing)
            {
                Logger.Debug("[Rift] Unregistered from pulsator.");
                Pulsator.OnPulse -= OnPulse;
                _isPulsing = false;
            }
        }

        private void OnPulse(object sender, EventArgs e)
        {
            if (_pulseTimer.IsFinished)
            {
                _pulseTimer.Stop();
                Scans();
                _pulseTimer.Reset();
            }
        }

        #endregion

        public void Dispose()
        {
            DisablePulse();
        }

    }


}
