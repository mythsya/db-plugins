using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zeta.Game.Internals.Actors;
using Trinity.Technicals;
using Zeta.Game;
using Zeta.Common;
using Logger = Trinity.Technicals.Logger;

namespace Trinity.Items
{
    public static class ItemDropper
    {
        public static HashSet<int> DroppedItems = new HashSet<int>();

        /// <summary>
        /// Drop item in town and record it so we can avoid picking it up again.
        /// </summary>
        public static bool Drop(ACDItem item)
        {
            if (!ZetaDia.IsInGame || !ZetaDia.IsInTown || item.IsAccountBound)
                return false;

            if (item.IsPotion || item.IsMiscItem || item.IsGem || item.IsCraftingReagent || item.IsCraftingPage)
                return false;

            Logger.Log("--> Dropping {0} ({1}) in town. DynamicId={2} ", item.Name, item.ActorSNO, item.DynamicId);

            if (item.Drop())
            {
                DroppedItems.Add(item.DynamicId);
                Thread.Sleep(25);
                return true;
            }

            return false;
        }
    }
}

