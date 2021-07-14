using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Admin.StyleEditor.Areas.Admin.Models
{
    /// <summary>
    /// The model containing the configuration settings for the plugin
    /// </summary>
    public partial record ConfigurationModel : BaseNopModel
    {
        /// <summary>
        /// Whether the custom styles should be disabled
        /// </summary>
        [NopResourceDisplayName("Plugins.Admin.StyleEditor.Configuration.DisableCustomStyles")]
        public bool DisableCustomStyles { get; set; }

        /// <summary>
        /// The custom styles
        /// </summary>
        [NopResourceDisplayName("Plugins.Admin.StyleEditor.Configuration.CustomStyles")]
        public string CustomStyles { get; set; }
    }
}
