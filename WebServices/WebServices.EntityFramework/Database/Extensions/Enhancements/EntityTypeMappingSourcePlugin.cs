using System.Collections.Generic;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class EntityTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin {
    private readonly IReadOnlyCollection<ITypeMappingSource> _sources;

    public EntityTypeMappingSourcePlugin(IEnumerable<ITypeMappingSource> sources) {
        _sources = sources.AsReadOnlyCollection();
    }

    public RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) {
        if(_sources.Count == 0) {
            return default;
        }
        foreach(var source in _sources) {
            if(source.GetMapping(mappingInfo) is { } mapping) {
                return mapping;
            }
        }
        return default;
    }
}