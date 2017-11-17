using System;
using System.Linq.Expressions;

namespace NanoMapper.Core {

    /// <summary>
    /// Represents a generic mapping.
    /// </summary>
    public interface IMapping { }

    /// <summary>
    /// Represents transform mappings for source to target application.
    /// </summary>
    public interface IMapping<TSource, TTarget> {

        /// <summary>
        /// Maps the given property from source to target where
        /// both source and target contain compatible property
        /// applications.
        /// </summary>
        IMapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);

        /// <summary>
        /// Maps the given property from source to target
        /// using the specified @translationFunc to perform
        /// mapping.
        /// </summary>
        IMapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc);

        /// <summary>
        /// Specifies that this property should be ignored.
        /// </summary>
        IMapping<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);

        /// <summary>
        /// Applies the configured mappings from the source object onto the target object.
        /// </summary>
        void Apply(TSource source, TTarget target);
    }

}
