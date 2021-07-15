using System;

namespace Nop.Plugin.Admin.StyleEditor.Helpers
{
    /// <summary>
    /// Provides access to the current date/time
    /// </summary>
    public interface ICurrentDateTimeHelper
    {
        /// <summary>
        /// The current date/time object, in UTC
        /// </summary>
        public DateTime UtcNow { get; }
    }
}
