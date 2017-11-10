# NanoMapper.NET

A super-small object mapping library for .NET

    nuget install NanoMapper

## The basics

NanoMapper.NET's sole purpose in life is to alleviate the tediousness
of mapping (applying) the values from one object (source) on to some other object (target).

This is accomplished using the `ApplyTo(...)` object extension method.

    source.ApplyTo(target);

### Configuring object mappings

NanoMapper automatically matches the properties from both sides
 based on the names and type value.

The property analysis process is generally performed only once for any given pair of
source / target type combinations. Once made, a mapping function is stored in a
global cache to enable super-fast future applications.

Because of this it is recommended that most mappings be pre-configured once during
the applications startup process.

This is done through the default, global mapper class `Mapper`.

    Mapper.Configure<SourceType, TargetType>(map => {
      // configure custom global mappings here...
    });

The `Configure(...)` method takes a function who's argument is a 

#### Mapping properties

By default, a mapper will try to match the properties up of both objects
using the property's name and type.

To explicitly configure a target's property mapping, we use the `map.Property(...)` method.

    map.Property(t => t.Property, s => s.OtherProperty);

Here the `t` parameter represents the "target" type, the `s` parameter representing the "source" object.

When no suitable implicit conversion exists between the source and target property types
you must provide a translation function that can be called to perform the value mapping.

    map.Property(t => t.Property, s => s.OtherPropertyWithDifferentType, v => CodeToTranslateSourceValueToTargetValue(v));

Use the source object overload when you need to map a value that requires multiple source properties,
or when the value is conditional, or complex.

    map.Property(t => t.Property, s => CodeToTranslateSourceTypeToTargetTypeValue(s));

> Note: Annonymous target types are not supported (yet!) as they are immutable as their properties cannot be set using the current implementation. You can however use them as source objects.

#### Ignoring properties

Not all properties are created equal. And, for the ones you don't want to be applied
automatically, you can configure them to be ignored.

    map.Ignore(t => t.Property);

#### Local mapping overrides

To configure "on-the-fly" mappings we can take advantage of the
`ApplyTo(...)` method overload that allows us pass in a mapping
configuration function.

    // Apply the objects using specific mapping overrides
    source.ApplyTo(target, map => { ... });

This is especially useful when you need to include properties that were previously ignored:

    map.Property(t => t.Property);

## Testing / DI compatibility

The default `ApplyTo(...)` method uses a global mapper instance.

During testing or when dealing with systems that rely on dependency injection and IoC containers,
it is often necessary to avoid using static constructs.

When this requirement arises we can simply create a new `Mapper`
instance and pass that through instead the `ApplyTo(...)` method instead.

    // Create a new Mapper instance
    var mapper = new Mapper();

    // Configure some mappings
    mapper.Configure<SourceType, TargetType>(map => { ... });

    // Use the mapper
    source.ApplyTo(target, mapper);

Instance mappers do not access the global cache by default.
In order to utilise both techniques we need only set the mappers
`EnableGlobalMappings` property to `true`.

In doing so the mapper will check the global cache for existing mapping configurations
and apply them first before any instance or overriding mappings.

    // Create a new Mapper instance
    var mapper = new Mapper {

      // Allow the Mapper to call into the global cache   
      EnableGlobalMappings = true
    };

    // Configure some mappings
    mapper.Configure<SourceType, TargetType>(map => { ... });

    // Use the mapper
    source.ApplyTo(target, mapper);

<hr />

*NanoMapper.NET is free, open-source software.* ([MIT](https://raw.githubusercontent.com/garydouble/NanoMapper.NET/master/LICENSE))

[Issues](https://github.com/garydouble/NanoMapper.NET/issues),
[feedback](mailto:garydouble@live.com) and
[pull requests](https://github.com/garydouble/NanoMapper.NET) all welcome.  
Happy Coding!

<p style="text-align:center;">Made with
<span style="color:crimson;font-size:2em;vertical-align:sub;">&hearts;</span> by
<a href="http://garydouble.com" style="font-weight:bold;">Gary Doublé</a>.</p>
