using FluentValidation;
using Nop.Plugin.Admin.StyleEditor.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Admin.StyleEditor.Areas.Admin.Validators
{
    /// <summary>
    /// Represents configuration model validator
    /// </summary>
    public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
    {
        #region Ctor

        /// <summary>
        /// Creates the validator for the <see cref="ConfigurationModel"/>
        /// </summary>
        /// <param name="localizationService"></param>
        public ConfigurationValidator(ILocalizationService localizationService)
        {
        }

        #endregion
    }
}