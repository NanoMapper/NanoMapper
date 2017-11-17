using System;
using System.Collections.Concurrent;

namespace NanoMapper.Core {

    /// <summary>
    /// Provides a default IMappingContainer implementation.
    /// </summary>
    /// <inheritdoc cref="IMappingContainer" />
    internal class MappingContainer : IMappingContainer {

        public void Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> configure) where TSource : class where TTarget : class {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>());
            
            configure((Mapping<TSource, TTarget>)mapping);
        }

        public Mapping<TSource, TTarget> GenerateMappingFor<TSource, TTarget>() {
            var mapping = _mappings.GetOrAdd(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), new Mapping<TSource, TTarget>());

            // Return a copy of the data
            return new Mapping<TSource, TTarget>((Mapping<TSource, TTarget>)mapping);
        }


        private readonly ConcurrentDictionary<Tuple<Type, Type>, IMapping> _mappings = new ConcurrentDictionary<Tuple<Type, Type>, IMapping>();
    }

}
