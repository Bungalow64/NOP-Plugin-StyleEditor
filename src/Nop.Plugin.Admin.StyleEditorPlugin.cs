using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Admin.StyleEditor.Settings;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace Nop.Plugin.Admin.StyleEditor
{
    /// <summary>
    /// Plugin class handling install/uninstall
    /// </summary>
    public class StyleEditorPlugin : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="StyleEditorPlugin"/>
        /// </summary>
        /// <param name="localizationService"></param>
        /// <param name="settingService"></param>
        /// <param name="webHelper"></param>
        public StyleEditorPlugin(
            ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/StyleEditor/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            await _settingService.SaveSettingAsync(new StyleEditorSettings
            {
                DisableCustomStyles = false
            });

            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Admin.StyleEditor.Configuration.FormatStyles"] = "Format styles",
                ["Plugins.Admin.StyleEditor.Configuration.DisableCustomStyles"] = "Disable custom styles",
                ["Plugins.Admin.StyleEditor.Configuration.DisableCustomStyles.Hint"] = "Hides the custom styles from the site",
                ["Plugins.Admin.StyleEditor.Configuration.CustomStyles"] = "Custom styles",
                ["Plugins.Admin.StyleEditor.Configuration.CustomStyles.Hint"] = "The custom styles to be used in the site, written in CSS"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<StyleEditorSettings>();

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Admin.StyleEditor");

            await base.UninstallAsync();
        }

        #endregion
    }
}
