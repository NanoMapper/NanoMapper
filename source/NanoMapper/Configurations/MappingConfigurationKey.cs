using System;

namespace NanoMapper.Configurations {
    public class MappingConfigurationKey : Tuple<Type, Type> {
        public MappingConfigurationKey(Type sourceType, Type targetType)
            : base(sourceType, targetType) { }

    }
}