using NanoMapper.Core;
using NanoMapper.Exceptions;
using NanoMapper.Extensions;
using Xunit;

namespace NanoMapper.Tests {

    public class MapperTests {

        //[Fact]
        //public void FullMappingScenarioTest() {
        //    var key = new MappingConfigurationKey(typeof(SourceClass), typeof(TargetClass));

        //    var container = Mappings.CreateContainer();
        //    var source = new SourceClass();
        //    var target = new TargetClass();

        //    Assert.False(container.Mappings.ContainsKey(key));

        //    source.ApplyTo(target, container);

        //    // mapping configurations are cached
        //    Assert.True(container.Mappings.ContainsKey(key));

        //    // automatic property resolution
        //    Assert.NotEqual(source.Id.ToString(), target.Id);
        //    Assert.Equal(source.Name, target.Name);
        //    Assert.Equal(source.Description, target.Description);
        //    Assert.Equal(source.InternalState, target.InternalState);

        //    container = Mappings.CreateContainer();
        //    source = new SourceClass();
        //    target = new TargetClass();

        //    source.ApplyTo(target, container, map => {
        //        map.Property(t => t.Id, s => s.Id.ToString());
        //    });

        //    // explicit property mapping
        //    Assert.Equal(source.Id.ToString(), target.Id);

        //    // reset just the objects
        //    source = new SourceClass();
        //    target = new TargetClass();
        //    source.ApplyTo(target, container);

        //    // local overrides are not cached
        //    Assert.NotEqual(source.Id.ToString(), target.Id);

        //    container = Mappings.CreateContainer();
        //    source = new SourceClass();
        //    target = new TargetClass();

        //    container.Configure<SourceClass, TargetClass>(map => {
        //        map.Ignore(t => t.Name);
        //    });

        //    source.ApplyTo(target, container);

        //    // ignored properties are not mapped
        //    Assert.NotEqual(source.Name, target.Name);
        //    Assert.Equal(source.Description, target.Description);

        //    source = new SourceClass();
        //    target = new TargetClass();

        //    source.ApplyTo(target, container, map => {
        //        map.Property(t => t.Name);
        //    });

        //    // ignored properties with explicit includes are mapped
        //    Assert.Equal(source.Name, target.Name);
        //    Assert.Equal(source.Description, target.Description);

        //    container = Mappings.CreateContainer();
        //    source = new SourceClass();
        //    target = new TargetClass();

        //    Assert.False(Mappings.GlobalContainer.Mappings.ContainsKey(key));

        //    Mappings.Configure<SourceClass, TargetClass>(map => {
        //        map.Property(t => t.Name, s => "GLOBAL MAPPING VALUE");
        //    });

        //    source.ApplyTo(target, container);

        //    const string GLOBAL_MAPPING_NAME_VALUE = "GLOBAL MAPPING VALUE";

        //    // don't use global cache by default
        //    Assert.NotEqual(source.Name, GLOBAL_MAPPING_NAME_VALUE);
        //    Assert.NotEqual(target.Name, GLOBAL_MAPPING_NAME_VALUE);
        //    Assert.Equal(source.Name, target.Name);

        //    container = Mappings.CreateContainer();
        //    source = new SourceClass();
        //    target = new TargetClass();

        //    container.EnableGlobalMappings = true;

        //    source.ApplyTo(target, container);

        //    // global cache was enabled
        //    Assert.Equal(source.Name, GLOBAL_MAPPING_NAME_VALUE);
        //    Assert.Equal(target.Name, GLOBAL_MAPPING_NAME_VALUE);
        //}
        
        [Fact]
        public void DefaultMappingsTest() {
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.NotEqual(source.Name, target.Name);
            Assert.NotEqual(source.SourceDescription, target.TargetDescription);

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container);

            Assert.Equal(source.Name, target.Name);
            Assert.NotEqual(source.SourceDescription, target.TargetDescription);
        }
        
        [Fact]
        public void DefaultMappingsWithAdditionalsTest() {
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.NotEqual(source.Name, target.Name);
            Assert.NotEqual(source.SourceDescription, target.TargetDescription);

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container, map => {
                map.Property(t => t.TargetDescription, s => s.SourceDescription);
            });

            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.SourceDescription, target.TargetDescription);
        }

        //[Fact]
        //public void SimpleMappingTest() {
        //    var source = new SourceClass();
        //    var target = new TargetClass();

        //    Assert.NotEqual(source.Name, target.Name);
        //    Assert.NotEqual(source.Description, target.Description);

        //    var container = Mappings.CreateContainer();

        //    container.Configure<SourceClass, TargetClass>(map => {
        //        map.Property(t => t.Name, s => s.Name);
        //    });

        //    source.ApplyTo(target, container, map => {
        //        map.Property(t => t.Description, s => s.Description);
        //    });

        //    Assert.Equal(source.Name, target.Name);
        //    Assert.Equal(source.Description, target.Description);
        //}

        [Fact]
        public void MappingReadOnlyTargetPropertyThrowsException() {
            Assert.Throws<ReadOnlyPropertyException>(() => {
                var source = new SourceClass();
                var target = new ReadOnlyClass();
                
                var container = Mappings.CreateContainer();

                source.ApplyTo(target, container, map => {
                    map.Property(t => t.ReadOnlyProperty, s => s.Id);
                });
            });
        }

        public class SourceClass {
            public int Id { get; set; } = 12345;
            public string Name { get; set; } = "source test";
            public string SourceDescription { get; set; } = "This is a source test";
            internal int InternalState { get; set; } = 999;
            private int PrivateState { get; set; } = 123;
        }
        public class TargetClass {
            public string Id { get; set; } = "98765";
            public string Name { get; set; } = "TARGET TEST";
            public string TargetDescription { get; set; } = "This is a target test";
            internal int InternalState { get; set; } = 111;
            private int PrivateState { get; set; } = 456;
        }
        public class ReadOnlyClass {
            public int ReadOnlyProperty => 123;
        }
    }

}
