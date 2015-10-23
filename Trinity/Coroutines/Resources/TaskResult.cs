using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCoroutines.Resources
{
    public enum TaskResult
    {
        /// <summary>
        /// Successfully did what it was supposed to do, e.g. If task to move, did it reach the destination.
        /// </summary>
        Success,

        /// <summary>
        /// Was not able to do what its supposed to do, e.g. Task to craft something and didnt have enough materials.
        /// </summary>
        Failure,

        /// <summary>
        /// Task wants to be executed again.
        /// </summary>
        Repeat,

        /// <summary>
        /// Task wants itself and everything before it in hierarchy to be re-run.
        /// </summary>
        RepeatAll,
    }
}

