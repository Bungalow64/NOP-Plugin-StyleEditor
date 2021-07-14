using Nop.Core.Configuration;

namespace Nop.Plugin.Admin.StyleEditor.Settings
{
    /// <summary>
    /// The settings of the plugin
    /// </summary>
    public partial class StyleEditorSettings : ISettings
    {
        /// <summary>
        /// Gets or sets whether the custom styles should be disabled
        /// </summary>
        public virtual bool DisableCustomStyles { get; set; }

        /// <summary>
        /// Gets or sets the custom styles
        /// </summary>
        public virtual string CustomStyles { get; set; }
    }
}
