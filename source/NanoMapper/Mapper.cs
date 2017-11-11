using System;
using System.Collections.Concurrent;
using System.Threading;


namespace NanoMapper {

    /// <summary>
    /// Represents an object that can configure type mappings.
    /// </summary>
    public interface IMapper {

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
        ConcurrentDictionary<MappingConfigurationKey, object> Mappings { get; }
    }

    /// <summary>
    /// Global mapper configuration entry point.
    /// </summary>
    public static class Mapper {

        /// <summary>
        /// Creates a new instance of a mapper for use in unit testing and DI / IoC based scenarios.
        /// </summary>
        public static IMapper CreateMapper() {
            return new MapperImpl();
        }

        /// <summary>
        /// Configures the mappings from source types to target types against the global mapping cache.
        /// </summary>
        /// <param name="configure">A mapping configuration function</param>
        public static void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            GlobalInstance.Configure(configure);
        }

        /// <summary>
        /// Singleton global mapper instance
        /// </summary>
        internal static IMapper GlobalInstance => _staticMapper.Value;

        /// <summary>
        /// Provides the mechanics for static singleton instantiation.
        /// </summary>
        private static readonly Lazy<MapperImpl> _staticMapper = new Lazy<MapperImpl>(LazyThreadSafetyMode.PublicationOnly);
    }

    /// <summary>
    /// Provides the default IMapper implementation.
    /// </summary>
    /// <inheritdoc cref="IMapper" />
    internal class MapperImpl : IMapper {

        public void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            IMappingConfiguration<TSource, TTarget> mapping = null;

            if (EnableGlobalMappings && this != Mapper.GlobalInstance) {
                // try to locate the mapping from the global mapper
            }

            // try to locate the mapping from the local mapper

            // apply the configurations
            configure(mapping);

        }

        public bool EnableGlobalMappings { get; set; }

        public ConcurrentDictionary<MappingConfigurationKey, object> Mappings => new ConcurrentDictionary<MappingConfigurationKey, object>();
    }

}
