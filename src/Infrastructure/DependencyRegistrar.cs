using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Admin.StyleEditor.Helpers;

namespace Nop.Plugin.Admin.StyleEditor.Infrastructure
{
    /// <summary>
    /// Registers the plugin dependencies
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddSingleton<ICurrentDateTimeHelper, CurrentDateTimeHelper>();
        }

        /// <summary>
        /// Order of this registration
        /// </summary>
        public int Order => 1;
    }
}
