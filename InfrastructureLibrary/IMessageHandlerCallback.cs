using System.Threading.Tasks;

namespace InfrastructureLibrary
{
    public interface IMessageHandlerCallback
    {
        Task<bool> HandleMessageAsync(string messageType, string message);
    }
}