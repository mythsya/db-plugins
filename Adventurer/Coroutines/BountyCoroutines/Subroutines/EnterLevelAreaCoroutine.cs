﻿using System.Threading.Tasks;
using Adventurer.Game.Actors;
using Adventurer.Game.Combat;
using Adventurer.Game.Exploration;
using Adventurer.Game.Quests;
using Adventurer.Settings;
using Adventurer.Util;
using Buddy.Coroutines;
using Zeta.Common;
using Zeta.Game;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.Coroutines.BountyCoroutines.Subroutines
{
    public class EnterLevelAreaCoroutine : IBountySubroutine
    {
        private readonly int _questId;
        private readonly int _sourceWorldId;
        private readonly int _destinationWorldId;
        private readonly int _portalMarker;
        private readonly int _portalActorId;
        private int _discoveredPortalActorId;
        private bool _prioritizeExitScene;
        private bool _exitSceneUnreachable;
        private int _prePortalWorldDynamicId;
        private bool _isDone;
        private States _state;
        private int _objectiveScanRange = 5000;
        private Vector3 _objectiveLocation = Vector3.Zero;
        private Vector3 _exitSceneLocation = Vector3.Zero;

        #region State

        public enum States
        {
            NotStarted,
            Searching,
            Moving,
            MovingToExitScene,
            Entering,
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
                    Logger.Info("[EnterLevelArea] " + value);
                }
                _state = value;
            }
        }

        #endregion

        public bool IsDone
        {
            get
            {
                if (AdvDia.CurrentWorldId == SourceWorldId) return false;
                if (AdvDia.CurrentWorldId == DestinationWorldId)
                {
                    //SafeZerg.Instance.DisableZerg();
                    return true;
                }
                return _isDone && AdvDia.CurrentWorldId == DestinationWorldId || AdvDia.CurrentWorldId != SourceWorldId;
            }
        }

        public int SourceWorldId
        {
            get { return _sourceWorldId; }
        }

        public int DestinationWorldId
        {
            get { return _destinationWorldId; }
        }


        public EnterLevelAreaCoroutine(int questId, int sourceWorldId, int destinationWorldId, int portalMarker, int portalActorId, bool prioritizeExitScene = false)
        {
            _questId = questId;
            _sourceWorldId = sourceWorldId;
            _destinationWorldId = destinationWorldId;
            _portalMarker = portalMarker;
            _portalActorId = portalActorId;
            _prioritizeExitScene = prioritizeExitScene;
        }


        public async Task<bool> GetCoroutine()
        {
            if (PluginSettings.Current.BountyZerg) SafeZerg.Instance.EnableZerg();
            switch (State)
            {
                case States.NotStarted:
                    return await NotStarted();
                case States.Searching:
                    return await Searching();
                case States.Moving:
                    return await Moving();
                case States.MovingToExitScene:
                    return await MovingToExitScene();
                case States.Entering:
                    return await Entering();
                case States.Completed:
                    return await Completed();
                case States.Failed:
                    return await Failed();
            }
            return false;
        }

        public void Reset()
        {
            _isDone = false;
            _state = States.NotStarted;
            _objectiveScanRange = 5000;
            _objectiveLocation = Vector3.Zero;
            _exitSceneLocation = Vector3.Zero;
            _exitSceneUnreachable = false;
        }

        public void DisablePulse()
        {
        }

        public BountyData BountyData
        {
            get { return _bountyData ?? (_bountyData = BountyDataFactory.GetBountyData(_questId)); }
        }

        private async Task<bool> NotStarted()
        {
            State = States.Searching;
            return false;
        }

        private async Task<bool> Searching()
        {
            if (_objectiveLocation == Vector3.Zero)
            {
                ScanForObjective();
            }
            if (_objectiveLocation != Vector3.Zero)
            {
                State = States.Moving;
                return false;
            }
            //if (_prioritizeExitScene && !_exitSceneUnreachable && ScenesStorage.CurrentWorldSceneIds.Any(s => s.Contains("Exit")))
            //{
            //    var exitScene = ScenesStorage.CurrentWorldScenes.FirstOrDefault(s => s.Name.Contains("Exit"));
            //    if (exitScene != null)
            //    {
            //        var centerNode =
            //            exitScene.Nodes.Where(n=>n.HasEnoughNavigableCells).OrderBy(n => n.Center.DistanceSqr(exitScene.Center)).FirstOrDefault();
            //        if (centerNode != null)
            //        {
            //            _exitSceneLocation = centerNode.NavigableCenter;
            //            Logger.Debug("[EnterLevelArea] Moving to exit scene");
            //            State=States.MovingToExitScene;
            //            return false;
            //        }
            //    }

            //}
            if (!await ExplorationCoroutine.Explore(BountyData.LevelAreaIds)) return false;
            ScenesStorage.Reset();
            return false;
        }

        private async Task<bool> Moving()
        {
            if (!await NavigationCoroutine.MoveTo(_objectiveLocation, 12)) return false;

            if (AdvDia.MyPosition.Distance(_objectiveLocation) > 30 && NavigationCoroutine.LastResult == CoroutineResult.Failure)
            {
                _previouslyFoundLocation = _objectiveLocation;
                _returnTimeForPreviousLocation = PluginTime.CurrentMillisecond;
                _objectiveLocation = Vector3.Zero;
                _objectiveScanRange = ActorFinder.LowerSearchRadius(_objectiveScanRange);
                if (_objectiveScanRange <= 0)
                {
                    _objectiveScanRange = 50;
                }
                State = States.Searching;
                return false;
            }

            var portal = ActorFinder.FindGizmo(_portalActorId);
            if (portal == null)
            {
                portal = BountyHelpers.GetPortalNearMarkerPosition(_objectiveLocation);
                if (_portalActorId == 0) _discoveredPortalActorId = portal.ActorSNO;
                //if (_portalActorId != portal.ActorSNO && BountyData.Act == Act.A5)
                //{
                //    Logger.Info("[EnterLevelArea] Was expecting to use portal SNO {0}, using {1} instead.", _portalActorId, portal.ActorSNO);
                //    _portalActorId = portal.ActorSNO;
                //}
            }
            else
            {
                if (portal.Position.Distance(_objectiveLocation) > 15)
                {
                    portal = null;
                }
            }
            if (portal == null)
            {
                State = States.Searching;
                return false;
            }
            _objectiveLocation = portal.Position;
            State = States.Entering;
            _prePortalWorldDynamicId = AdvDia.CurrentWorldDynamicId;
            return false;
        }

        private async Task<bool> MovingToExitScene()
        {
            if (!await NavigationCoroutine.MoveTo(_exitSceneLocation, 12)) return false;

            if (AdvDia.MyPosition.Distance2D(_exitSceneLocation) > 30 && NavigationCoroutine.LastResult == CoroutineResult.Failure)
            {
                Logger.Debug("[EnterLevelArea] Exit scene is unreachable, going back to the normal routine");
                _exitSceneLocation = Vector3.Zero;
                _exitSceneUnreachable = true;
            }
            State = States.Searching;
            return false;
        }

        private async Task<bool> Entering()
        {
            if (_objectiveLocation.Distance(AdvDia.MyPosition) > 22)
            {
                State = States.Moving;
                return false;
            }
            if (!await UsePortalCoroutine.UsePortal(_portalActorId != 0 ? _portalActorId : _discoveredPortalActorId, _prePortalWorldDynamicId)) return false;
            if (AdvDia.CurrentWorldId != DestinationWorldId)
            {
                Logger.Debug("[Bounty] We are not where we are supposed to be.");
                State = States.Searching;
                return false;
            }
            _discoveredPortalActorId = 0;
            SafeZerg.Instance.DisableZerg();
            State = States.Completed;
            return false;
        }

        private async Task<bool> Completed()
        {
            EntryPortals.AddEntryPortal();
            await Coroutine.Sleep(1000);
            _isDone = true;
            return false;
        }

        private async Task<bool> Failed()
        {
            _isDone = true;
            return false;
        }


        private long _lastScanTime;
        private Vector3 _previouslyFoundLocation = Vector3.Zero;
        private long _returnTimeForPreviousLocation;
        private BountyData _bountyData;

        private void ScanForObjective()
        {
            if (_previouslyFoundLocation != Vector3.Zero && PluginTime.ReadyToUse(_returnTimeForPreviousLocation, 60000))
            {
                _objectiveLocation = _previouslyFoundLocation;
                _previouslyFoundLocation = Vector3.Zero;
                _returnTimeForPreviousLocation = PluginTime.CurrentMillisecond;
                Logger.Debug("[EnterLevelArea] Returning previous objective location.");

                return;
            }
            if (PluginTime.ReadyToUse(_lastScanTime, 250))
            {
                _lastScanTime = PluginTime.CurrentMillisecond;
                if (_portalMarker != 0)
                {
                    _objectiveLocation = BountyHelpers.ScanForMarkerLocation(_portalMarker, _objectiveScanRange);
                }
                // Belial
                if (_objectiveLocation == Vector3.Zero && _portalActorId == 159574)
                {
                    _objectiveLocation = BountyHelpers.ScanForActorLocation(_portalActorId, _objectiveScanRange);
                }
                //if (_objectiveLocation == Vector3.Zero && _portalActorId != 0)
                //{
                //    _objectiveLocation = BountyHelpers.ScanForActorLocation(_portalActorId, _objectiveScanRange);
                //}
                if (_objectiveLocation != Vector3.Zero)
                {
                    Logger.Info("[EnterLevelArea] Found the objective at distance {0}", AdvDia.MyPosition.Distance2D(_objectiveLocation));
                }
            }
        }
    }
}
