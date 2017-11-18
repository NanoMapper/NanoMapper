
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NanoMapper.Core {

    /// <summary>
    /// Represents a generic mapping.
    /// </summary>
    public interface IMapping {

        /// <summary>
        /// Gets the bindings configured inside this mapping object.
        /// </summary>
        IDictionary<PropertyInfo, Delegate> Bindings { get; }
    }

}
