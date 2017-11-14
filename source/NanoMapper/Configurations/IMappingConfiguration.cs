using System;
using System.Linq.Expressions;

namespace NanoMapper.Configurations {
    
    /// <summary>
    /// Represents a mapping configuration object that is used to configure object property mappings.
    /// </summary>
    public interface IMappingConfiguration { }

    /// <summary>
    /// Represents a mapping configuration object that is used to configure object property mappings.
    /// </summary>
    public interface IMappingConfiguration<TSource, TTarget> : IMappingConfiguration where TSource : class where TTarget : class {

        /// <summary>
        /// Maps the given property from source to target where
        /// both source and target contain compatible property
        /// applications.
        /// </summary>
        IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);

        /// <summary>
        /// Maps the given property from source to target
        /// using the specified @translationFunc to perform
        /// mapping.
        /// </summary>
        IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc);

        /// <summary>
        /// Specifies that this property should be ignored.
        /// </summary>
        IMappingConfiguration<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);

        /// <summary>
        /// Executes the mapping application based on the current mapping configuration.
        /// </summary>
        void Execute(TSource source, TTarget target);
    }

    

}