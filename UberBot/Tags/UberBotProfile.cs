using System.IO;
using Zeta.Bot;
using Zeta.Bot.Profile;
using Zeta.TreeSharp;
using Zeta.XmlEngine;
using UberBot.Helpers;

namespace UberBot.Tags
{
    [XmlElement("UberBotProfile")]
	public class UberBotProfile : ProfileBehavior
	{
		private bool _mIsDone;
		public override bool IsDone
		{
			get
			{
				ProfileHelper.DataPath = Path.GetDirectoryName(Zeta.Bot.Settings.GlobalSettings.Instance.LastProfile);
				ProfileHelper.XmlLoaderProfile = ProfileManager.CurrentProfile.Path;
				ProfileHelper.XmlLeoricsRegretProfile = LeoricsRegretProfile;
				ProfileHelper.XmlVialofPutridnessProfile = VialofPutridnessProfile;
				ProfileHelper.XmlIdolofTerrorProfile = IdolofTerrorProfile;
				ProfileHelper.XmlHeartofEvilProfile = HeartofEvilProfile;
				
				Logging.Log("Profile Manager, Looking for next step");
				ProfileHelper.UberBotProfileManager();
				
				return _mIsDone;
			}
		}
		protected override Composite CreateBehavior()
		{
			return new Action(ret =>
			{
				_mIsDone = true;
			});
		}
		[XmlAttribute("LeoricsRegretProfile")]
		public string LeoricsRegretProfile
		{
			get;
			set;
		}
		[XmlAttribute("VialofPutridnessProfile")]
		public string VialofPutridnessProfile
		{
			get;
			set;
		}
		[XmlAttribute("IdolofTerrorProfile")]
		public string IdolofTerrorProfile
		{
			get;
			set;
		}
		[XmlAttribute("HeartofEvilProfile")]
		public string HeartofEvilProfile
		{
			get;
			set;
		}
		public override void ResetCachedDone()
		{
			_mIsDone = false;
			base.ResetCachedDone();
		}
	}
}
