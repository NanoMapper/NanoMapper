# NanoMapper.NET

The superbly simple, thread-safe object mapping library for .NET

**Package Manager**

    PM> Install-Package NanoMapper

**.NET CLI**

    > dotnet add package NanoMapper

## The basics

NanoMapper alleviates the supremely tedious business of mapping (applying) one object (the source) onto some other object (the target).

This is accomplished using the `ApplyTo(...)` object extension method.

    source.ApplyTo(target);

NanoMapper **automatically** matches like for like properties and, what is even better, is that NanoMapper caches mappings within containers so multiple `ApplyTo` calls are always **super-fast**.

> Note: Anonymous target types are not supported as they are immutable. However, you can use them as much as you like for source objects.

It is recommended t configure most static mappings once, during the application's start-up process using the global `Mappings.Configure(...)` method.

The `Configure(...)` functions take a mapping function that defines the mappings between the source and target types.

    // Configure the mapping between SourceType and TargetType objects
    Mappings.Configure<SourceType, TargetType>(map => {
      map.Property(...);
      ...
    });

### Mapping properties

By default, properties that exist on both source and target objects will be automatically mapped (providing they match in name and the source property type is assignable to the target property type).

Configure a target's property mapping using the `map.Property(...)` method:

    map.Property(t => t.Property, s => s.OtherProperty);

Here the `t` parameter represents the "target" object instance and the `s` parameter represents the "source" object instance.

Property mappings can also be highly complex and don't necessarily have to be simple property to property mappings:

    map.Property(t => t.Property, s => ComplexFunctionToTranslateSourcePropToTargetProp(s));

### Ignoring properties

Properties we want to omit from being automatically mapped should be "ignored":

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

Instance containers do not access the global mappings by default. In order to utilise any globally defined mappings we need to pass the `enableGlobalMappings` argument as `true`.

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
    
## Contributions and Support

*NanoMapper.NET is free, open-source software.* ([MIT](https://raw.githubusercontent.com/garydouble/NanoMapper.NET/master/LICENSE))

- Found a bug? [Raise an issue](https://github.com/garydouble/NanoMapper.NET/issues)
- Made an improvement? [Create a pull-request](https://github.com/garydouble/NanoMapper.NET)
- Got a question? [Ask on StackOverflow.com](https://stackoverflow.com/search?q=NanoMapper.NET)

Happy Coding!

---
<p style="text-align:center;">Made with
<span style="color:crimson;font-size:2em;vertical-align:sub;">&hearts;</span> by
<a href="http://garydouble.com" style="font-weight:bold;" target="_blank">Gary Doublé</a>.</p>
