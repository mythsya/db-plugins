using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adventurer.Coroutines;
using Adventurer.Coroutines.KeywardenCoroutines;
using Adventurer.Game.Actors;
using Adventurer.Game.Events;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace Adventurer.Game.Combat
{
    public class SafeZerg : PulsingObject
    {

        private static SafeZerg _instance;
        public static SafeZerg Instance
        {
            get { return _instance ?? (_instance = new SafeZerg()); }
        }

        private SafeZerg() { }

        private bool _zergEnabled;

        public void EnableZerg()
        {
            EnablePulse();
            _zergEnabled = true;
        }

        public void DisableZerg()
        {
            DisablePulse();
            _zergEnabled = false;
            TargetingHelper.TurnCombatOn();
        }

        protected override void OnPulse()
        {
            ZergCheck();
        }

        private void ZergCheck()
        {
            if (!_zergEnabled) { return; }
            var corruptGrowthDetectionRadius = ZetaDia.Me.ActorClass == ActorClass.Barbarian ? 30 : 20;
            var combatState = false;

            if (!combatState && ZetaDia.Me.HitpointsCurrentPct <= 0.8f)
            {
                combatState = true;
            }

            if (!combatState && 
                
                ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Any(u => u.IsFullyValid() && u.IsAlive && ( 
//                u.CommonData.IsElite || u.CommonData.IsRare || u.CommonData.IsUnique ||
                KeywardenDataFactory.GoblinSNOs.Contains(u.ActorSNO) || (KeywardenDataFactory.A4CorruptionSNOs.Contains(u.ActorSNO) && u.IsAlive & u.Position.Distance(AdvDia.MyPosition) <= corruptGrowthDetectionRadius))
                ))
               
            {
                combatState = true;
            }

            if (!combatState && ZetaDia.Actors.GetActorsOfType<DiaUnit>(true).Count(u => u.IsFullyValid() && u.IsHostile && u.IsAlive && u.Position.Distance(AdvDia.MyPosition) <= 15f) >= 8)
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
    }
}
