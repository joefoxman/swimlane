using Shared.ViewModels;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Contracts {
    public interface IPingService {
        Task<PingModel> Run(string ipOrDomain);
    }
}
