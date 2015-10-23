using System;
using System.ServiceModel;
using SimpleFollow.Helpers;
using SimpleFollow.Party;

namespace SimpleFollow.Network
{
    /// <summary>
    /// This is the Web Service (WCF) API that is called by the Followers. This is hosted (where the below methods are executed) on the leader server.
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Server : IFollowService
    {
        /// <summary>
        /// This method is processed by the leader (e.g. followers invoke it, the leader receives it and executes the body below)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message GetUpdate()
        {
            try
            {
                Message m = SimpleFollow.Leader;
                //Logging.Write(m.ToString());
                return m;
            }
            catch (Exception ex)
            {
                Logr.Log("Exception in GetUpdate() {0}", ex);
                return new Message();
            }
        }

        /// <summary>
        /// This method is processed by the leader (e.g. followers invoke it, the leader receives it and executes the body below)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void SendUpdate(Message message)
        {
            if (message != null)
                LeaderService.Inbox.Enqueue(message);
            else throw new ArgumentNullException("Message cannot be null!");
        }
    }
}