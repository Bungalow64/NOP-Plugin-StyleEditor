using System.Globalization;
using Nop.Core.Configuration;
using Nop.Plugin.Admin.StyleEditor.Helpers;

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

        /// <summary>
        /// Gets or sets the render type
        /// </summary>
        public virtual int RenderType { get; set; }

        /// <summary>
        /// The current version of the styles
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Updates the version
        /// </summary>
        /// <param name="currentDateTimeHelper"></param>
        public StyleEditorSettings UpdateVersion(ICurrentDateTimeHelper currentDateTimeHelper)
        {
            Version = currentDateTimeHelper.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);
            return this;
        }

        /// <summary>
        /// Gets the path to the custom styles, including version
        /// </summary>
        public string CustomStylesPath => $"/CustomStyle?v={Version}";
    }
}
