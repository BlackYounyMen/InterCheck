using Microsoft.Extensions.Configuration;
using System.IO;

namespace InterFace
{
    /// <summary>
    /// appsettings里面的帮助类
    /// </summary>
    public class ConfigHelper
    {
        private static IConfiguration configuration;

        static ConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();
        }

        /// <summary>
        /// 查詢Appsettings.json里的配置
        /// </summary>
        /// <param name="section">第一層的key值</param>
        /// <param name="key">第二層的key值</param>
        /// <returns></returns>
        public static string GetSection(string section, string key)
        {
            return configuration.GetSection(section).GetSection(key).Value;
        }

        /// <summary>
        /// 查詢Appsettings.json里的配置
        /// </summary>
        /// <param name="key">第二層的key值</param>
        /// <returns></returns>
        public static string GetSection(string key)
        {
            return configuration.GetSection(key).Value;
        }

        /// <summary>
        /// 查詢Appsettings.json里的Config配置
        /// </summary>
        public static string GetConfig(string key)
        {
            return configuration.GetSection("Config").GetSection(key).Value;
        }

        /// <summary>
        /// 查詢Appsettings.json里的ConnectionStrings配置
        /// </summary>
        public static string GetConnectionString(string key)
        {
            return configuration.GetConnectionString(key);
        }
    }
}