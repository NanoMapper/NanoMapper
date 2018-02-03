using System;

namespace NanoMapper {

    /// <summary>
    /// Provides object entry point extensions that exposes the Map(...) mapping application method.
    /// </summary>
    public static class MappingExtensions {

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class where TTarget : class
            => Map(source, target, Mapper.GlobalContainer);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target, Action<Mapping<TSource, TTarget>> map)
            where TSource : class where TTarget : class
            => Map(source, target, Mapper.GlobalContainer, map);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// </summary>
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container)
            where TSource : class where TTarget : class
            => Map(source, target, container, null);

        /// <summary>
        /// Applies all applicable property values from the source object onto the target object
        /// using the specified mapping overrides.
        /// </summary>
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container, Action<Mapping<TSource, TTarget>> map)
            where TSource : class where TTarget : class {

            // Do nothing if either object is null
            if (source == null || target == null) {
                return target;
            }

            var mapping = container.GenerateMappingFor<TSource, TTarget>();
            
            map?.Invoke(mapping);

            mapping.Apply(source, target);

            return target;
        }
        
        #region Depreciated
        
        /// <see cref="Map{TSource,TTarget}(TSource,TTarget)"/>
        [Obsolete("Use Map(...) instead")]
        public static TTarget ApplyTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class where TTarget : class
            => Map(source, target, Mapper.GlobalContainer);
        
        /// <see cref="Map{TSource,TTarget}(TSource,TTarget)"/>
        [Obsolete("Use Map(...) instead")]
        public static TTarget ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Action<Mapping<TSource, TTarget>> map)
            where TSource : class where TTarget : class
            => Map(source, target, Mapper.GlobalContainer, map);
        
        /// <see cref="Map{TSource,TTarget}(TSource,TTarget)"/>
        [Obsolete("Use Map(...) instead")]
        public static TTarget ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container)
            where TSource : class where TTarget : class
            => Map(source, target, container, null);

        /// <see cref="Map{TSource,TTarget}(TSource,TTarget)"/>
        [Obsolete("Use Map(...) instead")]
        public static TTarget ApplyTo<TSource, TTarget>(this TSource source, TTarget target, IMappingContainer container, Action<Mapping<TSource, TTarget>> map)
            where TSource : class where TTarget : class => Map(source, target, container, map);

        #endregion
    }

}
