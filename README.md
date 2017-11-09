# NanoMapper.NET

Super small object mapping library for .NET

## Installing

	nuget install NanoMapper

### Copying object

	source.CopyTo(target);
	
### Mapping objects

	source.CopyTo(target, map => {
		// configure one time mapping overrides here...
	});
	
	Mapper.Configure<SourceType, TargetType>(map => {
		// configure custom global mappings here...
	});

### Mapping properties

	map.Property(t => t.Property);

	map.Property(t => t.Property, s => s.OtherProperty);
	
	map.Property(t => t.Property, s => s.OtherPropertyWithDifferentType, v => CodeToTranslateSourceTypeValueToTargetTypeValue(v));

	map.Property(t => t.Property, s => CodeToTranslateSourceTypeToTargetTypeValue(s));

### Ignoring properties

	map.Ignore(o => o.Property);

### Testing and dependency injection

	var mapper = new Mapper();

	mapper.Configure<SourceType, TargetType>(map => {
		// configure instance scoped mappings here...
	});

	source.CopyTo(target, mapper);

