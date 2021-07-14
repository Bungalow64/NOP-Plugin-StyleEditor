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
    public class StyleEditorPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        /// <summary>
        /// Whether the widget should be hidden
        /// </summary>
        public bool HideInWidgetList => true;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="StyleEditorPlugin"/>
        /// </summary>
        /// <param name="localizationService"></param>
        /// <param name="widgetSettings"></param>
        /// <param name="settingService"></param>
        /// <param name="webHelper"></param>
        public StyleEditorPlugin(
            ILocalizationService localizationService,
            WidgetSettings widgetSettings,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _widgetSettings = widgetSettings;
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

            await AddPluginAsync(StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES);

            await base.InstallAsync();
        }

        /// <summary>
        /// Updates the plugin
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <param name="targetVersion"></param>
        /// <returns></returns>
        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            await AddPluginAsync(StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES);

            await base.UpdateAsync(currentVersion, targetVersion);
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<StyleEditorSettings>();

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Admin.StyleEditor");

            await RemovePluginAsync(StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES);

            await base.UninstallAsync();
        }

        /// <summary>
        /// Gets the list of widget zones
        /// </summary>
        /// <returns></returns>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> {
                PublicWidgetZones.Footer
            });
        }

        /// <summary>
        /// Gets the name of the widget to use
        /// </summary>
        /// <param name="widgetZone">The widget zone specified</param>
        /// <returns>The name of the widget</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return widgetZone switch
            {
                var _ when widgetZone.Equals(PublicWidgetZones.Footer) => StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES,
                _ => string.Empty,
            };
        }

        private async Task AddPluginAsync(string name)
        {
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(name))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(name);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }
        }

        private async Task RemovePluginAsync(string name)
        {
            if (_widgetSettings.ActiveWidgetSystemNames.Contains(name))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(name);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }
        }

        #endregion
    }
}
