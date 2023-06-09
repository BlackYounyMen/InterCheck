using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace Inter.Util
{
    public class AppSettingsHelper
    {
        static AppSettingsHelper()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

        public static IConfiguration Configuration { get; set; }
    }
}