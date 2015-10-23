using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Zeta.Bot.Navigation;
using Zeta.Common;

namespace SimpleFollow.Helpers
{
    public static class NavHelper
    {
        private static Coroutine _navigateToCoroutine;
        internal static MoveResult NavigateTo(Vector3 destination, string destinationName = "")
        {
            //if (Navigator.SearchGridProvider.Width == 0)
            //{
            //    Logger.Verbose("Navigation is Currently Unavailable");
            //    return MoveResult.PathGenerationFailed;
            //}

            if (_navigateToCoroutine == null || _navigateToCoroutine.IsFinished)
                _navigateToCoroutine = new Coroutine(async () => await Navigator.MoveTo(destination, destinationName));

            _navigateToCoroutine.Resume();

            if (_navigateToCoroutine.Status == CoroutineStatus.RanToCompletion)
                return (MoveResult)_navigateToCoroutine.Result;

            return MoveResult.Moved;
        }
    }
}
