using Microsoft.EntityFrameworkCore.Storage;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface ITypeMappingSource {
    RelationalTypeMapping GetMapping(RelationalTypeMappingInfo mappingInfo);
}