# NanoMapper.NET

The superbly simple, thread-safe object mapping library for .NET

**Package Manager**

    PM> Install-Package NanoMapper

**.NET CLI**

    > dotnet add package NanoMapper

## The basics

NanoMapper alleviates the supremely tedious business of mapping (applying) one object (the source) onto some other object (the target).

This is accomplished using the `ApplyTo(...)` object extension method found in the `NanoMapper` namespace.

    using NanoMapper;

    ... {
      source.ApplyTo(target);
    }

NanoMapper **automatically** matches like for like properties and, what is even better, is that NanoMapper caches mappings within containers so multiple `ApplyTo` calls are always **super-fast**.

> Note: Anonymous target types are not supported as they are immutable. However, you can use them as much as you like for source objects.

To configure mappings once during the application's start-up process use the global `Mappings.Configure<Source, Target>(...)` method.

The `Configure<Source, Target>(...)` methods take mapping functions that defines the mappings between the source and target types.

    // Configure the mapping between SourceType and TargetType objects
    Mappings.Configure<Source, Target>(map => {
      map.Property(...);
      ...
    });

### Mapping properties

By default, properties that exist on both source and target objects will be automatically mapped (providing they match in name and the source property type is assignable to the target property type).

Configure a target's property mapping using the `map.Property(...)` method:

    Mappings.Configure<Source, Target>(map => {
        map.Property(t => t.Property, s => s.OtherProperty);
    });

Here the `t` represents the "target" object instance and the `s` represents the "source" object instance.

Property mappings can also be highly complex and don't necessarily have to be simple property to property mappings:

    Mappings.Configure<Source, Target>(map => {
        map.Property(t => t.Property, s => ComplexFunctionToTranslateSourcePropToTargetProp(s));
    });

### Ignoring properties

Properties we want to omit from being automatically mapped can be "ignored":

    map.Ignore(t => t.Property);

#### Local mapping overrides

To configure "on-the-fly" mappings we use an `ApplyTo(...)` overload that allows us to pass in an additional mapping configuration function:

    // Apply the object using specific mapping overrides
    source.ApplyTo(target, map => { ... });

This is especially useful when you need to include properties that are currently being ignored:

    // Map this property
    map.Property(t => t.Property, ...);

## Containers

The default `ApplyTo(...)` method uses the global container instance.

In times when it is necessary to avoid using static constructs, such as during testing or integrating with systems that rely on dependency injection / IoC containers, we can create a new `IMappingContainer` instance and pass that to the `ApplyTo(...)` method instead:

    // Create a new container instance
    var container = Mappings.CreateContainer();

    // Configure some mappings
    container.Configure<SourceType, TargetType>(map => { ... });

    // Use the container
    source.ApplyTo(target, container);

Instance containers do not access the global mappings by default. To utilise globally mappings we pass `enableGlobalMappings: true` to the container creation method.

In doing so the container will check the global cache for existing mapping configurations and apply them first before any instance or overriding mappings.

    // Create a new container instance
    var container = Mappings.CreateContainer(enableGlobalMappings: true);

    // Configure some mappings
    container.Configure<SourceType, TargetType>(map => { ... });

    // Use the container
    source.ApplyTo(target, container);

Alternatively, you can pass in any instance of an `IMappingContainer` implementation to use as the parent container.

    // Define a new container
    class CustomMappingContainer : IMappingContainer { }

    // Create the custom (parent) container
    var customContainer = new CustomMappingContainer(...);

    // Create a standard container that uses the custom one
    var container = Mappings.CreateContainer(customContainer);

    // Use the container(s)
    source.ApplyTo(target, container);

    // or use the custom one directly
    source.ApplyTo(target, customContainer);
    
## Contributions and Support

*NanoMapper.NET is free, open-source software.* ([MIT](https://raw.githubusercontent.com/garydouble/NanoMapper.NET/master/LICENSE))

- Found a bug? [Raise an issue](https://github.com/garydouble/NanoMapper.NET/issues)
- Made an improvement? [Create a pull-request](https://github.com/garydouble/NanoMapper.NET)
- Got a question? [Ask on StackOverflow.com](https://stackoverflow.com/search?q=NanoMapper.NET)

Made with
<span style="color:crimson;font-size:2em;vertical-align:sub;">&hearts;</span> by [Gary Doublé](http://garydouble.com).  
Happy Coding!
