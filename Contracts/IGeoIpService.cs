using Shared.ViewModels;
using System.Threading.Tasks;

namespace Contracts {
    public interface IGeoIpService {
        Task<WhoIsModel> Run(string ipOrDomain);
    }
}
