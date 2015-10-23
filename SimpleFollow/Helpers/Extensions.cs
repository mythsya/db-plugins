using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Game.Internals.Actors;

namespace SimpleFollow.Helpers
{
    public static class Extensions
    {
        public static bool IsFullyValid(this DiaObject diaObject)
        {
            return diaObject != null && diaObject.IsValid && diaObject.ACDGuid != 0 && diaObject.CommonData != null && diaObject.CommonData.IsValid;
        }
    }
}
