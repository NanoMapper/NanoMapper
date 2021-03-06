﻿using System;

namespace NanoMapper {

    /// <summary>
    /// Represents an object that contains type mappings.
    /// </summary>
    public interface IMappingContainer {

        /// <see cref="Map{TSource,TTarget}"/>
        [Obsolete("Use Map(...) instead")]
        IMappingContainer Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> map) where TSource : class where TTarget : class;
        
        /// <summary>
        /// Configures the mappings from source types to target types.
        /// </summary>
        /// <param name="map">A mapping configuration function</param>
        /// <returns>The same instance so method chaining is possible</returns>
        IMappingContainer Map<TSource, TTarget>(Action<Mapping<TSource, TTarget>> map) where TSource : class where TTarget : class;
        
        /// <summary>
        /// Generates the mapping for the source to target transforms.
        /// </summary>
        Mapping<TSource, TTarget> GenerateMappingFor<TSource, TTarget>();

        /// <summary>
        /// Determines if this container has a mapping from source types to target types.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns>true if a mapping is configured, false otherwise.</returns>
        bool HasMappingFor<TSource, TTarget>();
    }

}
