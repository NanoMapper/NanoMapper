using System;
using System.Collections.Concurrent;
using System.Threading;
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
        /// <para>T: the mapper will access any globally configured mappings when resolving type application.</para>
        /// <para>F: the mapper will NOT access any globally configured mappings when resolving type application.</para>
        /// </summary>
        bool EnableGlobalMappings { get; set; }

        /// <summary>
        /// The mapping collection cache.
        /// </summary>
        ConcurrentDictionary<MappingConfigurationKey, IMappingConfiguration> Mappings { get; }
    }

    /// <summary>
    /// Global mappings container entry point.
    /// </summary>
    public static class Mappings {

        /// <summary>
        /// Creates a new instance of a mapping container
        /// </summary>
        /// <remarks>For use in unit testing and DI / IoC based scenarios.</remarks>
        public static IMappingContainer CreateContainer() {
            return new DefaultMappingContainerImpl();
        }

        /// <summary>
        /// Configures the mappings from source types to target types against the global mapping cache.
        /// </summary>
        /// <param name="configure">A mapping configuration function</param>
        public static void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            GlobalContainer.Configure(configure);
        }
        
        /// <summary>
        /// Singleton global mapper instance
        /// </summary>
        internal static IMappingContainer GlobalContainer => _staticMapper.Value;

        /// <summary>
        /// Provides the mechanics for static singleton instantiation.
        /// </summary>
        private static readonly Lazy<DefaultMappingContainerImpl> _staticMapper = new Lazy<DefaultMappingContainerImpl>(LazyThreadSafetyMode.PublicationOnly);
    }

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
