﻿using System;
using System.Threading;

namespace NanoMapper.Core {

    /// <summary>
    /// Global mappings container entry point.
    /// </summary>
    public static class Mappings {

        /// <summary>
        /// Creates a new instance of a mapping container
        /// </summary>
        public static IMappingContainer CreateContainer(bool enableGlobalMappings = false) {
            return new MappingContainer(enableGlobalMappings ? GlobalContainer : null);
        }

        /// <summary>
        /// Configures the mappings from source types to target types against the global mapping cache.
        /// </summary>
        /// <param name="configure">A mapping configuration function</param>
        public static IMappingContainer Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            return GlobalContainer.Configure(configure);
        }

        /// <summary>
        /// Singleton global container instance
        /// </summary>
        internal static IMappingContainer GlobalContainer => _staticContainer.Value;

        /// <summary>
        /// Provides the mechanics for static singleton instantiation.
        /// </summary>
        private static readonly Lazy<MappingContainer> _staticContainer = new Lazy<MappingContainer>(LazyThreadSafetyMode.PublicationOnly);
    }

}
