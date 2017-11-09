using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace NanoMapper.Tests {

    public class MappingTests {


        /* copy object
         * copy object with special mappings
         * configure global mapping
         * configuring for unit testing
         */


        [Fact]
        public void HandlesTheBasics() {
            var source = new SourceClass {
                SourceProperty = 123
            };

            var target = new TargetClass {};
            
            source.CopyTo(target, map => {
                map.Property(s => s.SourceProperty, t => t.TargetProperty, v => v.ToString());
            });

            Assert.Equal(source.SourceProperty.ToString(), target.TargetProperty);
        }

        [Fact]
        public void HandlesTypeMapping() {
            Mapper.Configure<SourceClass, TargetClass>(map => {
                map.Property(s => s.SourceProperty, t => t.TargetProperty);
            });
        }
    }


    public class SourceClass {
        public int SourceProperty { get; set; }
    }
    public class TargetClass {
        public string TargetProperty { get; set; }
    }
}
