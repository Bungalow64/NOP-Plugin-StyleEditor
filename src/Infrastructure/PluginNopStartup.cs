using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Admin.StyleEditor.Helpers;

namespace Nop.Plugin.Admin.StyleEditor.Infrastructure
{
    /// <summary>
    /// Handles the startup of the plugin
    /// </summary>
    public class PluginNopStartup : INopStartup
    {
        /// <summary>
        /// Configures the services required by the plugin
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICurrentDateTimeHelper, CurrentDateTimeHelper>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
        }

        /// <summary>
        /// Configures the application
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// The order of the startup
        /// </summary>
        public int Order => 11;
    }
}