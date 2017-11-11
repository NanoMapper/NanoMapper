using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NanoMapper {
    
    public interface IMappingConfiguration<TSource, TTarget> {
        IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);
        IMappingConfiguration<TSource, TTarget> Property<TResult>(Expression<Func<TTarget, TResult>> propertyExpression, Func<TSource, TResult> translationFunc);

        IMappingConfiguration<TSource, TTarget> Ignore<TResult>(Expression<Func<TTarget, TResult>> propertyExpression);
    }

    
    public class MappingConfigurationKey : Tuple<Type, Type> {
        public MappingConfigurationKey(Type sourceType, Type targetType)
            : base(sourceType, targetType) { }
        
    }


}
