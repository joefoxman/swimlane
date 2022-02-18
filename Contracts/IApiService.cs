using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts {
    public interface IApiService {
        Task<IEnumerable<T>> GetDataList<T>(string url);
        Task<T> GetDataSingle<T>(string url);
        Task<string> GetSingle(string url);
        Task<TS> Post<T, TS>(string url, T model);
        Task<TS> Delete<T, TS>(string url, T model);
        Task<TS> Delete<TS>(string url);
    }
}
