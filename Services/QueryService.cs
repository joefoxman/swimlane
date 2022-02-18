using Contracts;
using FluentValidation;
using Shared;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Services {
    public class QueryService : IQueryService {
        private readonly IGeoIpService _geoIpService;
        private readonly IRdapService _rdapService;
        private readonly IPingService _pingService;
        private readonly List<ServiceType> _defaultServices;
        private readonly AbstractValidator<ServiceInputModel> _serviceInputValidator;

        public QueryService(
            AbstractValidator<ServiceInputModel> serviceInputValidator,
            IGeoIpService geoIpService, 
            IRdapService rdapService,
            IPingService pingService) {

            _serviceInputValidator = serviceInputValidator;

            _geoIpService = geoIpService;
            _rdapService = rdapService;
            _pingService = pingService;

            _defaultServices = new List<ServiceType> {
                ServiceType.GeoIp
            };
        }

        public async Task<ServiceWrapperModel> RunServices(ServiceInputModel model) {
            var validationResult = _serviceInputValidator.Validate(model);
            if (!validationResult.IsValid) {
                // return an empty model
                return new ServiceWrapperModel();
            }

            var servicesWrapper = new ServiceWrapperModel();
            var tasks = new List<Task>();
            var ipOrDomain = model.IpOrDomain;
            var servicesToRun = model.Services.ParseEnums<ServiceType>().ToList();

            // will ensure that the default services will always run even if not in the list
            servicesToRun.AddRange(_defaultServices.Where(defaultService => !servicesToRun.Contains(defaultService)));

            Task<WhoIsModel> whoIsTask = null;
            Task<RdapModel> rdapTask = null;
            Task<PingModel> pingTask = null;

            // can do this foreach or contains on different rows
            foreach (var serviceToRun in servicesToRun) {
                // GeoPI, always run this one
                if (serviceToRun == ServiceType.GeoIp) {
                    whoIsTask = _geoIpService.Run(ipOrDomain);
                    tasks.Add(whoIsTask);
                }
                // RDAP
                else if (serviceToRun == ServiceType.Rdap) {
                    rdapTask = _rdapService.Run(ipOrDomain);
                    tasks.Add(rdapTask);
                }
                // PING
                else if (serviceToRun == ServiceType.Ping) {
                    pingTask = _pingService.Run(ipOrDomain);
                    tasks.Add(pingTask);
                }
            }

            // wait for all tasks to complete
            await Task.WhenAll(tasks);

            if (whoIsTask != null) {
                servicesWrapper.WhoIs = whoIsTask.Result;
            }
            if (rdapTask != null) {
                servicesWrapper.Rdap = rdapTask.Result;
            }
            if (pingTask != null) { 
                servicesWrapper.Ping = pingTask.Result;
            }

            return servicesWrapper;
        }
    }
}
