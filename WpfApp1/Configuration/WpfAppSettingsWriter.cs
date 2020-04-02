using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1.Configuration
{
    public partial class WpfAppSettings
    {
        public Task SaveCurrentValues()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(JSON_FILE, json);
            return Task.FromResult(0);
        }
    }
}
