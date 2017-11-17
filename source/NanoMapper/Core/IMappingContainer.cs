using System;

namespace NanoMapper.Core {

    /// <summary>
    /// Represents an object that contains type mappings.
    /// </summary>
    public interface IMappingContainer {

        /// <summary>
        /// Configures the mappings from source types to target types.
        /// </summary>
        /// <param name="configure">A mapping configuration function</param>
        void Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> configure) where TSource : class where TTarget : class;

        /// <summary>
        /// Generates the mapping for the source to target transforms.
        /// </summary>
        Mapping<TSource, TTarget> GenerateMappingFor<TSource, TTarget>();
    }

}
