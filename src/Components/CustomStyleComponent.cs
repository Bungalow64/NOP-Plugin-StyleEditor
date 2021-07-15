using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Admin.StyleEditor.Settings;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Admin.StyleEditor.Components
{
    /// <summary>
    /// The widget that includes the custom styles
    /// </summary>
    [ViewComponent(Name = StyleEditorPluginDefaults.WIDGETS_CUSTOM_STYLES)]
    public class CustomStyleComponent : NopViewComponent
    {
        private readonly StyleEditorSettings _settings;

        /// <summary>
        /// Creates a new <see cref="CustomStyleComponent"/>
        /// </summary>
        public CustomStyleComponent(StyleEditorSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Invokes the widget
        /// </summary>
        /// <returns>Returns the view of the widget</returns>
        public IViewComponentResult Invoke()
        {
            if (_settings.DisableCustomStyles)
            {
                return Content("");
            }

            if (_settings.RenderType == 2)
            {
                var template = _settings.UseAsync ? "CustomStylesAsync" : "CustomStylesLink";
                return View($"~/Plugins/Admin.StyleEditor/Views/{template}.cshtml", _settings.CustomStylesPath);
            }
            else
            {
                var styles = _settings.CustomStyles;

                return View("~/Plugins/Admin.StyleEditor/Views/CustomStyles.cshtml", styles);
            }
        }
    }
}
