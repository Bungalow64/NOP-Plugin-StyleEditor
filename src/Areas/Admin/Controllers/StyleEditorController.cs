using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Admin.StyleEditor.Areas.Admin.Models;
using Nop.Plugin.Admin.StyleEditor.Helpers;
using Nop.Plugin.Admin.StyleEditor.Settings;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Admin.StyleEditor.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for handling actions to configure and view the StyleEditor widget
    /// </summary>
    public class StyleEditorController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly StyleEditorSettings _settings;
        private readonly ICurrentDateTimeHelper _currentDateTimeHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of the <see cref="StyleEditorController"/>
        /// </summary>
        /// <param name="permissionService"></param>
        /// <param name="notificationService"></param>
        /// <param name="localizationService"></param>
        /// <param name="settingService"></param>
        /// <param name="settings"></param>
        /// <param name="currentDateTimeHelper"></param>
        public StyleEditorController(
            IPermissionService permissionService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            ISettingService settingService,
            StyleEditorSettings settings,
            ICurrentDateTimeHelper currentDateTimeHelper)
        {
            _permissionService = permissionService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _settingService = settingService;
            _settings = settings;
            _currentDateTimeHelper = currentDateTimeHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Redirects to the page showing the configuration options for the plugin
        /// </summary>
        /// <returns>Returns the configuration page</returns>
        public virtual async Task<IActionResult> Configure()
        {
            return await EditStyles();
        }

        /// <summary>
        /// Gets the page showing the configuration options for the plugin
        /// </summary>
        /// <returns>Returns the configuration page</returns>
        public virtual async Task<IActionResult> EditStyles()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
            {
                return AccessDeniedView();
            }

            var model = new ConfigurationModel
            {
                DisableCustomStyles = _settings.DisableCustomStyles,
                CustomStyles = _settings.CustomStyles,
                RenderType = _settings.RenderType,
                UseAsync = _settings.UseAsync
            };

            return View("~/Plugins/Admin.StyleEditor/Areas/Admin/Views/StyleEditor.cshtml", model);
        }

        /// <summary>
        /// Updates the configuration settings for the plugin
        /// </summary>
        /// <param name="model">The updated settings</param>
        /// <returns>Returns the configuration page</returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public virtual async Task<IActionResult> EditStyles(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
            {
                return AccessDeniedView();
            }

            if (!ModelState.IsValid)
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Plugins.Admin.StyleEditor.Configuration.CouldNotBeSaved"));
                return await Configure();
            }

            _settings.DisableCustomStyles = model.DisableCustomStyles;
            _settings.CustomStyles = model.CustomStyles;
            _settings.RenderType = model.RenderType;
            _settings.UseAsync = model.UseAsync;
            _settings.UpdateVersion(_currentDateTimeHelper);

            await _settingService.SaveSettingAsync(_settings);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Admin.StyleEditor.StylesUpdated"));

            return await EditStyles();
        }

        #endregion
    }
}
