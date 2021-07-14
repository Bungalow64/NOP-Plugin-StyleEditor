using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Nop.Plugin.Admin.StyleEditor.Settings;

namespace Nop.Web.Controllers
{
    /// <summary>
    /// Handles the serving of custom styles
    /// </summary>
    public partial class CustomStyleController : BasePublicController
    {
        #region Fields

        private readonly StyleEditorSettings _settings;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="CustomStyleController"/>
        /// </summary>
        /// <param name="settings"></param>
        public CustomStyleController(StyleEditorSettings settings)
        {
            _settings = settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the custom styles
        /// </summary>
        /// <returns></returns>
        /// <remarks>If custom styles have been disabled, this will return an empty response</remarks>
        [ResponseCache(Duration = 31536000)]
        [HttpGet]
        public virtual IActionResult Index()
        {
            var mediaType = new MediaTypeHeaderValue("text/css");
            if (_settings.DisableCustomStyles)
            {
                return Content(string.Empty, mediaType);
            }
            return Content(_settings.CustomStyles, mediaType);
        }

        #endregion
    }
}