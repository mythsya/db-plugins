using System.Linq;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.TreeSharp;
using Zeta.XmlEngine;
using Zeta.Common;
using Zeta.Common.Xml;
using Zeta.Bot;
using Zeta.Bot.Profile;
using Zeta.Bot.Profile.Composites;
using Zeta.Game.Internals.Actors;
using Zeta.Bot.Navigation;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;


namespace UberBot.Tags
{	
	public static class UberRun
	{
		public static int infernalMachineSNO = 257667;
		public static int infernalMachineBonesSNO = 366946;
		public static int infernalMachineEvilSNO = 366949;
		public static int infernalMachineGluttonySNO = 366947;
		public static int infernalMachineWarSNO = 366948;
		

		// Use Infernal Machine War
		public static void useInfernalMachine(string Machine)
		{
			
				foreach (ACDItem item in ZetaDia.Me.Inventory.Backpack)
				{				
					if (Machine == "Bones")
					{ 
						if (item.ActorSNO == infernalMachineBonesSNO)
						{
							// Use Item
							ZetaDia.Me.Inventory.UseItem(item.DynamicId);
							// Portal open
							
							Thread.Sleep(200);
						}
					}
					if (Machine == "War")
					{ 
						if (item.ActorSNO == infernalMachineWarSNO)
						{
							// Use Item
							ZetaDia.Me.Inventory.UseItem(item.DynamicId);
							// Portal open
							
							Thread.Sleep(200);
						}
					}
					if (Machine == "Evil")
					{ 
						if (item.ActorSNO == infernalMachineEvilSNO)
						{
							// Use Item
							ZetaDia.Me.Inventory.UseItem(item.DynamicId);
							// Portal open
							
							Thread.Sleep(200);
						}
					}
					if (Machine == "Gluttony")
					{ 
						if (item.ActorSNO == infernalMachineGluttonySNO)
						{
							// Use Item
							ZetaDia.Me.Inventory.UseItem(item.DynamicId);
							// Portal open
							
							Thread.Sleep(200);
						}
					}
				}
			
		}
	}
	
	[XmlElement("UberBotUse")]
	public class UberBotUse : ProfileBehavior
	{
		private bool m_IsDone = false;
		public static HashSet<string> sPreviousProfile = new HashSet<string>();
		public override bool IsDone
		{
			get
			{
				return m_IsDone;
			}
		}		
		protected override Composite CreateBehavior()
		{
			return new Zeta.TreeSharp.Action(ret =>
			{
				UberRun.useInfernalMachine(UseInfernalMachine); 
				Thread.Sleep(200);
				
				m_IsDone = true;
			});
		}

		[XmlAttribute("UseInfernalMachine")]
		public string UseInfernalMachine
		{
			get;
			set;
		}

		public override void ResetCachedDone()
		{
			m_IsDone = false;
			base.ResetCachedDone();
		}
	}
}
