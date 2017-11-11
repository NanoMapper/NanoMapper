using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;


namespace NanoMapper {

    public interface IMapper {
        void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure);

        bool EnableGlobalMappings { get; set; }

        ConcurrentDictionary<MappingConfigurationKey, object> Mappings { get; }
    }

    public static class Mapper {
        
        public static IMapper CreateMapper() {
            return new MapperImpl();
        }

        public static void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) {
            GlobalInstance.Configure(configure);
        }
        
        internal static IMapper GlobalInstance => _staticMapper.Value;
        private static readonly Lazy<MapperImpl> _staticMapper = new Lazy<MapperImpl>(LazyThreadSafetyMode.PublicationOnly);
    }

    public class MapperImpl : IMapper {

        public void Configure<TSource, TTarget>(Action<IMappingConfiguration<TSource, TTarget>> configure) {
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
