using System;
using System.Reflection;

namespace NanoMapper.Exceptions {

    /// <summary>
    /// Thrown when attempting to map to a read-only target property.
    /// </summary>
    public class ReadOnlyPropertyException : InvalidOperationException {

        /// <summary>
        /// Throw when attempting to map to a read-only target property.
        /// </summary>
        public ReadOnlyPropertyException(PropertyInfo propertyInfo)
            : base($"Property '{propertyInfo.Name}' cannot be used as a target mapping because it is read-only.") { }

    }

}
