using System;
using System.Collections.Concurrent;
using NanoMapper.Configurations;

namespace NanoMapper.Containers {

    /// <summary>
    /// Represents an object that contains type mappings.
    /// </summary>
    public interface IMappingContainer {

        /// <summary>
        /// Configures the mappings from source types to target types.
        /// </summary>
        /// <param name="configure">A mapping configuration function</param>
        void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) where TSource : class where TTarget : class;

        /// <summary>
        /// <para>T: the container will access any globally configured mappings when resolving type application.</para>
        /// <para>F: the container will NOT access any globally configured mappings when resolving type application.</para>
        /// </summary>
        bool EnableGlobalMappings { get; set; }

        /// <summary>
        /// The mapping collection cache.
        /// </summary>
        ConcurrentDictionary<MappingConfigurationKey, IMappingConfiguration> Mappings { get; }
    }

}