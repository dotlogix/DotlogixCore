using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.WebServices.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class DbModelBuilderExtensions {
    public static ReferenceCollectionBuilder<TOne, TMany> HasManyToOneRelation<TMany, TOne>(this EntityTypeBuilder<TMany> builder,
        Expression<Func<TMany, object>> foreignKeySelector,
        Expression<Func<TOne, object>> principleKeySelector)
        where TMany : class
        where TOne : class {
        return builder
           .HasOne<TOne>()
           .WithMany()
           .HasForeignKey(foreignKeySelector)
           .HasPrincipalKey(principleKeySelector);
    }

    public static ReferenceCollectionBuilder<TOne, TMany> HasOneToManyRelation<TOne, TMany>(this EntityTypeBuilder<TOne> builder,
        Expression<Func<TMany, object>> foreignKeySelector,
        Expression<Func<TOne, object>> principleKeySelector)
        where TMany : class
        where TOne : class {
        return builder
           .HasMany<TMany>()
           .WithOne()
           .HasForeignKey(foreignKeySelector)
           .HasPrincipalKey(principleKeySelector);
    }

    public static ReferenceReferenceBuilder<TPrincipal, TChild> HasOneToOneRelation<TPrincipal, TChild>(this EntityTypeBuilder<TPrincipal> builder,
        Expression<Func<TPrincipal, object>> principalKeySelector,
        Expression<Func<TChild, object>> keySelector, bool required = true)
        where TPrincipal : class
        where TChild : class {
        return builder
           .HasOne<TChild>()
           .WithOne()
           .HasForeignKey(principalKeySelector).IsRequired(required)
           .HasPrincipalKey(keySelector).IsRequired(required);
    }

    public static IndexBuilder<TEntity> HasIndex<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> keySelectorExpression, bool isUnique = false)
        where TEntity : class {
        return builder
           .HasIndex(keySelectorExpression)
           .IsUnique(isUnique);
    }

    public static IndexBuilder<TEntity> HasUniqueIndex<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> keySelectorExpression)
        where TEntity : class {
        return builder
           .HasIndex(keySelectorExpression)
           .IsUnique();
    }
        
    public static EntityTypeBuilder<T> UseUtcDateTimes<T>(this EntityTypeBuilder<T> typeBuilder) where T : class
    {
        var entityType = typeof(T);
        foreach (var property in entityType.GetProperties())
        {
            if (property.IsDefined(typeof(NotMappedAttribute)))
            {
                continue;
            }

            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                continue;
            }

            var propertyBuilder = typeBuilder.Property(property.Name);
            propertyBuilder.HasConversion(DateTimeKindSpecifyConverter.Utc);
        }

        return typeBuilder;
    }
        
    public static EntityTypeBuilder<T> UseStringUuids<T>(this EntityTypeBuilder<T> typeBuilder) where T : class
    {
        var entityType = typeof(T);
        foreach (var property in entityType.GetProperties())
        {
            if (property.IsDefined(typeof(NotMappedAttribute)))
            {
                continue;
            }

            if (property.PropertyType != typeof(Guid) && property.PropertyType != typeof(Guid?))
            {
                continue;
            }

            var propertyBuilder = typeBuilder.Property(property.Name);
            propertyBuilder.HasColumnType("char(36)");
        }
        return typeBuilder;
    }
}