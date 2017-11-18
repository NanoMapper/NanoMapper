using System.Globalization;
using NanoMapper.Core;
using NanoMapper.Exceptions;
using NanoMapper.Extensions;
using Xunit;

namespace NanoMapper.Tests {

    public class MapperTests {

        [Fact]
        public void FullMappingScenarioTest() {
            var container = Mappings.CreateContainer();
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.False(container.HasMappingFor<SourceClass, TargetClass>());

            source.ApplyTo(target, container);

            // mapping configurations are cached
            Assert.True(container.HasMappingFor<SourceClass, TargetClass>());

            // automatic property resolution
            Assert.NotEqual(source.Id.ToString(), target.Id);
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.InternalState, target.InternalState);

            container = Mappings.CreateContainer();
            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, container, map => {
                map.Property(t => t.Id, s => s.Id.ToString());
            });

            // explicit property mapping
            Assert.Equal(source.Id.ToString(), target.Id);

            // reset just the objects
            source = new SourceClass();
            target = new TargetClass();
            source.ApplyTo(target, container);

            // local overrides are not cached
            Assert.NotEqual(source.Id.ToString(), target.Id);


            container = Mappings.CreateContainer();
            source = new SourceClass();
            target = new TargetClass();

            Assert.False(Mappings.GlobalContainer.HasMappingFor<SourceClass, TargetClass>());

            Mappings.Configure<SourceClass, TargetClass>(map => {
                map.Property(t => t.Name, s => "GLOBAL MAPPING VALUE");
            });

            source.ApplyTo(target, container);

            const string GLOBAL_MAPPING_NAME_VALUE = "GLOBAL MAPPING VALUE";

            // don't use global cache by default
            Assert.NotEqual(source.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.NotEqual(target.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.Equal(source.Name, target.Name);

            container = Mappings.CreateContainer(enableGlobalMappings: true);
            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, container);

            // global cache was enabled
            Assert.Equal(source.Name, GLOBAL_MAPPING_NAME_VALUE);
            Assert.Equal(target.Name, GLOBAL_MAPPING_NAME_VALUE);
        }
        
        [Fact]
        public void IgnoredPropertiesAreNotMapped() {
            var source = new SourceClass();
            var target = new TargetClass();
            
            Assert.NotEqual(source.Name, target.Name);
            
            var container = Mappings.CreateContainer();

            container.Configure<SourceClass, TargetClass>(map => {
                map.Ignore(t => t.Name);
            });

            source.ApplyTo(target, container);
            
            Assert.NotEqual(source.Name, target.Name);
        }

        [Fact]
        public void IgnoredPropertiesWithExplicitIncludesAreMapped() {
            var source = new SourceClass();
            var target = new TargetClass();
            
            Assert.NotEqual(source.Name, target.Name);
            
            var container = Mappings.CreateContainer();

            container.Configure<SourceClass, TargetClass>(map => {
                map.Ignore(t => t.Name);
            });

            source.ApplyTo(target, container, map => {
                map.Property(t => t.Name);
            });
            
            Assert.Equal(source.Name, target.Name);
        }
        
        [Fact]
        public void InternalPropertiesAreMappedByDefault() {
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.NotEqual(source.InternalState, target.InternalState);

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container);

            Assert.Equal(source.InternalState, target.InternalState);
        }
        
        [Fact]
        public void InheritedPropertiesAreMappedByDefault() {
            var source = new DerivedSourceClass();
            var target = new DerivedTargetClass();
            
            Assert.NotEqual(source.Name, target.Name);
            Assert.NotEqual(source.Active, target.Active);

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container);
            
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Active, target.Active);
        }


        [Fact]
        public void PropertiesAreMatchedByNameAndTypes() {
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
        public void UnmatchedPropertiesCanBeMapped() {
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

        [Fact]
        public void ConfiguredContainerWithAdditionalsTest() {
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.NotEqual(source.Id.ToString(CultureInfo.InvariantCulture), target.Id);
            Assert.NotEqual(source.Name, target.Name);
            Assert.NotEqual(source.SourceDescription, target.TargetDescription);

            var container = Mappings.CreateContainer();

            container.Configure<SourceClass, TargetClass>(map => {
                map.Property(t => t.Id, s => s.Id.ToString(CultureInfo.InvariantCulture));
            });

            source.ApplyTo(target, container, map => {
                map.Property(t => t.TargetDescription, s => s.SourceDescription);
            });
            
            Assert.Equal(source.Id.ToString(CultureInfo.InvariantCulture), target.Id);
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.SourceDescription, target.TargetDescription);
        }

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
        public class DerivedSourceClass : SourceClass {
            public bool Active { get; set; } = true;
        }

        public class TargetClass {
            public string Id { get; set; } = "98765";
            public string Name { get; set; } = "TARGET TEST";
            public string TargetDescription { get; set; } = "This is a target test";
            internal int InternalState { get; set; } = 111;
            private int PrivateState { get; set; } = 456;
        }
        public class DerivedTargetClass : TargetClass {
            public bool Active { get; set; } = false;
        }

        public class ReadOnlyClass {
            public int ReadOnlyProperty => 123;
        }
    }

}
