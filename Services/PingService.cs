using Contracts;
using Shared.ViewModels;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Services {
    public class PingService : IPingService {
        public async Task<PingModel> Run(string ipOrDomain) {
            var pingModel = new PingModel();
            var pinger = new Ping();
            var reply = await pinger.SendPingAsync(ipOrDomain);

            if (reply == null) { 
                return pingModel;
            }

            pingModel.Status = reply.Status.ToString();
            pingModel.Address = reply.Address.MapToIPv4().ToString();
            pingModel.RoundtripTime = reply.RoundtripTime;
            pingModel.Buffer = reply.Buffer;

            return pingModel;
        }
    }
}
