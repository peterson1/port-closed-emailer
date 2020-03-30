using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.PortScanning
{
    public class TcpClientPortScanner1 : IPortScanner
    {
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(2);


        public async Task<bool> IsPortOpen(string host, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    var res = client.BeginConnect(host, port, null, null);
                    var ok  = await Task.Run(() => res.AsyncWaitHandle.WaitOne(TimeOut));
                    if (ok) client.EndConnect(res);
                    return ok;
                }
                catch 
                {
                    return false;
                }
            }
        }


        public Task<bool> IsPortOpen(string hostWithPort)
        {
            if (!hostWithPort.Contains(":"))
                throw BadArg("Hostname and port number should be separated by “:”", hostWithPort);

            var ss = hostWithPort.Split(':');

            if (ss.Length != 2)
                throw BadArg("Invalid hostname with port format:", hostWithPort);

            if (!int.TryParse(ss[1], out int portNum))
                throw BadArg($"Non-numeric port number: “{ss[1]}”", hostWithPort);

            return IsPortOpen(ss[0], portNum);
        }


        private ArgumentException BadArg(string whyBad, string arg)
        {
            var msg = $"{whyBad}{Environment.NewLine}{arg}";
            return new ArgumentException(msg);
        }
    }
}
