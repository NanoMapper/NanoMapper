using System;
using System.Collections.Concurrent;

namespace NanoMapper.Core {

    /// <summary>
    /// Provides a default IMappingContainer implementation.
    /// </summary>
    /// <inheritdoc cref="IMappingContainer" />
    internal class MappingContainer : IMappingContainer {
        
        public MappingContainer()
            : this(null) { }

        public MappingContainer(IMappingContainer parentContainer) {
            _parentContainer = parentContainer;
        }

        public IMappingContainer Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>(_parentContainer == null));
            
            configure((Mapping<TSource, TTarget>)mapping);

            return this;
        }

        public Mapping<TSource, TTarget> GenerateMappingFor<TSource, TTarget>() {
            var parentMappping = _parentContainer?.GenerateMappingFor<TSource, TTarget>();
            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>(_parentContainer == null));
            
            // Return a copy of the data
            return new Mapping<TSource, TTarget>(parentMappping, (Mapping<TSource, TTarget>)mapping);
        }

        public bool HasMappingFor<TSource, TTarget>() {
            return _mappings.ContainsKey(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)));
        }


        private readonly ConcurrentDictionary<Tuple<Type, Type>, IMapping> _mappings = new ConcurrentDictionary<Tuple<Type, Type>, IMapping>();
        private readonly IMappingContainer _parentContainer;
    }

}
