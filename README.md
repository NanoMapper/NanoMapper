# NanoMapper.NET

The superbly simple, thread safe object mapping library for .NET

    PM> Install-Package NanoMapper

## The basics

NanoMapper's sole purpose in life is to alleviate the supremely tedious business
of mapping (applying) some object (the source) to some other object (the target).

This is accomplished using the `ApplyTo(...)` object extension method.

    source.ApplyTo(target);

NanoMapper automatically matches the properties from both sides,
 using their names and types.

> Note: Annonymous target types are not supported (yet!) as they are immutable and not compatible with the current implementation. You can however use them as source objects.

Property analysis is *generally* performed once for any pair of
source / target type combinations. And once made, the mapping configuration
is cached so future applications execute super-fast.

Because of this, it is recommended that most mappings be pre-configured once during
the application's startup process.

This can be done using the global `Mappings.Configure(...)` method, which takes in a mapping configuration function that defines the property mappings between the source and target types.

    // Configure the mapping between SourceType and TargetType objects
    Mappings.Configure<SourceType, TargetType>(map => {
      map.Property(...);
      ...
    });

#### Mapping properties

By default, properties that exist on both source and target objects that match in both name and type will be automatically mapped and do not require explicit configuration.

However, to configure a target's property mapping, we use the `map.Property(...)` method:

    map.Property(t => t.Property, s => s.OtherProperty);

Here the `t` parameter represents the "target", the `s` parameter representing the "source".

Property mappings can also be highly complex and don't necessarily have to be simple property to property mappings:

    map.Property(t => t.Property, s => CodeToTranslateSourceTypeToTargetTypeValue(s));

#### Ignoring properties

Properties we want to omit from being automatically applied should be "ignored":

    map.Ignore(t => t.Property);

#### Local mapping overrides

To configure "on-the-fly" mappings we use an
`ApplyTo(...)` method overload that allows us to pass in an additional
mapping configuration function:

    // Apply the object using specific mapping overrides
    source.ApplyTo(target, map => { ... });

This is especially useful when you need to include properties that are currently being ignored:

    // Map this property
    map.Property(t => t.Property, ...);

## Testing / DI compatibility

The default `ApplyTo(...)` method uses the global container instance.

In times when it is necessary to avoid using static constructs, such as during testing or integrating with systems that rely on dependency injection / IoC containers, we can create a new `IMappingContainer` instance and pass that to the `ApplyTo(...)` method instead:

    // Create a new container instance
    var container = Mappings.CreateContainer();

    // Configure some mappings
    container.Configure<SourceType, TargetType>(map => { ... });

    // Use the container
    source.ApplyTo(target, container);

Instance containers do not access the global mappings by default.
In order to utilise any globally defined mappings we need to pass the 
`enableGlobalMappings` argument as `true`.

In doing so the container will check the global cache for existing mapping configurations and apply them first before any instance or overriding mappings.

    // Create a new container instance
    var container = Mappings.CreateContainer(enableGlobalMappings: true);

    // Configure some mappings
    container.Configure<SourceType, TargetType>(map => { ... });

    // Use the container
    source.ApplyTo(target, container);

Alternatively, a custom `IMappingContainer` implementation can be implemented to suit any specific needs.

    // Define a new container
    class CustomMappingContainer : IMappingContainer { }

    // Use the container
    source.ApplyTo(target, new CustomMappingContainer(...));
    
<hr />

*NanoMapper.NET is free, open-source software.* ([MIT](https://raw.githubusercontent.com/garydouble/NanoMapper.NET/master/LICENSE))

[Issues](https://github.com/garydouble/NanoMapper.NET/issues),
[feedback](mailto:garydouble@live.com) and
[pull requests](https://github.com/garydouble/NanoMapper.NET) all welcome.  
Happy Coding!

<p style="text-align:center;">Made with
<span style="color:crimson;font-size:2em;vertical-align:sub;">&hearts;</span> by
<a href="http://garydouble.com" style="font-weight:bold;" target="_blank">Gary Doubl�</a>.</p>
