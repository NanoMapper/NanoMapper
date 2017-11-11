//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Threading;

//namespace NanoMapper {

//    public static class MapperExtension {

//        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target) {
            
//        }

//        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Action<Mapping<TSource, TTarget>> configure) {
//            var mappings = new Mapping<TSource, TTarget>();



//            configure(mappings);


//        }

//        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Func<Mapper> mapperFactory)
//            => ApplyTo(source, target, mapperFactory());
        
//        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Mapper mapper) {

//        }

//        public static void ApplyTo<TSource, TTarget>(this TSource source, TTarget target, Mapper mapper, Action<Mapping<TSource, TTarget>> configure) {

//            if (!mapper.Mappings.TryGetValue(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), out object mappingConfigEntry)) {

//            }



//            if (source.Equals(target)) {
//                return;
//            }

//            var sourceType = source.GetType().GetTypeInfo();
//            var targetType = target.GetType().GetTypeInfo();

//            var sourceProps = sourceType.GetProperties().Where(p => p.CanRead);
//            var targetProps = targetType.GetProperties().Where(p => p.CanWrite);
//            var properties = sourceProps.Intersect(targetProps, new ComparePropertiesByNameAndType()).ToList();

//            var sourceFields = sourceType.GetFields();
//            var targetFields = targetType.GetFields();
//            var fields = sourceFields.Intersect(targetFields, new CompareFieldsByNameAndType()).ToList();

//            Copy(source, target, properties, fields);
//        }

//        private static void Copy(object source, object target, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields) {
//            foreach (var property in properties) {
//                property.SetValue(target, property.GetValue(source));
//            }

//            foreach (var field in fields) {
//                field.SetValue(target, field.GetValue(source));
//            }
//        }
//    }

//    public class Mapper {
//        public static Mapping<TSource, TTarget> Configure<TSource, TTarget>() {
//            return Configure<TSource, TTarget>(null);
//        }

//        public static Mapping<TSource, TTarget> Configure<TSource, TTarget>(Action<Mapping<TSource, TTarget>> configure) {
//            var mapping = GlobalMapper.GetMappingFor<TSource, TTarget>();

//            configure(mapping);

//            return  mapping;
//        }


//        public Mapping<TSource, TTarget> GetMappingFor<TSource, TTarget>() {
//            var key = new Tuple<Type, Type>(typeof(TSource), typeof(TTarget));
//            var mappingConfigEntry = _mappings.GetOrAdd(key, k => new Mapping<TSource, TTarget>());

//            return (Mapping<TSource, TTarget>)mappingConfigEntry;
//        }
//        public bool HasMappingFor<TSource, TTarget>() {
//            return _mappings.ContainsKey(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)));
//        }



//        private readonly ConcurrentDictionary<Tuple<Type, Type>, IMapping> _mappings = new ConcurrentDictionary<Tuple<Type, Type>, IMapping>();
        
//        public static Mapper GlobalMapper => _globalMapper.Value;
//        private static readonly Lazy<Mapper> _globalMapper = new Lazy<Mapper>(() => new Mapper(), LazyThreadSafetyMode.PublicationOnly);
//    }
    
//    public interface IMapping { }

//    public class Mapping<TSource, TTarget> : IMapping {


//        public Mapping<TSource, TTarget> IncludeFields() {
//            return this;
//        }

//        public Mapping<TSource, TTarget> Property<TSourceResult>(Expression<Func<TSource, TSourceResult>> property) {
//            return this;
//        }

//        public Mapping<TSource, TTarget> Property<TSourceResult, TTargetResult>(Expression<Func<TSource, TSourceResult>> source, Expression<Func<TTarget, TTargetResult>> target) {
//            return this;
//        }

//        public Mapping<TSource, TTarget> Property<TSourceResult, TTargetResult>(Expression<Func<TSource, TSourceResult>> source, Expression<Func<TTarget, TTargetResult>> target, Func<TSourceResult, TTargetResult> adapter) {
//            return this;
//        }

//        public Mapping<TSource, TTarget> Ignore<TResult>(Expression<Func<TSource, TResult>> propertyExpression) {
//            return this;
//        }

//    }

//    public class ComparePropertiesByNameAndType : IEqualityComparer<PropertyInfo> {

//        public bool Equals(PropertyInfo source, PropertyInfo target) {
//            if (source == target) {
//                return true;
//            }

//            return source.Name.Equals(target.Name) && target.PropertyType.GetTypeInfo().IsAssignableFrom(source.PropertyType);
//        }

//        public int GetHashCode(PropertyInfo obj) {
//            return obj.Name.GetHashCode();
//        }
//    }

//    public class CompareFieldsByNameAndType : IEqualityComparer<FieldInfo> {

//        public bool Equals(FieldInfo source, FieldInfo target) {
//            if (source == target) {
//                return true;
//            }

//            return source.Name.Equals(target.Name) && target.FieldType.GetTypeInfo().IsAssignableFrom(source.FieldType);
//        }

//        public int GetHashCode(FieldInfo obj) {
//            return obj.Name.GetHashCode();
//        }
//    }

//}
