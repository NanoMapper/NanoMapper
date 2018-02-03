using NanoMapper.Exceptions;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NanoMapper.Tests {

    public class MapperTests {

        [Fact]
        public void StaticMappingScenarioTest() {
            const string GLOBAL_MAPPING_NAME_VALUE = "GLOBAL MAPPING VALUE";

            Assert.False(Mapper.GlobalContainer.HasMappingFor<SourceClass, TargetClass>());

            Mappings.Configure<SourceClass, TargetClass>(map => {
                map.Property(t => t.Name, s => GLOBAL_MAPPING_NAME_VALUE);
            });

            Assert.True(Mapper.GlobalContainer.HasMappingFor<SourceClass, TargetClass>());

            var source = new SourceClass();
            var target = new TargetClass();

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container);

            // containers don't use global mappings by default
            Assert.NotEqual(GLOBAL_MAPPING_NAME_VALUE, target.Name);
            Assert.Equal(source.Name, target.Name);

            container = Mappings.CreateContainer(enableGlobalMappings: true);
            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, container);

            // container should have accessed the global mappings
            Assert.Equal(GLOBAL_MAPPING_NAME_VALUE, target.Name);
        }

        [Fact]
        public void MappingConfigurationsAreCached() {
            var source = new SourceClass();
            var target = new TargetClass();

            var container = Mappings.CreateContainer();

            Assert.False(container.HasMappingFor<SourceClass, TargetClass>());

            source.ApplyTo(target, container);

            Assert.True(container.HasMappingFor<SourceClass, TargetClass>());
        }

        [Fact]
        public void LocalMappingOverrideConfigurationsAreNotCached() {
            var source = new SourceClass();
            var target = new TargetClass();

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container, map => {
                map.Property(p => p.TargetDescription, s => s.SourceDescription);
            });

            Assert.Equal(source.SourceDescription, target.TargetDescription);

            source = new SourceClass();
            target = new TargetClass();

            source.ApplyTo(target, container);

            Assert.NotEqual(source.SourceDescription, target.TargetDescription);
        }

        [Fact]
        public void PropertiesOfDifferingTypesCanBeMapped() {
            var source = new SourceClass();
            var target = new TargetClass();

            Assert.NotEqual(source.Id.ToString(CultureInfo.InvariantCulture), target.Id);

            var container = Mappings.CreateContainer();

            source.ApplyTo(target, container, map => {
                map.Property(t => t.Id, s => s.Id.ToString(CultureInfo.InvariantCulture));
            });

            Assert.Equal(source.Id.ToString(CultureInfo.InvariantCulture), target.Id);
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

        [Fact]
        public void ContainersAreThreadSafe() {
            var container = Mappings.CreateContainer();

            Parallel.For(0, 100000, i => {
                var source = new SourceClass();
                var target = new TargetClass();

                container.Configure<SourceClass, TargetClass>(map => {
                    map.Property(t => t.Id, s => s.Id.ToString(CultureInfo.InvariantCulture));
                });

                source.ApplyTo(target, container, map => {
                    map.Property(t => t.TargetDescription, s => s.SourceDescription);
                });

                Assert.Equal(source.Id.ToString(CultureInfo.InvariantCulture), target.Id);
                Assert.Equal(source.Name, target.Name);
                Assert.Equal(source.SourceDescription, target.TargetDescription);
            });
        }

        [Fact]
        public void GracefullyHandlesNullsOnBothSides() {
            var source = new SourceClass();
            var target = new TargetClass();
            var container = Mappings.CreateContainer();

            source.ApplyTo((TargetClass)null, container);
            ((SourceClass)null).ApplyTo(target, container);
            ((SourceClass)null).ApplyTo((TargetClass)null, container);

            var newSource = new SourceClass();
            var newTarget = new TargetClass();

            Assert.Equal(newSource.Id, source.Id);
            Assert.Equal(newSource.Name, source.Name);
            Assert.Equal(newSource.SourceDescription, source.SourceDescription);

            Assert.Equal(newTarget.Id, target.Id);
            Assert.Equal(newTarget.Name, target.Name);
            Assert.Equal(newTarget.TargetDescription, target.TargetDescription);
        }
        
        [Fact]
        public void TargetIsReturnedFromApplyTo() {
            var source = new SourceClass();
            var target = new TargetClass();
            
            var container = Mappings.CreateContainer();

            var appliedTarget = source.ApplyTo(target, container);

            Assert.Same(target, appliedTarget);
            Assert.Equal(source.Name, target.Name);
        }
        
        [Fact]
        public void ApplyToCanBeUsedInBasicLinqSelectExpression() {
            var sources = new[] {
                new SourceClass(),
                new SourceClass(),
                new SourceClass()
            };

            var container = Mappings.CreateContainer();

            var targets = sources.Select(s => s.ApplyTo(new TargetClass(), container)).ToList();
            
            for (var i = 0; i < sources.Length; i++) {
                Assert.Equal(sources[i].Name, targets[i].Name);
            }
        }

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
