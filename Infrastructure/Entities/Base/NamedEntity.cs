using DotLogix.Architecture.Infrastructure.Entities.Options;

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    public abstract class NamedEntity : Entity, INamed
    {
        public string Name { get; set; }
    }
}