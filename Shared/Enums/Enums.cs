using System.ComponentModel;

namespace Shared.Enums {
    public class Enums {
        public enum ServiceType {
            [Description("GeoIP")]
            GeoIp = 1,

            [Description("RDAP")]
            Rdap = 2,

            [Description("Ping")]
            Ping = 3,

            [Description("ReverseDns")]
            ReverseDns = 4
        }
    }
}
