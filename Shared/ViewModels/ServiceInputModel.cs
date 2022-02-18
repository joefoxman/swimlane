using System.Collections.Generic;

namespace Shared.ViewModels {
    public class ServiceInputModel {
        public string IpOrDomain { get; set; }
        public IEnumerable<string> Services { get; set; }
    }
}
