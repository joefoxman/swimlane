namespace Shared.ViewModels {
    public class PingModel {
        public string Address { get; set; }
        public byte[] Buffer { get; set; }

        public long RoundtripTime { get; set; }

        public string Status { get; set; }
    }
}
