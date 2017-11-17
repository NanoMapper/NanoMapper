using NanoMapper.Core;
using System;

namespace NanoMapper.Extensions {

    /// <summary>
    /// Provides object entry point extensions that exposes the ApplyTo(...) mapping application method.
    /// </summary>
    public static class MappingExtensions {

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class where TTarget : class => ApplyTo(source, target, Mappings.GlobalContainer);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Action<IMapping<TSource, TTarget>> configure)
            where TSource : class where TTarget : class => ApplyTo(source, target, Mappings.GlobalContainer, configure);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container)
            where TSource : class where TTarget : class => ApplyTo(source, target, container, null);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container, Action<IMapping<TSource, TTarget>> configure)
            where TSource : class where TTarget : class {

            var mapping = container.GetMappingFor<TSource, TTarget>();

            if (configure != null) {
                configure(mapping);
            }

            mapping.Apply(source, target);
        }

    }
}
