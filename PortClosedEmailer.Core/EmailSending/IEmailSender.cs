using System;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.EmailSending
{
    public interface IEmailSender
    {
        event EventHandler<Exception> SendingError;
        Task  SendPortClosedAlert  (string hostName);
    }
}
