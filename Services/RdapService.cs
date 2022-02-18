using Contracts;
using Shared;
using Shared.ViewModels;
using System.Threading.Tasks;

namespace Services {
    public class RdapService : IRdapService {
        private readonly IApiService _apiService;

        public RdapService(IApiService apiService) {
            _apiService = apiService;
        }

        public async Task<RdapModel> Run(string ipOrDomain) {
            // RDAP
            string url;
            if (ipOrDomain.IsIpAddress()) {
                url = $"https://rdap.arin.net/registry/ip/{ipOrDomain}";
            }
            else {
                // assume domain name
                url = $"https://www.rdap.net/domain/{ipOrDomain}";
            }
            var data = await _apiService.GetSingle(url);
            if (data == null) {
                // TODO: Log this as not found
                return new RdapModel();
            }
            return new RdapModel { Data = data };
        }
    }
}
