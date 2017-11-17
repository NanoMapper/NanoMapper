using NanoMapper.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoMapper.Core {

    public abstract class Mapping : IMapping {
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
                    _transforms.Add(targetProperty, (TSource s) => sourceProperty.GetValue(s));
                }
            }
        }

        public void Apply(TSource source, TTarget target) {
            foreach (var transform in this._transforms) {
                transform.Key.SetValue(target, transform.Value(source));
            }
        }

        private readonly Dictionary<PropertyInfo, Func<TSource, object>> _transforms = new Dictionary<PropertyInfo, Func<TSource, object>>();
        
        private static readonly TypeInfo SourceTypeInfo = typeof(TSource).GetTypeInfo();
        private static readonly TypeInfo TargetTypeInfo = typeof(TTarget).GetTypeInfo();
    }
}