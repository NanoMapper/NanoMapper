using NanoMapper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NanoMapper.Configurations {

    /// <inheritdoc cref="IMappingConfiguration{TSource,TTarget}" />
    public class MappingConfiguration<TSource, TTarget> : IMappingConfiguration<TSource, TTarget> where TSource : class where TTarget : class {

        public IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            ValidateAndRegisterMapping(propertyInfo, (TSource source) => (TResult)propertyInfo.GetValue(source));

            return this;
        }

        public IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            ValidateAndRegisterMapping(propertyInfo, translationFunc);

            return this;
        }

        public IMappingConfiguration<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            PropertyMappings.Remove(propertyInfo);

            return this;
        }

        public void Execute(TSource source, TTarget target) {
            foreach (var mapping in PropertyMappings) {
                var translate = (Func<TSource, object>)mapping.Value;
                mapping.Key.SetValue(target, translate(source));
            }
        }

        private static PropertyInfo ExtractPropertyInfoFromPropertyExpression<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {

            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess) {
                return (PropertyInfo)((MemberExpression)propertyExpression.Body).Member;
            }

            throw new InvalidOperationException("Property expression must be a valid property reference");
        }

        private void ValidateAndRegisterMapping<TResult>(PropertyInfo propertyInfo, Func<TSource, TResult> translationFunc) {
            if (!propertyInfo.CanWrite) {
                throw new ReadOnlyPropertyException(propertyInfo);
            }

            if (PropertyMappings.ContainsKey(propertyInfo)) {
                PropertyMappings[propertyInfo] = translationFunc;
            }
            else {
                PropertyMappings.Add(propertyInfo, translationFunc);
            }
        }

        private readonly IDictionary<PropertyInfo, object> PropertyMappings = new Dictionary<PropertyInfo, object>();
    }

}
