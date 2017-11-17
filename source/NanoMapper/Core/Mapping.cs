using NanoMapper.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NanoMapper.Exceptions;

namespace NanoMapper.Core {

    /// <summary>
    /// Encapsulates common mapping functionality shared by IMapper implementations.
    /// </summary>
    public abstract class Mapping : IMapping {

        /// <summary>
        /// Provides a property equality comparer
        /// that can be used to compare property equality
        /// by their name and type combinations (obviously).
        /// </summary>
        protected internal static readonly IEqualityComparer<PropertyInfo> PropertyComparer = new ComparePropertyByNameAndType();
    }

    public class Mapping<TSource, TTarget> : Mapping, IMapping<TSource, TTarget> {

        public Mapping() {
            var sourceProps = SourceTypeInfo.GetProperties().Where(p => p.CanRead).ToList();
            var targetProps = TargetTypeInfo.GetProperties().Where(p => p.CanWrite).ToList();
            
            foreach (var targetProperty in targetProps) {
                var sourceProperty = sourceProps.FirstOrDefault(s => PropertyComparer.Equals(s, targetProperty));

                if (sourceProperty != null) {
                    sourceProps.Remove(sourceProperty);
                    _bindings.Add(targetProperty, (Func<TSource, object>)((TSource s) => sourceProperty.GetValue(s)));
                }
            }
        }


        public IMapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            RegisterBinding(propertyInfo, (TSource source) => (TResult)propertyInfo.GetValue(source));

            return this;
        }
        
        public IMapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc) {            
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            RegisterBinding(propertyInfo, translationFunc);

            return this;
        }
        
        public IMapping<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            _bindings.Remove(propertyInfo);

            return this;
        }
        public void Apply(TSource source, TTarget target) {
            foreach (var binding in this._bindings) {
                binding.Key.SetValue(target, ((Func<TSource, object>)binding.Value).Invoke(source));
            }
        }


        
        private PropertyInfo ExtractPropertyInfoFromPropertyExpression<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess) {
                return (PropertyInfo)((MemberExpression)propertyExpression.Body).Member;
            }

            throw new InvalidOperationException("Property expression must be a valid property reference");
        }

        private void RegisterBinding<TResult>(PropertyInfo propertyInfo, Func<TSource, TResult> binding) {
            if (!propertyInfo.CanWrite) {
                throw new ReadOnlyPropertyException(propertyInfo);
            }
            
            if (_bindings.ContainsKey(propertyInfo)) {
                _bindings[propertyInfo] = binding;
            }
            else {
                _bindings.Add(propertyInfo, binding);
            }
        }
        
        private readonly Dictionary<PropertyInfo, Delegate> _bindings = new Dictionary<PropertyInfo, Delegate>();
        
        private static readonly TypeInfo SourceTypeInfo = typeof(TSource).GetTypeInfo();
        private static readonly TypeInfo TargetTypeInfo = typeof(TTarget).GetTypeInfo();
    }
}