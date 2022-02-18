using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.ViewModels;
using System.Collections.Generic;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Services;
using Shared.Validators;

namespace Tests {
    [TestClass]
    public class QueryServiceTests {
        [TestMethod]
        [TestCategory(Shared.Constants.UNIT)]
        public void RunOneServicePass() {
            var queryService = new Mock<IQueryService>();
            var serviceWrapperModel = new ServiceWrapperModel();
            var model = new ServiceInputModel {
                IpOrDomain = "google.com",
                Services = new List<string>{ "rdap" } 
            };
            queryService.Setup(p => p.RunServices(It.IsAny<ServiceInputModel>())).ReturnsAsync(serviceWrapperModel);
            var servicesController = GetServicesController(queryService.Object);
            var actionResult = servicesController.RunServices(model).Result;
            var result = actionResult as OkObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [TestCategory(Shared.Constants.INTEGRATION)]
        public void RunServicesFail() {
            var queryService = GetQueryService();
            var model = new ServiceInputModel {
                IpOrDomain = "google.com",
                Services = new List<string> { "NoService" }
            };

            var results = queryService.RunServices(model).Result;
            Assert.IsTrue(results.Rdap == null);
        }

        [TestMethod]
        [TestCategory(Shared.Constants.INTEGRATION)]
        public void RunServicesGeoIpPass() {
            var queryService = GetQueryService();
            var model = new ServiceInputModel {
                IpOrDomain = "google.com",
                Services = new List<string> { "GeoIp" }
            };

            var results = queryService.RunServices(model).Result;
            Assert.IsTrue(results.WhoIs != null);
        }
        [TestMethod]
        [TestCategory(Shared.Constants.INTEGRATION)]
        public void RunServicesRdapPass() {
            var queryService = GetQueryService();
            var model = new ServiceInputModel {
                IpOrDomain = "google.com",
                Services = new List<string> { "Rdap" }
            };

            var results = queryService.RunServices(model).Result;
            Assert.IsTrue(results.Rdap != null);
        }

        [TestMethod]
        [TestCategory(Shared.Constants.INTEGRATION)]
        public void RunServicesPingPass() {
            var queryService = GetQueryService();
            var model = new ServiceInputModel {
                IpOrDomain = "google.com",
                Services = new List<string> { "Ping" }
            };

            var results = queryService.RunServices(model).Result;
            Assert.IsTrue(results.Ping != null);
        }

        private static QueryService GetQueryService() {
            var ipService = new ApiService();
            var geoIpService = new GeoIpService(ipService);
            var rdapService = new RdapService(ipService);
            var pingService = new PingService();
            var validator = new ServiceModelValidator();
            var queryService = new QueryService(validator, geoIpService, rdapService, pingService);
            return queryService;
        }

        private static ServicesController GetServicesController(IQueryService queryService) {
            if (queryService == null) {
                queryService = new Mock<IQueryService>().Object;
            }

            var servicesController = new ServicesController(queryService);
            return servicesController;
        }
    }
}