using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adventurer.Util;
using Zeta.Bot;

namespace Adventurer.Coroutines.BountyCoroutines
{
    public class AttackCoroutine
    {

        #region State

        private enum States
        {
            NotStarted,
            Checking,
            Attacking,
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
                if (value != States.NotStarted)
                {
                    Logger.Debug("[Attack] " + value);
                }
                _state = value;
            }
        }

        #endregion

        private AttackCoroutine(int actorGuid)
        {
            var attackSkill = RoutineManager.Current.DestroyObjectPower;
            Logger.Debug("[Attack] Selected ");
        }
    }
}
