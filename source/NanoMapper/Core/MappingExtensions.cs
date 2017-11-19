using System;

namespace NanoMapper {

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
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Action<Mapping<TSource, TTarget>> configure)
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
        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container, Action<Mapping<TSource, TTarget>> configure)
            where TSource : class where TTarget : class {

            // Do nothing is either object is null
            if (source == null || target == null) {
                return;
            }

            var mapping = container.GenerateMappingFor<TSource, TTarget>();
            
            configure?.Invoke(mapping);

            mapping.Apply(source, target);
        }

    }
}
