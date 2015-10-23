﻿using System.Threading.Tasks;
using Adventurer.Game.Actors;
using Adventurer.Game.Combat;
using Adventurer.Game.Exploration;
using Adventurer.Game.Quests;
using Adventurer.Settings;
using Adventurer.Util;
using Zeta.Common;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.Coroutines.BountyCoroutines.Subroutines
{
    public class MoveToMapMarkerCoroutine : IBountySubroutine
    {
        private readonly int _questId;
        private readonly int _worldId;
        private readonly int _marker;
        private readonly int _actorId;

        private bool _isDone;
        private States _state;

        private int _objectiveScanRange = 5000;

        #region State

        public enum States
        {
            NotStarted,
            Searching,
            Moving,
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
                    Logger.Info("[MoveToMapMarker] " + value);
                }
                _state = value;
            }
        }

        #endregion

        public bool IsDone
        {
            get { return _isDone || AdvDia.CurrentWorldId != _worldId; }
        }



        public MoveToMapMarkerCoroutine(int questId, int worldId, int marker, int actorId = 0, bool zergSafe = false)
        {
            _questId = questId;
            _worldId = worldId;
            _marker = marker;
            _actorId = actorId;
        }


        public async Task<bool> GetCoroutine()
        {
            if (PluginSettings.Current.BountyZerg && BountyData.QuestType != BountyQuestType.KillMonster) SafeZerg.Instance.EnableZerg();
            switch (State)
            {
                case States.NotStarted:
                    return await NotStarted();
                case States.Searching:
                    return await Searching();
                case States.Moving:
                    return await Moving();
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
                _partialMovesCount = 0;
                return false;
            }
            if (!await ExplorationCoroutine.Explore(BountyData.LevelAreaIds)) return false;
            ScenesStorage.Reset();
            return false;
        }

        private int _partialMovesCount;
        private async Task<bool> Moving()
        {
            if (!await NavigationCoroutine.MoveTo(_objectiveLocation, 10)) return false;
            if (AdvDia.MyPosition.Distance2D(_objectiveLocation) > 30 && NavigationCoroutine.LastResult == CoroutineResult.Failure)
            {
                _partialMovesCount++;
                if (_partialMovesCount < 2)
                {
                    return false;
                }
                _previouslyFoundLocation = _objectiveLocation;
                _returnTimeForPreviousLocation = PluginTime.CurrentMillisecond;
                _partialMovesCount = 0;
                _objectiveLocation = Vector3.Zero;
                _objectiveScanRange = ActorFinder.LowerSearchRadius(_objectiveScanRange);
                if (_objectiveScanRange <= 0)
                {
                    _objectiveScanRange = 50;
                }
                State = States.Searching;
                return false;
            }
            SafeZerg.Instance.DisableZerg();
            State = States.Completed;
            return false;
        }

        private async Task<bool> Completed()
        {
            SafeZerg.Instance.DisableZerg();
            _isDone = true;
            return false;
        }

        private async Task<bool> Failed()
        {
            _isDone = true;
            return false;
        }

        private Vector3 _objectiveLocation = Vector3.Zero;
        private Vector3 _previouslyFoundLocation = Vector3.Zero;
        private long _returnTimeForPreviousLocation;

        private long _lastScanTime;
        private BountyData _bountyData;

        private void ScanForObjective()
        {
            if (_previouslyFoundLocation != Vector3.Zero && PluginTime.ReadyToUse(_returnTimeForPreviousLocation, 60000))
            {
                _objectiveLocation = _previouslyFoundLocation;
                _previouslyFoundLocation = Vector3.Zero;
                _returnTimeForPreviousLocation = PluginTime.CurrentMillisecond;
                Logger.Debug("[MoveToMapMarker] Returning previous objective location.");

                return;
            }
            if (PluginTime.ReadyToUse(_lastScanTime, 1000))
            {
                _lastScanTime = PluginTime.CurrentMillisecond;
                if (_marker != 0)
                {
                    if (_marker == -1)
                    {
                        _objectiveLocation = BountyHelpers.ScanForMarkerLocation(0, _objectiveScanRange);

                    }
                    else
                    {
                        _objectiveLocation = BountyHelpers.ScanForMarkerLocation(_marker, _objectiveScanRange);

                    }
                }
                //if (_objectiveLocation == Vector3.Zero && _actorId != 0)
                //{
                //    _objectiveLocation = BountyHelpers.ScanForActorLocation(_actorId, _objectiveScanRange);
                //}
                if (_objectiveLocation != Vector3.Zero)
                {
                    using (new PerformanceLogger("[MoveToMapMarker] Path to Objective Check", true))
                    {
                        //if ((Navigator.GetNavigationProviderAs<DefaultNavigationProvider>().CanFullyClientPathTo(_objectiveLocation)))
                        //{
                        Logger.Info("[MoveToMapMarker] Found the objective at distance {0}",
                            AdvDia.MyPosition.Distance2D(_objectiveLocation));
                        //}
                        //else
                        //{
                        //    Logger.Debug("[MoveToMapMarker] Found the objective at distance {0}, but cannot get a path to it.",
                        //        AdvDia.MyPosition.Distance2D(_objectiveLocation));
                        //    _objectiveLocation = Vector3.Zero;
                        //}
                    }

                }
            }
        }
    }
}
