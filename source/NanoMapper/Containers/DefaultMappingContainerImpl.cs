using NanoMapper.Configurations;
using System;
using System.Collections.Concurrent;

namespace NanoMapper.Containers {

    /// <summary>
    /// Provides a default IMappingContainer implementation.
    /// </summary>
    /// <inheritdoc cref="IMappingContainer" />
    internal class DefaultMappingContainerImpl : IMappingContainer {

        public void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            var key = new MappingConfigurationKey(typeof(TSource), typeof(TTarget));

            //IMappingConfiguration<TSource, TTarget> mapping, globalMapping;

            //if (EnableGlobalMappings && this != NanoMapper.Mappings.GlobalContainer) {

            //    // try to locate the mapping from the global mapper
            //    if (NanoMapper.Mappings.GlobalContainer.Mappings.TryGetValue(key, out var globalMappingEntry)) {
            //        globalMapping = (IMappingConfiguration<TSource, TTarget>)globalMappingEntry;
            //    }
            //}

            //// try to locate the mapping from the local mapper
            //if (NanoMapper.Mappings.GlobalContainer.Mappings.TryGetValue(key, out var mapping)) {
            //    globalMapping;
            //}

            //// apply the configurations
            //configure(mapping);

        }

        public bool EnableGlobalMappings { get; set; }

        public ConcurrentDictionary<MappingConfigurationKey, IMappingConfiguration> Mappings => new ConcurrentDictionary<MappingConfigurationKey, IMappingConfiguration>();
    }

}
