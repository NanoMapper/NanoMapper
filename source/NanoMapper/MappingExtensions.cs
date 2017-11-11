using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NanoMapper {

    /// <summary>
    /// Provides object entry point extensions that exposes the ApplyTo(...) mapping application method.
    /// </summary>
    public static class MappingExtensions {
        
        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class where TTarget : class
            => ApplyTo(source, target, Mapper.GlobalInstance);
        
        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Action<IMappingConfiguration<TSource, TTarget>> configure)
            where TSource : class where TTarget : class
            => ApplyTo(source, target, Mapper.GlobalInstance, configure);
        
        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMapper mapper) {

        }
        
        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMapper mapper, Action<IMappingConfiguration<TSource, TTarget>> configure) {

            //if (!mapper.Mappings.TryGetValue(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), out object mappingConfigEntry)) {

            //}



            //if (source.Equals(target)) {
            //    return;
            //}

            //var sourceType = source.GetType().GetTypeInfo();
            //var targetType = target.GetType().GetTypeInfo();

            //var sourceProps = sourceType.GetProperties().Where(p => p.CanRead);
            //var targetProps = targetType.GetProperties().Where(p => p.CanWrite);
            //var properties = sourceProps.Intersect(targetProps, new ComparePropertiesByNameAndType()).ToList();

            //var sourceFields = sourceType.GetFields();
            //var targetFields = targetType.GetFields();
            //var fields = sourceFields.Intersect(targetFields, new CompareFieldsByNameAndType()).ToList();

            //Copy(source, target, properties, fields);
        }

        //private static void Copy(object source, object target, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields) {
        //    foreach (var property in properties) {
        //        property.SetValue(target, property.GetValue(source));
        //    }

        //    foreach (var field in fields) {
        //        field.SetValue(target, field.GetValue(source));
        //    }
        //}

    }

}
