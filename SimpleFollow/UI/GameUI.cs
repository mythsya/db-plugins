using System;
using System.Diagnostics;
using SimpleFollow.Helpers;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals;

namespace SimpleFollow.UI
{
    public class GameUI
    {
        //private const ulong mercenaryOKHash = 1591817666218338490;
        private const ulong MercenaryPartyOKHash = 0xF85A00117F5902E9;
        //private const ulong conversationSkipHash = 0x942F41B6B5346714;
        //private const ulong TalkToInteractButton1Hash = 0x8EB3A93FB1E49EB8;
        //private const ulong ConfirmTimedDungeonOKHash = 0xF9E7B8A635A4F725;
        //private const ulong genericOKHash = 0x891D21408238D18E;
        private const ulong GenericOKHash = 0x891D21408238D18E;
        private const ulong PartyLeaderBossAcceptHash = 0x69B3F61C0F8490B0;
        private const ulong PartyFollowerBossAcceptHash = 0xF495983BA9BE450F;
        private const ulong PlayGameButtonHash = 0x51A3923949DC80B7;
        private const ulong CustomizeBannerCloseHash = 0xF92E21E990666E6B;
        private const ulong PartySlot2IconHash = 0x24F157EFB9C1744A;
        private const ulong PartyInviteOKHash = 0x9EFA05648195042D;
        private const ulong BattleNetOKHash = 0xB4433DA3F648A992;

        private const ulong LeaveGameButtonHash = 0x3B55BA1E41247F50;
        private const ulong OutOfGameLeavePartyButtonHash = 0xCD03E1585F7150B9;

        internal const int ClickThreadSleepInterval = 125;

        private static Stopwatch clickTimer = new Stopwatch();
        private static Random clickDelayRnd = new Random();

        private static TimeSpan ClickTimerTimeout = new TimeSpan(0, 0, 0, 5, 250);

        private static int clickTimerRandomVal = -1;

        public static UIElement OutOfGameLeavePartyButton
        {
            get { return UIElement.FromHash(OutOfGameLeavePartyButtonHash); }
        }

        public static UIElement PlayGameButton
        {
            get { return UIElement.FromHash(PlayGameButtonHash); }
        }

        public static UIElement PartyLeaderBossAccept
        {
            get { return UIElement.FromHash(PartyLeaderBossAcceptHash); }
        }

        public static UIElement PartyFollowerBossAccept
        {
            get { return UIElement.FromHash(PartyFollowerBossAcceptHash); }
        }

        public static UIElement GenericOK
        {
            get { return UIElement.FromHash(GenericOKHash); }
        }

        public static UIElement BattleNetOK
        {
            get { return UIElement.FromHash(BattleNetOKHash); }
        }

        public static UIElement MercenaryPartyOK
        {
            get { return UIElement.FromHash(MercenaryPartyOKHash); }
        }

        public static UIElement PartyInviteOK
        {
            get { return UIElement.FromHash(PartyInviteOKHash); }
        }

        public static UIElement CustomizeBannerClose
        {
            get { return UIElement.FromHash(CustomizeBannerCloseHash); }
        }

        // this is used to check if we are in a party, from the game menu
        public static UIElement PartySlot2Icon
        {
            get { return UIElement.FromHash(PartySlot2IconHash); }
        }

        public static UIElement LeaveGameButton
        {
            get { return UIElement.FromHash(LeaveGameButtonHash); }
        }

        private static DateTime lastSafeClickCheck = DateTime.MinValue;

        internal static bool ElementIsVisible(UIElement uiElement, string name = "")
        {
            if (uiElement == null)
            {
                return false;
            }

            if (!uiElement.IsValid)
            {
                //if (SimpleFollowSettings.Instance.DebugLogging)
                //    Logr.Debug(("Element {0} is not valid", uiElement.Hash);
                return false;
            }

            if (!uiElement.IsVisible)
            {
                //if (SimpleFollowSettings.Instance.DebugLogging)
                //    Logr.Debug(("Element {0} {1} ({2}) is not visible", uiElement.Hash, name, uiElement.Name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks for known windows, buttons, etc and clicks them
        /// </summary>
        internal static void SafeCheckClickButtons()
        {
            try
            {
                if (!ZetaDia.IsInGame)
                {
                    SafeClick(BattleNetOK, ClickDelay.NoDelay, "Battle.Net OK", 1000, true);
                }

                // limit this thing running to once a second, to save CPU
                if (DateTime.UtcNow.Subtract(lastSafeClickCheck).TotalMilliseconds < 1000)
                    return;

                if (ZetaDia.IsLoadingWorld)
                    return;

                if (ZetaDia.IsPlayingCutscene)
                    return;

                if (!ZetaDia.Service.IsValid)
                    return;

                lastSafeClickCheck = DateTime.UtcNow;

                // Handled seperately out of game
                if (ZetaDia.IsInGame)
                    SafeClick(PartyInviteOK, ClickDelay.Delay, "Party Invite", 750, true);

                SafeClick(GenericOK, ClickDelay.Delay, "Generic OK", 0, true);
                SafeClick(BattleNetOK, ClickDelay.NoDelay, "Battle.Net OK", 1000, true);

                if (!ZetaDia.IsInGame)
                    return;

                if (ZetaDia.Me == null)
                    return;

                if (ZetaDia.Me.IsDead)
                    return;

                if (!ZetaDia.Me.IsValid)
                    return;

                SafeClick(PartyLeaderBossAccept, ClickDelay.NoDelay, "Boss Portal Accept", 0, true);
                SafeClick(PartyFollowerBossAccept, ClickDelay.NoDelay, "Boss Portal Accept", 0, true);
                SafeClick(MercenaryPartyOK, ClickDelay.NoDelay, "Mercenary Party OK");
                SafeClick(CustomizeBannerClose, ClickDelay.NoDelay, "Customize Banner Close");
            }
            catch (Exception ex)
            {
                Logr.Log("Error clicking UI Button: " + ex.ToString());
            }
        }

        private static void SetClickTimerRandomVal()
        {
            clickTimerRandomVal = clickDelayRnd.Next(1250, 2250);
            Logr.Log("Random timer set to {0}ms", clickTimerRandomVal);
        }

        private static bool ClickTimerRandomReady()
        {
            return clickTimer.IsRunning && clickTimer.ElapsedMilliseconds >= clickTimerRandomVal;
        }

        private static bool ClickTimerRandomNotReady()
        {
            long timeRemaining = clickTimerRandomVal - clickTimer.ElapsedMilliseconds;
            Logr.Log("Pausing bot -  waiting for button timer {0}ms", clickTimerRandomVal);
            return clickTimer.IsRunning && clickTimer.ElapsedMilliseconds < clickTimerRandomVal;
        }

        private static DateTime _lastClick = DateTime.MinValue;

        /// <summary>
        /// Clicks a UI Element after a random interval. 
        /// </summary>
        /// <param name="uiElement"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static bool SafeClick(UIElement uiElement, ClickDelay delayOption, string name = "", int postClickThreadSleepDuration = 0, bool fireWorldTransferStart = false)
        {
            try
            {
                if (DateTime.UtcNow.Subtract(_lastClick).TotalMilliseconds < 500)
                    return false;

                if (ElementIsVisible(uiElement, name))
                {
                    if (!String.IsNullOrEmpty(name))
                    {
                        Logr.Debug("{0} button is visible", name);
                    }
                    else
                    {
                        Logr.Debug("{0}={1} is visible", uiElement.Hash, uiElement.Name);
                    }

                    if (!clickTimer.IsRunning && delayOption == ClickDelay.Delay)
                    {
                        clickTimer.Start();
                        SetClickTimerRandomVal();
                    }
                    else if ((ClickTimerRandomReady() && ElementIsVisible(uiElement, name)) || delayOption == ClickDelay.NoDelay)
                    {
                        if (!String.IsNullOrEmpty(name))
                        {
                            Logr.Log("Clicking {0} button", name);
                        }
                        else
                        {
                            Logr.Log("Clicking {0}={1}", uiElement.Hash, uiElement.Name);
                        }

                        // sleep plugins for a bit
                        if (ZetaDia.IsInGame && fireWorldTransferStart)
                            GameEvents.FireWorldTransferStart();

                        _lastClick = DateTime.UtcNow;
                        uiElement.Click();
                        //BotMain.PauseFor(TimeSpan.FromMilliseconds(ClickThreadSleepInterval));

                        //if (postClickThreadSleepDuration > 0)
                        //    BotMain.PauseFor(TimeSpan.FromMilliseconds(postClickThreadSleepDuration));

                        clickTimer.Reset();
                    }
                    else
                    {
                        Logr.Debug("Pausing bot, waiting for {0}={1}", uiElement.Hash, uiElement.Name);
                        BotMain.PauseWhile(ClickTimerRandomNotReady, 0, ClickTimerTimeout);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logr.Log("Error clicking UI button {0}: " + ex.ToString(), name);
                return false;
            }
        }
    }

    public enum ClickDelay
    {
        NoDelay = 0,
        Delay = 1
    }
}