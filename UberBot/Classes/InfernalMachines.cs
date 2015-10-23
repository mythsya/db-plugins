using UberBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Zeta.Common;
using Zeta.Game;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
using Zeta.Game.Internals.Actors;

namespace UberBot.Classes
{
    public class InfernalMachines
    {
        public static readonly List<int> InfernalMachinesSNOs = new List<int> { 366946, 366947, 366948, 366949 };

		public static List<int> InfernalMachinesCount = new List<int> { 0, 0, 0, 0 };
		
        public static int BonesCount = 0;
		public static int GluttonyCount = 0;		
        public static int WarCount = 0;
        public static int EvilCount = 0;

        public static bool IsInfernalMachineSNO(int sno)
        {
            return InfernalMachinesSNOs.Any(k => k == sno);
        }

        public static void AddToInfernalMachinesCount(int sno, int increment)
        {
            if (!IsInfernalMachineSNO(sno) || increment < 1) return;

            switch (InfernalMachinesSNOs.IndexOf(sno))
            {
                case 0:
					BonesCount += increment;
					InfernalMachinesCount[InfernalMachinesSNOs.IndexOf(sno)] = BonesCount;
                    break;
                case 1:
					GluttonyCount += increment;
					InfernalMachinesCount[InfernalMachinesSNOs.IndexOf(sno)] = GluttonyCount;
                    break;
                case 2:
					WarCount += increment;
					InfernalMachinesCount[InfernalMachinesSNOs.IndexOf(sno)] = WarCount;
                    break;
                case 3:
					EvilCount += increment;
					InfernalMachinesCount[InfernalMachinesSNOs.IndexOf(sno)] = EvilCount;
                    break;
            }
        }

        public static void Refresh()
        {
			try
			{
				BonesCount = 0;
				GluttonyCount = 0;
				WarCount = 0;
				EvilCount = 0;

                InfernalMachinesCount[0] = BonesCount;
                InfernalMachinesCount[1] = GluttonyCount;
                InfernalMachinesCount[2] = WarCount;
                InfernalMachinesCount[3] = EvilCount;

				ZetaDia.Me.Inventory.Backpack
					.Where(i => IsInfernalMachineSNO(i.ActorSNO) &&
                        i.InternalName.Contains("InfernalMachine_"))
					.ForEach(i => AddToInfernalMachinesCount(i.ActorSNO, (int)i.ItemStackQuantity));
			}
			catch { }
        }
    }
}
