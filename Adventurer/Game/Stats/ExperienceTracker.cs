using System;
using Adventurer.Game.Events;
using Adventurer.Util;
using Zeta.Game;

namespace Adventurer.Game.Stats
{
    public class ExperienceTracker : PulsingObject
    {
        public long CurrentExperience { get; private set; }
        public TimeSpan CurrentTime { get { return DateTime.UtcNow - _startTime; } }
        private DateTime _startTime;
        private long _lastSeen;
        public bool IsStarted { get; private set; }

        public void Start()
        {
            _startTime = DateTime.UtcNow;
            CurrentExperience = 0;
            _lastSeen = GetLastSeen();
            EnablePulse();
            IsStarted = true;
            Logger.Info("[XPTracker] Starting a new experience tracking session.");
        }

        public void StopAndReport(string reporterName)
        {
            DisablePulse();
            if (IsStarted)
            {
                UpdateExperience();
                ReportExperience(reporterName);
            }
            IsStarted = false;
        }

        private void ReportExperience(string reporterName)
        {
            Logger.Warn("[{0}] Total XP Gained: {1:0,0}", reporterName, CurrentExperience);
            Logger.Warn("[{0}] XP / Hour: {1:0,0}", reporterName, CurrentExperience / (DateTime.UtcNow - _startTime).TotalHours);
        }

        private void UpdateExperience()
        {
            var currentLastSeen = GetLastSeen();
            if (_lastSeen < currentLastSeen)
            {
                CurrentExperience += (currentLastSeen - _lastSeen);
            }
            _lastSeen = currentLastSeen;
        }

        private static long GetLastSeen()
        {
            return ZetaDia.Me.Level == 70
                ? ZetaDia.Me.ParagonCurrentExperience
                : ZetaDia.Me.CurrentExperience;
        }

        protected override void OnPulse()
        {
            UpdateExperience();
        }
    }
}
