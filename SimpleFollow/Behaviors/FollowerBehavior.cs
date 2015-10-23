using System;
using SimpleFollow.Helpers;
using SimpleFollow.Network;
using SimpleFollow.Party;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Game;
using Zeta.TreeSharp;
using Action = Zeta.TreeSharp.Action;

namespace SimpleFollow.Behaviors
{
    /// <summary>
    /// Class FollowerComposite.
    /// </summary>
    public class FollowerComposite
    {
        /// <summary>
        /// Creates the behavior.
        /// </summary>
        /// <returns>Composite.</returns>
        internal static Composite CreateBehavior()
        {
            return new Decorator(ret => ZetaDia.Service.IsValid && ZetaDia.Service.Hero.IsValid,
                new Action(ret => FollowerOutOfGame())
                );
        }

        /// <summary>
        /// The last clicked out of game invite
        /// </summary>
        private static DateTime _lastClickedOutOfGameInvite = DateTime.MinValue;

        /// <summary>
        /// Gets the last clicked out of game invite.
        /// </summary>
        /// <value>The last clicked out of game invite.</value>
        public static DateTime LastClickedOutOfGameInvite
        {
            get { return _lastClickedOutOfGameInvite; }
            private set { _lastClickedOutOfGameInvite = value; }
        }

        /// <summary>
        /// Out of game check
        /// </summary>
        /// <returns>RunStatus.</returns>
        internal static RunStatus FollowerOutOfGame()
        {
            if (ZetaDia.Service.Hero == null)
            {
                Logr.Log("Error: ZetaDia.Service.Hero is null!");
                return RunStatus.Failure;
            }

            if (!ZetaDia.Service.Hero.IsValid)
            {
                Logr.Log("Error: ZetaDia.Service.Hero is invalid!");
                return RunStatus.Failure;
            }

            bool isPartyLeader = Social.IsPartyleader;

            if (!SimpleFollow.Enabled)
            {
                SharedComposites.CheckReplaceOutOfGameHook();
                return RunStatus.Failure;
            }

            FollowerService.AsyncClientUpdate();

            GameUI.SafeCheckClickButtons();

            if (ZetaDia.IsInGame)
            {
                return RunStatus.Success;
            }

            const int inviteWaitDelaySeconds = 3;

            if (DateTime.UtcNow.Subtract(_lastClickedOutOfGameInvite).TotalSeconds < inviteWaitDelaySeconds)
            {
                return RunStatus.Running;
            }

            if (GameUI.ElementIsVisible(GameUI.PartyInviteOK) && DateTime.UtcNow.Subtract(_lastClickedOutOfGameInvite).TotalMilliseconds > inviteWaitDelaySeconds)
            {
                _lastClickedOutOfGameInvite = DateTime.UtcNow;
                GameUI.SafeClick(GameUI.PartyInviteOK, ClickDelay.NoDelay, "Party Invite", 1500, true);
                GameEvents.FireWorldTransferStart();
                return RunStatus.Running;
            }

            if (Social.IsPartyleader && GameUI.ElementIsVisible(GameUI.OutOfGameLeavePartyButton) && DateTime.UtcNow.Subtract(_lastClickedOutOfGameInvite).TotalSeconds > inviteWaitDelaySeconds)
            {
                Logr.Log("We are party leader and out of game, leaving party to re-create");
                GameUI.SafeClick(GameUI.OutOfGameLeavePartyButton, ClickDelay.NoDelay, "Leave Party Button", 1000);
                return RunStatus.Running;
            }

            if (!isPartyLeader && SimpleFollow.Leader.IsInGame && GameUI.ElementIsVisible(GameUI.OutOfGameLeavePartyButton))
            {
                Logr.Log("We are Out of game, Leader is In Game - Leaving Party for re-invite");
                GameUI.SafeClick(GameUI.OutOfGameLeavePartyButton, ClickDelay.NoDelay, "Leave Party Button", 1000);
                return RunStatus.Running;
            }

            if (SimpleFollow.Leader.IsInGame && !isPartyLeader && GameUI.ElementIsVisible(GameUI.PartySlot2Icon, "Party Slot 2 Icon"))
            {
                Logr.Log("We are Out of game, Leader is In Game - Resuming game");
                GameUI.SafeClick(GameUI.PlayGameButton, ClickDelay.NoDelay, "Play Game Button", 1000);
                return RunStatus.Running;
            }

            if (isPartyLeader && !Social.IsInParty)
            {
                Logr.Log("Out of game, Waiting for Invite");
                return RunStatus.Running;
            }

            Logr.Log("Out of game, Waiting for leader");
            return RunStatus.Running;
        }
    }
}