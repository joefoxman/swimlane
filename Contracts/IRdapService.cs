using Shared.ViewModels;
using System.Threading.Tasks;

namespace Contracts {
    public interface IRdapService {
        Task<RdapModel> Run(string ipOrDomain);
    }
}
