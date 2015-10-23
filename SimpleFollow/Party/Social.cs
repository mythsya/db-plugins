using System;
using SimpleFollow.Helpers;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals;

namespace SimpleFollow.Party
{
    public class Social
    {
        private const int uiClickThreadSleep = 250;

        public static UIElement SocialFlyoutButton
        {
            get { return UIElement.FromHash(0x80AFF6E674F9ACB4); }
        }

        public UIElement ToggleSocialButton
        {
            get { return UIElement.FromHash(0xAB9216FD8AADCFF9); }
        }

        public UIElement CloseSocialButton
        {
            get { return UIElement.FromHash(0x7B1FD584DA74FA94); }
        }

        public UIElement AddFriendButton
        {
            get { return UIElement.FromHash(0xC93C68F46C33F13); }
        }

        public UIElement Friend1InviteButton
        {
            get { return UIElement.FromHash(0xC590DACA798C3CA4); }
        }

        public UIElement Friend2InviteButton
        {
            get { return UIElement.FromHash(0x270DE7E871AC762B); }
        }

        public UIElement Friend3InviteButton
        {
            get { return UIElement.FromHash(0x5E598FAE1E003BBE); }
        }

        public UIElement Friend3AlternateInviteButton
        {
            get { return UIElement.FromHash(0x84326743BF45C585); }
        }

        public UIElement Friend4InviteButton
        {
            get { return UIElement.FromHash(0x688C5E9A3FEF18FE); }
        }

        public UIElement Friend4AlternateInviteButton
        {
            get { return UIElement.FromHash(0xE8A2876E22DA8E30); }
        }

        public UIElement PartySlot2Icon
        {
            get { return UIElement.FromHash(0x693898E759B64D02); }
        }

        public UIElement PartySlot3Icon
        {
            get { return UIElement.FromHash(0x55A8A8E38EBCA175); }
        }

        public UIElement PartySlot4Icon
        {
            get { return UIElement.FromHash(0x1B284C72BEB1897C); }
        }

        private static Social _instance = new Social();

        public static Social Instance
        {
            get { return _instance ?? (_instance = new Social()); }
        }

        public bool IsSocialWindowOpen
        {
            get
            {
                if (DateTime.UtcNow.Subtract(lastOpenedSocialWindow).TotalMilliseconds < 50)
                    return false;

                if (AddFriendButton == null)
                    return false;
                if (!AddFriendButton.IsValid)
                    return false;
                if (!AddFriendButton.IsVisible)
                    return false;

                return true;
            }
        }

        private static int lastNumPartyMembers = 0;
        private static DateTime lastUpdateNumPartyMembers = DateTime.MinValue;

        public static int NumPartyMembers
        {
            get
            {
                if (!ZetaDia.Service.IsValid)
                    return lastNumPartyMembers;
                if (!ZetaDia.Service.Hero.IsValid)
                    return lastNumPartyMembers;
                if (ZetaDia.IsLoadingWorld)
                    return lastNumPartyMembers;

                if (DateTime.UtcNow.Subtract(lastUpdateNumPartyMembers).TotalSeconds < 5)
                    return lastNumPartyMembers;

                if (ZetaDia.Service.IsValid &&
                    ZetaDia.Service.Platform.IsValid &&
                    ZetaDia.Service.Platform.IsConnected &&
                    ZetaDia.Service.Hero.IsValid &&
                    ZetaDia.Service.Party.IsValid)
                {
                    lastUpdateNumPartyMembers = DateTime.UtcNow;
                    lastNumPartyMembers = ZetaDia.Service.Party.NumPartyMembers;
                    return lastNumPartyMembers;
                }
                return 0;
            }
        }

        private static bool lastIsPartyLeader = false;
        private static DateTime lastUpdateIsPartyLeaderMembers = DateTime.MinValue;

        public static bool IsPartyleader
        {
            get
            {
                if (!ZetaDia.Service.IsValid)
                    return false;
                if (!ZetaDia.Service.Hero.IsValid)
                    return false;
                if (ZetaDia.IsLoadingWorld)
                    return false;

                if (DateTime.UtcNow.Subtract(lastUpdateIsPartyLeaderMembers).TotalSeconds < 5)
                    return lastIsPartyLeader;

                if (ZetaDia.Service.IsValid &&
                    ZetaDia.Service.Platform.IsValid &&
                    ZetaDia.Service.Platform.IsConnected &&
                    ZetaDia.Service.Hero.IsValid &&
                    ZetaDia.Service.Party.IsValid)
                {
                    lastUpdateIsPartyLeaderMembers = DateTime.UtcNow;
                    lastIsPartyLeader = ZetaDia.Service.Party.IsPartyLeader;
                    return lastIsPartyLeader;
                }
                return false;
            }
        }

        public static bool IsInParty
        {
            get { return NumPartyMembers > 1; }
        }

        public void OpenSocialWindow()
        {
            if (DateTime.UtcNow.Subtract(lastOpenedSocialWindow).TotalMilliseconds <= 500)
                return;
            // only click social button to open social window if window is not visible
            if (!IsSocialWindowOpen)
            {
                GameUI.SafeClick(SocialFlyoutButton, ClickDelay.NoDelay, "Social Flyout Button", 500);
                GameUI.SafeClick(ToggleSocialButton, ClickDelay.NoDelay, "Social Window", 500);
                lastOpenedSocialWindow = DateTime.UtcNow;
                recheckInvites = true;
                return;
            }
            if (IsSocialWindowOpen && !GameUI.ElementIsVisible(AddFriendButton, "Add Friend Button"))
            {
                Logr.Log("Unable to open social window!");
            }
        }

        public void CloseSocialWindow()
        {
            GameUI.SafeClick(CloseSocialButton, ClickDelay.NoDelay, "Social Window Close");
        }

        private DateTime lastInviteCheck = DateTime.MinValue;
        private DateTime lastOpenedSocialWindow = DateTime.MinValue;

        private bool recheckInvites = false;

        internal void CheckCloseSocialWindow()
        {
            if (DateTime.UtcNow.Subtract(lastOpenedSocialWindow).TotalSeconds < 5)
                return;

            bool anyInviteReady =
                GameUI.ElementIsVisible(Friend1InviteButton) ||
                GameUI.ElementIsVisible(Friend2InviteButton) ||
                GameUI.ElementIsVisible(Friend3InviteButton) ||
                GameUI.ElementIsVisible(Friend3AlternateInviteButton) ||
                GameUI.ElementIsVisible(Friend4InviteButton);

            if (IsSocialWindowOpen && (!anyInviteReady || DateTime.UtcNow.Subtract(lastOpenedSocialWindow).TotalSeconds > 20))
            {
                CloseSocialWindow();
            }
        }

        internal void CheckInvites()
        {
            if (!BotMain.IsRunning)
                return;

            if (ZetaDia.IsLoadingWorld)
                return;

            if (ZetaDia.Me != null && !ZetaDia.Me.IsValid)
                return;

            if (ZetaDia.IsInGame && ZetaDia.Me != null && ZetaDia.Me.IsValid && ZetaDia.Me.IsDead)
            {
                return;
            }

            if (GameUI.ElementIsVisible(GameUI.BattleNetOK, "BattleNetOK"))
            {
                GameUI.SafeCheckClickButtons();
                return;
            }

            const int recheckDuration = 500;

            if (!recheckInvites && DateTime.UtcNow.Subtract(lastInviteCheck).TotalMilliseconds < recheckDuration)
                return;

            lastInviteCheck = DateTime.UtcNow;

            if (ZetaDia.Service.Party.IsPartyFull)
            {
                recheckInvites = false;
                if (IsSocialWindowOpen)
                    CloseSocialWindow();
                return;
            }

            // Start inviting
            if (!IsSocialWindowOpen)
            {
                Logr.Debug("Opening Social Window");
                OpenSocialWindow();
                return;
            }

            if (IsSocialWindowOpen)
            {
                if (Settings.Instance.InviteFriend1 && GameUI.ElementIsVisible(Friend1InviteButton, "Friend 1 Invite"))
                {
                    GameUI.SafeClick(Friend1InviteButton, ClickDelay.NoDelay, "Friend 1 Invite", 750);
                    recheckInvites = true;
                    return;
                }
                if (Settings.Instance.InviteFriend2 && GameUI.ElementIsVisible(Friend2InviteButton, "Friend 2 Invite"))
                {
                    GameUI.SafeClick(Friend2InviteButton, ClickDelay.NoDelay, "Friend 2 Invite", 750);
                    recheckInvites = true;
                    return;
                }
                if (Settings.Instance.InviteFriend3 && GameUI.ElementIsVisible(Friend3InviteButton, "Friend 3 Invite"))
                {
                    GameUI.SafeClick(Friend3InviteButton, ClickDelay.NoDelay, "Friend 3 Invite", 750);
                    recheckInvites = true;
                    return;
                }
                if (Settings.Instance.InviteFriend3 && GameUI.ElementIsVisible(Friend3AlternateInviteButton, "Friend 3 Alternate Invite"))
                {
                    GameUI.SafeClick(Friend3AlternateInviteButton, ClickDelay.NoDelay, "Friend 3 Invite", 750);
                    recheckInvites = true;
                    return;
                }
                if (Settings.Instance.InviteFriend4 && GameUI.ElementIsVisible(Friend4InviteButton, "Friend 4 Invite"))
                {
                    GameUI.SafeClick(Friend4InviteButton, ClickDelay.NoDelay, "Friend 4 Invite", 750);
                    recheckInvites = true;
                    return;
                }
                if (Settings.Instance.InviteFriend4 && GameUI.ElementIsVisible(Friend4AlternateInviteButton, "Friend 4 Alternate Invite"))
                {
                    GameUI.SafeClick(Friend4AlternateInviteButton, ClickDelay.NoDelay, "Friend 4 Alternate Invite", 750);
                    recheckInvites = true;
                    return;
                }
                Logr.Debug("No more invites possible");
                recheckInvites = false;
                CloseSocialWindow();
            }
        }
    }
}