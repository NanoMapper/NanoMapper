# NanoMapper.NET

A super-small object mapping library for .NET

    nuget install NanoMapper

## The basics

NanoMapper.NET's sole purpose in life is to alleviate the tediousness
of mapping (applying) the values from one object (source) onto another (target).

This is accomplished using the `ApplyTo(...)` object extension method.

    source.ApplyTo(target);

### Configuring object mappings

NanoMapper.NET automatically matches the properties from both sides,
 using their names and types.

Property analysis is *generally* performed once for any pair of
source / target object type combination. Once made, the mapping configuration
is cached so future applications execute super-fast.

Because of this it is recommended that most mappings be pre-configured once during
the application startup process.

This is done through the global `Mapper` class' `Configure(...)` method, which takes in a mapping configuration function used to map the source type's properties to the target's.

    // Configure the mapping between SourceType and TargetType objects
    Mapper.Configure<SourceType, TargetType>(map => { ... });

#### Mapping properties

By default, a mapper will match all properties using the property names and types.
As such, properties that exist on both source and target objects that match in both name and type
will be automatically configured and don't require explicit configurations.

However, to explicitly configure a target's property mapping, we use the `map.Property(...)` method.

    map.Property(t => t.Property, s => s.OtherProperty);

Here the `t` parameter represents the "target", the `s` parameter representing the "source".

You must provide a translation function that can be called to perform the value mapping
when no suitable implicit conversion exists between the source and target property types.

    map.Property(t => t.Property, s => s.OtherPropertyWithDifferentType, v => CodeToTranslateSourceValueToTargetValue(v));

When you need to map a property that requires access to the whole source object, we use the source object overload .

    map.Property(t => t.Property, s => CodeToTranslateSourceTypeToTargetTypeValue(s));

The is especially useful when the intended value is conditional, or highly complex;

> Note: Annonymous target types are not supported (yet!) as they are immutable and not compatible with the current implementation. You can however use them as source objects.

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
