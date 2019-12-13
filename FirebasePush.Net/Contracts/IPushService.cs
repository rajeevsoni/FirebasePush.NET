using FirebasePush.Net.Messages;
using System.Threading.Tasks;

namespace FirebasePush.Net.Contracts
{
    public interface IPushService
    {
        Task<PushResponse> Push(PushRequest requestMessage);
    }
}
