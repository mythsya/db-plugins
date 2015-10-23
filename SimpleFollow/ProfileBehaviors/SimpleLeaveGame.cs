using SimpleFollow.UI;
using Zeta.TreeSharp;
using Zeta.XmlEngine;

namespace SimpleFollow.ProfileBehaviors
{
    [XmlElement("SimpleLeaveGame")]
    public class SimpleLeaveGame : Zeta.Bot.Profile.Common.LeaveGameTag
    {
        public SimpleLeaveGame()
        {
            StayInParty = Settings.Instance.StayInParty;
        }

        protected override Zeta.TreeSharp.Composite CreateBehavior()
        {
            return new Sequence(
                base.CreateBehavior()
                );
        }
    }
}