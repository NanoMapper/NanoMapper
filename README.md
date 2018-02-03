# NanoMapper.NET

> A superbly simple, object-to-object mapping library for .NET

NanoMapper alleviates the supremely tedious business of mapping and applying some object (the source) onto some other object (the target).

**Package Manager**

    PM> Install-Package NanoMapper

**.NET CLI**

    > dotnet add package NanoMapper

## The basics

Mappings are accomplished using the `Map(...)` object extension method found in the `NanoMapper` namespace.

    using NanoMapper;

    ... {
      source.Map(target);
    }

NanoMapper **automagically** matches like-for-like properties based on names and property type covariance.

> Note: Anonymous types are immutable and not supported as targets (yet) but can be used as source objects.

### Configuration

All mappings are configured using simple mapping functions which define explicit transforms between source properties and target properties.

Mappings can be configured at the following levels, ordered by precedence.

1. at the instance level using `source.Map(target, map => { ... })`
2. at the [container](#containers) level using `container.Map<Source, Target>(map => { ... })`
3. or globally using `Mapper.Map<Source, Target>(map => { ... })`

> Note: It is a wise practice to only configuring global mappings once during application start-up.

### Mapping properties

NanoMapper matches properties based on their names and property type covariance; properties that exist on both source and target types where the names match where each source property type is assignable to the target property type are automatically mapped, no explicit configuration required.

Map a source-to-target property mapping using the `map.Property(...)` method:

    map.Property(target => target.Property, source => source.OtherProperty);

Property mappings can be highly complex and don't necessarily have to be straight mappings:

    map.Property(target => target.Property, source => (source.Abc + source.Xyz).ToString();

### Ignoring properties

Properties we want to omit from being automatically mapped can be "ignored":

    map.Ignore(target => target.Property);

When you need to include properties that are currently being ignored you can always include them back in:

    // Map this property
    map.Property(target => target.Property, ...);

## Containers

By default, NanoMapper caches mappings within containers so multiple calls to the `Map(...)` methods execute **super-fast**.

The standard `Map(...)` method uses the global container instance cache.

In times when it is necessary to avoid using static constructs, such as during isolated testing or integrating with systems that use / promote dependency injection / IoC containers, we can create a new `IMappingContainer` instance and pass that to the `Map(...)` method instead:

    // Create a new container instance
    var container = Mapper.CreateContainer();

    // Map some mappings
    container.Map<Source, Target>(map => { ... });

    // Use the container
    source.Map(target, container);

By design, instance containers do **not** access the global mappings by default. To utilise globally mappings we pass `enableGlobalMappings: true` to the container creation method.

In doing so the container will check the global cache for existing mapping configurations and apply them first before any instance or overriding mappings.

    // Create a new container instance
    var container = Mapper.CreateContainer(enableGlobalMappings: true);

    // Map some mappings
    container.Map<Source, Target>(map => { ... });

    // Use the container
    source.Map(target, container);

Alternatively, you can pass in any instance of an `IMappingContainer` implementation to use as the parent container.

    // Define a new container
    class CustomMappingContainer : IMappingContainer { }

    // Create the custom (parent) container
    var customContainer = new CustomMappingContainer(...);

    // Create a standard container that uses the custom one
    var container = Mapper.CreateContainer(customContainer);

    // Use the container(s)
    source.Map(target, container);

    // or use the custom one directly
    source.Map(target, customContainer);
    
## Contributions and Support

*NanoMapper.NET is free, open-source software.* ([ISC](https://raw.githubusercontent.com/garydouble/NanoMapper.NET/master/LICENSE))

- Found a bug? [Raise an issue](https://github.com/garydouble/NanoMapper.NET/issues)
- Made an improvement? [Create a pull-request](https://github.com/garydouble/NanoMapper.NET)
- Need some help? [Ask on StackOverflow.com](https://stackoverflow.com/search?q=NanoMapper.NET)

Made with
<span style="color:crimson;font-size:2em;vertical-align:sub;">&hearts;</span> by [Gary Doubl&eacute;](http://garydouble.com).  
Happy Coding!
