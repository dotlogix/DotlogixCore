using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public class EntityMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin{
    private readonly IEntityQueryTranslator _translator;
    public IEnumerable<IMethodCallTranslator> Translators => _translator.MethodTranslators;

    public EntityMethodCallTranslatorPlugin(IEntityQueryTranslator translator) {
        _translator = translator;
    }
}