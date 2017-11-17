using NanoMapper.Extensions;
using System;

namespace NanoMapper.Core {

    /// <summary>
    /// Provides a default IMappingContainer implementation.
    /// </summary>
    /// <inheritdoc cref="IMappingContainer" />
    internal class MappingContainer : IMappingContainer {

        public void Configure<TSource, TTarget>(Action<IMapping<TSource, TTarget>> configure) where TSource : class where TTarget : class {

        }

        public IMapping<TSource, TTarget> GetMappingFor<TSource, TTarget>() {
            return new Mapping<TSource, TTarget>();
        }
    }

}
