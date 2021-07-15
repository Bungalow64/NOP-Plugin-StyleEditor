using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Admin.StyleEditor.Settings;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Admin.StyleEditor
{
    /// <summary>
    /// Plugin class handling install/uninstall
    /// </summary>
    public class StyleEditorPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;

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
        /// <param name="permissionService"></param>
        public StyleEditorPlugin(
            ILocalizationService localizationService,
            WidgetSettings widgetSettings,
            ISettingService settingService,
            IWebHelper webHelper,
            IPermissionService permissionService)
        {
            _localizationService = localizationService;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
            _webHelper = webHelper;
            _permissionService = permissionService;
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
        public override void Install()
        {
             _settingService.SaveSetting(new StyleEditorSettings
            {
                DisableCustomStyles = false
            });

             _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Admin.StyleEditor.EditorTitle"] = "Style editor",
                ["Plugins.Admin.StyleEditor.StylesUpdated"] = "The styles have been updated",
                ["Plugins.Admin.StyleEditor.Configuration.Styles"] = "Custom styles",
                ["Plugins.Admin.StyleEditor.Configuration.Config"] = "Configuration",
                ["Plugins.Admin.StyleEditor.Configuration.CouldNotBeSaved"] = "The styles could not be saved",
                ["Plugins.Admin.StyleEditor.Configuration.FormatStyles"] = "Format styles",
                ["Plugins.Admin.StyleEditor.Configuration.DisableCustomStyles"] = "Disable custom styles",
                ["Plugins.Admin.StyleEditor.Configuration.DisableCustomStyles.Hint"] = "Hides the custom styles from the site",
                ["Plugins.Admin.StyleEditor.Configuration.CustomStyles"] = "Custom styles",
                ["Plugins.Admin.StyleEditor.Configuration.CustomStyles.Hint"] = "The custom styles to be used in the site, written in CSS",
                ["Plugins.Admin.StyleEditor.Configuration.RenderType"] = "Style loading type",
                ["Plugins.Admin.StyleEditor.Configuration.RenderType.Hint"] = "How the styles should be loaded in the browser.  Inline is recommended if you only have a small number of custom styles.",
                ["Plugins.Admin.StyleEditor.Configuration.Inline"] = "Inline",
                ["Plugins.Admin.StyleEditor.Configuration.Inline.Hint"] = "Loading the styles inline includes the custom styles within the page itself.  Best for when you only have a small number of custom styles, since it doesn't require an extra HTTP request, but does slightly increase the size of the pages being returned to visitors",
                ["Plugins.Admin.StyleEditor.Configuration.File"] = "File",
                ["Plugins.Admin.StyleEditor.Configuration.File.Hint"] = "Loading the styles via a file is how most stylesheets are loaded, but requires the browser to make an extra request to your site to get it.  However, this file is cached in the browser, so each visitor will only request this once.",
                ["Plugins.Admin.StyleEditor.Configuration.Asynchronous"] = "Load asynchronously (files only)",
                ["Plugins.Admin.StyleEditor.Configuration.Asynchronous.Hint"] = "Loads the custom styles in a way that doesn't block the rest of the page from rendering.  As a result, there might be a brief moment after the page loads before the custom styles are applied.  Only applies to the file loading type."
            });

            AddPlugin(StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES);

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override void Uninstall()
        {
            _settingService.DeleteSetting<StyleEditorSettings>();

            _localizationService.DeletePluginLocaleResources("Plugins.Admin.StyleEditor");

            RemovePlugin(StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES);

            base.Uninstall();
        }

        /// <summary>
        /// Gets the list of widget zones
        /// </summary>
        /// <returns></returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> {
                PublicWidgetZones.BodyEndHtmlTagBefore
            };
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
                var _ when widgetZone.Equals(PublicWidgetZones.BodyEndHtmlTagBefore) => StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES,
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Adds the menu item for the editor page
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
            {
                return;
            }

            var menuItem = new SiteMapNode
            {
                SystemName = "StyleEditor",
                Title = _localizationService.GetResource("Plugins.Admin.StyleEditor.EditorTitle"),
                ControllerName = "StyleEditor",
                ActionName = "EditStyles",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
                IconClass = "fa fa-dot-circle-o"
            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Configuration");
            if (pluginNode != null)
            {
                pluginNode.ChildNodes.Add(menuItem);
            }
            else
            {
                rootNode.ChildNodes.Add(menuItem);
            }
        }

        private void AddPlugin(string name)
        {
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(name))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(name);
                _settingService.SaveSetting(_widgetSettings);
            }
        }

        private void RemovePlugin(string name)
        {
            if (_widgetSettings.ActiveWidgetSystemNames.Contains(name))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(name);
                _settingService.SaveSetting(_widgetSettings);
            }
        }

        #endregion
    }
}
