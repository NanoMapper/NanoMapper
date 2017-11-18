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

    
    /// <summary>
    /// Represents transform mappings for source to target application.
    /// </summary>
    public class Mapping<TSource, TTarget> : Mapping {

        public Mapping() {
            var sourceProps = SourceTypeInfo.GetProperties(BindingFlags).Where(p => p.CanRead).ToList();
            var targetProps = TargetTypeInfo.GetProperties(BindingFlags).Where(p => p.CanWrite).ToList();
            
            foreach (var targetProperty in targetProps) {
                var sourceProperty = sourceProps.FirstOrDefault(s => PropertyComparer.Equals(s, targetProperty));

                if (sourceProperty != null) {
                    sourceProps.Remove(sourceProperty);
                    _bindings.Add(targetProperty, (Func<TSource, object>)((TSource s) => sourceProperty.GetValue(s)));
                }
            }
        }

        internal Mapping(Mapping<TSource, TTarget> other) {
            _bindings = new Dictionary<PropertyInfo, Delegate>(other._bindings);
        }
        
        /// <summary>
        /// Maps the given property from source to target where
        /// both source and target contain compatible property
        /// applications.
        /// </summary>
        public Mapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var targetPropertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            // We do this because when the types differ we can't use the target type's PropertyInfo
            var sourcePropertyInfo =
                SourceTypeInfo.Equals(TargetTypeInfo) ?
                targetPropertyInfo :
                SourceTypeInfo.GetProperty(targetPropertyInfo.Name, BindingFlags);

            RegisterBinding(targetPropertyInfo, (TSource source) => (TResult)sourcePropertyInfo.GetValue(source));

            return this;
        }
        
        /// <summary>
        /// Maps the given property from source to target
        /// using the specified @translationFunc to perform
        /// mapping.
        /// </summary>
        public Mapping<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc) {            
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            RegisterBinding(propertyInfo, translationFunc);

            return this;
        }
        
        /// <summary>
        /// Specifies that this property should be ignored.
        /// </summary>
        public Mapping<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression) {
            var propertyInfo = ExtractPropertyInfoFromPropertyExpression(propertyExpression);

            _bindings.Remove(propertyInfo);

            return this;
        }
        
        /// <summary>
        /// Applies the configured mappings from the source object onto the target object.
        /// </summary>
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

        private static readonly BindingFlags BindingFlags =
            BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.FlattenHierarchy|BindingFlags.Instance;
    }

}