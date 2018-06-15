using System.Threading.Tasks;

namespace Lykke.AlgoStore.Service.Security.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
