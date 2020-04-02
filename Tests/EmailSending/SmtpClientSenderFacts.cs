using PortClosedEmailer.Core.EmailSending;
using System.Threading.Tasks;
using Tests.Configuration;
using Xunit;

namespace Tests.EmailSending
{
    public class SmtpClientSenderFacts
    {
        [InlineData("cred1.txt")]
        [InlineData("cred2.txt")]
        [Theory(DisplayName = "Sends email")]
        public Task Sendsemail(string credsFilename)
        {
            var cfg = new TestSettings(credsFilename);
            var sut = new SmtpClientSender1(cfg, null);
            return sut.SendPortClosedAlert("samplehost:123");
        }
    }
}
