using System.Collections.Generic;
using System.Reflection;

namespace NanoMapper.Comparers {

    /// <summary>
    /// Used to compare properties by means of their name and type assign-ability
    /// </summary>
    public class ComparePropertyByNameAndType : IEqualityComparer<PropertyInfo> {

        /// <summary>
        /// Compares the source and target properties to see if they are equal.
        /// </summary>
        public bool Equals(PropertyInfo source, PropertyInfo target) {
            if (object.Equals(source, target)) {
                return true;
            }

            return source.Name.Equals(target.Name) && target.PropertyType.GetTypeInfo().IsAssignableFrom(source.PropertyType);
        }

        /// <summary />
        public int GetHashCode(PropertyInfo obj) {
            return obj.Name.GetHashCode();
        }
    }
}
