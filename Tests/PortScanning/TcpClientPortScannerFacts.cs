using FluentAssertions;
using PortClosedEmailer.Core.PortScanning;
using System;
using System.Threading.Tasks;
using Tests.Configuration;
using Xunit;

namespace Tests.PortScanning
{
    public class TcpClientPortScannerFacts
    {
        private const string SOLR_DEV = "solrdev.eastasia.cloudapp.azure.com";


        [Fact(DisplayName = "TcpClient scans port")]
        public async Task TcpClientscansport()
        {
            var cfg = new TestSettings();
            var sut = new TcpClientPortScanner1();
            (await sut.IsPortOpen(SOLR_DEV, 8983)).Should().BeTrue();
            (await sut.IsPortOpen(SOLR_DEV, 1111)).Should().BeFalse();
        }


        [Fact(DisplayName = "TcpClient detects death")]
        public async Task TcpClientdetectsdeath()
        {
            var sut = new TcpClientPortScanner1();
            (await sut.IsPortOpen(SOLR_DEV, 8983)).Should().BeFalse();
        }
    }
}
