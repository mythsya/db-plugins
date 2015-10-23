using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adventurer.Game.Actors;
using Adventurer.Game.Combat;
using Adventurer.Game.Exploration;
using Adventurer.Game.Rift;
using Adventurer.Settings;
using Adventurer.Util;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Common.Helpers;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.Coroutines.KeywardenCoroutines
{
    public class KeywardenCoroutine : IDisposable
    {
        private readonly KeywardenData _keywardenData;
        private Vector3 _keywardenLocation = Vector3.Zero;
        private HashSet<int> _levelAreaIds;
        private WaitCoroutine _waitCoroutine;

        private enum States
        {
            NotStarted,
            TakingWaypoint,
            Searching,
            Moving,
            Waiting,
            Completed,
            Failed
        }

        private States _state;
        private States State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;

                switch (value)
                {
                    case States.NotStarted:
                        break;
                    default:
                        Logger.Debug("[Keywarden] " + value);
                        break;
                }
                _state = value;
            }
        }

        public KeywardenCoroutine(KeywardenData keywardenData)
        {
            _keywardenData = keywardenData;
            _levelAreaIds = new HashSet<int> { _keywardenData.LevelAreaId };
        }

        public async Task<bool> GetCoroutine()
        {
            switch (State)
            {
                case States.NotStarted:
                    return NotStarted();
                case States.TakingWaypoint:
                    return await TakingWaypoint();
                case States.Searching:
                    return await Searching();
                case States.Moving:
                    return await Moving();
                case States.Waiting:
                    return await Waiting();
                case States.Completed:
                    return await Completed();
                case States.Failed:
                    return await Failed();
            }
            return false;
        }

        private bool NotStarted()
        {
            DisablePulse();
            _keywardenLocation = Vector3.Zero;

            if (!_keywardenData.IsAlive)
            {
                State = States.Completed;
                return false;
            }
            TargetingHelper.TurnCombatOn();
            Logger.Info("[Keywarden] Lets go find da guy with da machina, shall we?");
            State = AdvDia.CurrentLevelAreaId == _keywardenData.LevelAreaId ? States.Searching : States.TakingWaypoint;
            return false;
        }

        private async Task<bool> TakingWaypoint()
        {
            DisablePulse();
            if (!await WaypointCoroutine.UseWaypoint(_keywardenData.WaypointNumber)) return false;
            State = States.Searching;
            return false;
        }

        private async Task<bool> Searching()
        {
            if (!_keywardenData.IsAlive)
            {
                State = States.Waiting;
                return false;
            }
            EnablePulse();
            if (_keywardenLocation != Vector3.Zero)
            {
                State = States.Moving;
                Logger.Info("[Keywarden] It's clobberin time!");
                return false;
            }
            if (!await ExplorationCoroutine.Explore(_levelAreaIds)) return false;
            
            Logger.Error("[Keywarden] Oh shit, that guy is nowhere to be found.");
            ScenesStorage.ResetVisited();
            State = States.Searching;
            return false;
        }

        private async Task<bool> Moving()
        {
            EnablePulse();
            TargetingHelper.TurnCombatOn();
            if (!await NavigationCoroutine.MoveTo(_keywardenLocation, 5)) return false;
            if (_keywardenData.IsAlive)
            {
                _keywardenLocation = GetKeywardenLocation();
                if (_keywardenLocation == Vector3.Zero)
                {
                    State = States.Searching;
                }
            }
            else
            {
                Logger.Info("[Keywarden] Keywarden shish kebab!");
                State = States.Waiting;
            }
            return false;
        }

        private async Task<bool> Waiting()
        {
            DisablePulse();
            if (_waitCoroutine == null)
            {
                _waitCoroutine = new WaitCoroutine(5000);
            }
            if (!await _waitCoroutine.GetCoroutine()) return false;
            _waitCoroutine = null;
            State = States.Completed;
            return false;
        }


        private async Task<bool> Completed()
        {
            DisablePulse();
            return true;
        }

        private async Task<bool> Failed()
        {
            DisablePulse();
            return true;
        }

        private void Scans()
        {
            _keywardenLocation = GetKeywardenLocation();
            if (State == States.Searching)
            {
                ZergCheck();
            }
        }

        private Vector3 GetKeywardenLocation()
        {
            if(_keywardenLocation!=Vector3.Zero) return _keywardenLocation;
            var keywarden = ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).FirstOrDefault(a => a.IsFullyValid() && a.ActorSNO == _keywardenData.KeywardenSNO);
            return (keywarden != null) ? keywarden.Position : Vector3.Zero;
        }

        private void ZergCheck()
        {
            if (PluginSettings.Current.KeywardenZergMode.HasValue && !PluginSettings.Current.KeywardenZergMode.Value)
            {
                return;
            }
            var corruptGrowthDetectionRadius = ZetaDia.Me.ActorClass == ActorClass.Barbarian ? 30 : 20;
            var combatState = false;

            if (_keywardenLocation != Vector3.Zero && _keywardenLocation.Distance(AdvDia.MyPosition) <= 50f)
            {
                combatState = true;
            }

            if (!combatState && ZetaDia.Me.HitpointsCurrentPct <= 0.8f)
            {
                combatState = true;
            }

            if (!combatState && _keywardenData.Act == Act.A4)
            {
                if (ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Any(
                            a =>
                                a.IsFullyValid() && KeywardenDataFactory.A4CorruptionSNOs.Contains(a.ActorSNO) &&
                                a.IsAlive & a.Position.Distance(AdvDia.MyPosition) <= corruptGrowthDetectionRadius))
                {
                    combatState = true;
                }
            }


            if (!combatState && ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Any(u => u.IsFullyValid() && u.IsAlive && KeywardenDataFactory.GoblinSNOs.Contains(u.ActorSNO)))
            {
                combatState = true;
            }

            if (!combatState && ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Count(u => u.IsFullyValid() && u.IsHostile && u.IsAlive && u.Position.Distance(AdvDia.MyPosition) <= 15f) >= 7)
            {
                combatState = true;
            }

            if (combatState)
            {
                TargetingHelper.TurnCombatOn();
            }
            else
            {
                TargetingHelper.TurnCombatOff();
            }
        }

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
            if (!Adventurer.GetCurrentTag().StartsWith("KeywardensTag"))
            {
                DisablePulse();
                return;
            }
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
