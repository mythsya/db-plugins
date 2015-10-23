using System.ServiceModel;
using SimpleFollow.Party;

namespace SimpleFollow.Network
{
    [ServiceContract]
    internal interface IFollowService
    {
        [OperationContract]
        Message GetUpdate();

        [OperationContract]
        void SendUpdate(Message message);
    }
}