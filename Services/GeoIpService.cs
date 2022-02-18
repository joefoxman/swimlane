using Contracts;
using Shared.ViewModels;
using System.Threading.Tasks;

namespace Services {
    public class GeoIpService : IGeoIpService {
        private readonly IApiService _apiService;

        public GeoIpService(IApiService apiService) {
            _apiService = apiService;
        }

        public async Task<WhoIsModel> Run(string ipOrDomain) {
            var returnObject = new WhoIsModel();
            var url = $"https://ipwhois.app/json/{ipOrDomain}";
            var whoIs = await _apiService.GetDataSingle<WhoIsModel>(url);
            if (whoIs == null) {
                // TODO: Log this as not found
                return returnObject;
            }
            return whoIs;
        }
    }
}
