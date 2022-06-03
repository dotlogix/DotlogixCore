using DotLogix.WebServices.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public class NamingStrategyConventionSetPlugin : IConventionSetPlugin
{
    private readonly IDbNamingStrategy _namingStrategy;

    public NamingStrategyConventionSetPlugin(IDbNamingStrategy namingStrategy) {
        _namingStrategy = namingStrategy;
    }

    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        var convention = new NamingStrategyConvention(_namingStrategy);
        // entity
        conventionSet.EntityTypeAddedConventions.Add(convention);
        conventionSet.EntityTypeAnnotationChangedConventions.Add(convention);
        conventionSet.EntityTypeBaseTypeChangedConventions.Add(convention);
        
        // properties
        conventionSet.PropertyAddedConventions.Add(convention);

        // constraints
        conventionSet.ForeignKeyAddedConventions.Add(convention);
        conventionSet.ForeignKeyPropertiesChangedConventions.Add(convention);
        conventionSet.ForeignKeyOwnershipChangedConventions.Add(convention);

        conventionSet.KeyAddedConventions.Add(convention);
        conventionSet.IndexAddedConventions.Add(convention);
        conventionSet.IndexUniquenessChangedConventions.Add(convention);
        
        conventionSet.ModelFinalizingConventions.Add(convention);
        return conventionSet;
    }
}