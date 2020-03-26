using System.Threading.Tasks;

namespace PortClosedEmailer.Core.PortScanning
{
    public interface IPortScanner
    {
        Task<bool>  IsPortOpen  (string hostName, int portNumber);
        Task<bool>  IsPortOpen  (string hostNameWithPortNumber);
    }
}
