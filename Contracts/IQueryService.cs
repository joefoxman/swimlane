using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Contracts {
    public interface IQueryService {
        Task<ServiceWrapperModel> RunServices(ServiceInputModel model);
    }
}
