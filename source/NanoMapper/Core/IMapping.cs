
namespace NanoMapper.Core {
    
    /// <summary>
    /// Represents a generic mapping.
    /// </summary>
    public interface IMapping { }

    /// <summary>
    /// Represents transform mappings for source to target application.
    /// </summary>
    public interface IMapping<TSource, TTarget> {
    
        void Apply(TSource source, TTarget target);
    }

}
