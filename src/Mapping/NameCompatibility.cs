using System;
using System.Collections.Generic;
using Nop.Data.Mapping;

namespace Nop.Plugin.Admin.StyleEditor.Mapping
{
    /// <summary>
    /// Backward compatibility of table naming
    /// </summary>
    public partial class NameCompatibility : INameCompatibility
    {
        /// <inheritdoc/>
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>();

        /// <inheritdoc/>
        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>();
    }
}