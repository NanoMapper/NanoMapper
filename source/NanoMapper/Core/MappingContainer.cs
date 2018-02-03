using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NanoMapper {

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

        
        /// <see cref="Map{TSource,TTarget}"/>
        [Obsolete("Use Map(...) instead")]
        public IMappingContainer Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> map) where TSource : class where TTarget : class
            => Map(map);

        public IMappingContainer Map<TSource, TTarget>(Action<Mapping<TSource, TTarget>> map) where TSource : class where TTarget : class {
            if (map == null) throw new ArgumentNullException(nameof(map));

            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>(_parentContainer == null));

            map((Mapping<TSource, TTarget>)mapping);

            return this;
        }

        public Mapping<TSource, TTarget> GenerateMappingFor<TSource, TTarget>() {
            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>(_parentContainer == null));

            if (_parentContainer != null) {
                var parentMappping = _parentContainer.GenerateMappingFor<TSource, TTarget>();
                return new Mapping<TSource, TTarget>(parentMappping.Bindings.Concat(mapping.Bindings));
            }
            
            return new Mapping<TSource, TTarget>(mapping.Bindings);
        }

        public bool HasMappingFor<TSource, TTarget>() {
            return _mappings.ContainsKey(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)));
        }


        private readonly ConcurrentDictionary<Tuple<Type, Type>, IMapping> _mappings = new ConcurrentDictionary<Tuple<Type, Type>, IMapping>();
        private readonly IMappingContainer _parentContainer;
    }

}
