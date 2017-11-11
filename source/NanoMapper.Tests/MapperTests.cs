using Xunit;

namespace NanoMapper.Tests {

    public class MapperTests {

        [Fact]
        public void TestTheMappingSystem() {
            var key = new MappingConfigurationKey(typeof(SourceClass), typeof(TargetClass));

            var mapper = Mapper.CreateMapper();
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.False(mapper.Mappings.ContainsKey(key));

            source.ApplyTo(target, mapper);

            // mapping configurations are cached
            Assert.True(mapper.Mappings.ContainsKey(key));

            // automatic property resolution
            Assert.NotEqual(source.Id.ToString(), target.Id);
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Description, target.Description);
            Assert.Equal(source.InternalState, target.InternalState);

            mapper = Mapper.CreateMapper();
            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, mapper, map => {
                map.Property(t => t.Id, s => s.Id.ToString());
            });

            // explicit property mapping
            Assert.Equal(source.Id.ToString(), target.Id);

            // reset just the objects
            source = new SourceClass();
            target = new TargetClass();
            source.ApplyTo(target, mapper);

            // local overrides are not cached
            Assert.NotEqual(source.Id.ToString(), target.Id);

            mapper = Mapper.CreateMapper();
            source = new SourceClass();
            target = new TargetClass();

            mapper.Configure<SourceClass, TargetClass>(map => {
                map.Ignore(t => t.Name);
            });

            source.ApplyTo(target, mapper);

            // ignored properties are not mapped
            Assert.NotEqual(source.Name, target.Name);
            Assert.Equal(source.Description, target.Description);

            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, mapper, map => {
                map.Property(t => t.Name);
            });

            // ignored properties with explicit includes are mapped
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Description, target.Description);

            mapper = Mapper.CreateMapper();
            source = new SourceClass();
            target = new TargetClass();
            
            Assert.False(Mapper.GlobalInstance.Mappings.ContainsKey(key));

            Mapper.Configure<SourceClass, TargetClass>(map => {
                map.Property(t => t.Name, s => "GLOBAL MAPPING VALUE");
            });

            source.ApplyTo(target, mapper);

            const string GLOBAL_MAPPING_NAME_VALUE = "GLOBAL MAPPING VALUE";

            // don't use global cache by default
            Assert.NotEqual(source.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.NotEqual(target.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.Equal(source.Name, target.Name);

            mapper = Mapper.CreateMapper();
            source = new SourceClass();
            target = new TargetClass();

            mapper.EnableGlobalMappings = true;

            source.ApplyTo(target, mapper);

            // global cache was enabled
            Assert.Equal(source.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.Equal(target.Name, GLOBAL_MAPPING_NAME_VALUE);
        }
        
        public class SourceClass {
            public int Id { get; set; } = 12345;
            public string Name { get; set; } = "source test";
            public string Description { get; set; } = "This is a source test";
            internal int InternalState { get; set; } = 999;
            private int PrivateState { get; set; } = 123;
        }
        public class TargetClass {
            public string Id { get; set; } = "98765";
            public string Name { get; set; } = "TARGET TEST";
            public string Description { get; set; } = "This is a target test";
            internal int InternalState { get; set; } = 111;
            private int PrivateState { get; set; } = 456;
        }
    }

}
