using System.Collections.Generic;
using DotLogix.WebServices.EntityFramework.Expressions.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

public class EntityMemberTranslatorPlugin : IMemberTranslatorPlugin {
    private readonly IEntityQueryTranslator _translator;
    public IEnumerable<IMemberTranslator> Translators => _translator.MemberTranslators;

    public EntityMemberTranslatorPlugin(IEntityQueryTranslator translator) {
        _translator = translator;
    }

}