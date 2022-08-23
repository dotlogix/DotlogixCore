using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DotLogix.Core.Expressions.Rewriters;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotLogix.WebServices.EntityFramework.Database; 

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class QueryEnhancementOptions : DbContextExtensionOptions {
    private readonly Dictionary<Type, ServiceDescriptor> _evaluatableFilters;
    private readonly Dictionary<Type, ServiceDescriptor> _expressionRewriters;
    private readonly Dictionary<Type, ServiceDescriptor> _memberRewriters;
    private readonly Dictionary<Type, ServiceDescriptor> _memberTranslators;
    private readonly Dictionary<Type, ServiceDescriptor> _methodRewriters;
    private readonly Dictionary<Type, ServiceDescriptor> _methodTranslators;
    private readonly Dictionary<Type, ServiceDescriptor> _typeMappingSource;

    public QueryEnhancementOptions() {
        _memberTranslators = new Dictionary<Type, ServiceDescriptor>();
        _methodTranslators = new Dictionary<Type, ServiceDescriptor>();
        _expressionRewriters = new Dictionary<Type, ServiceDescriptor>();
        _methodRewriters = new Dictionary<Type, ServiceDescriptor>();
        _memberRewriters = new Dictionary<Type, ServiceDescriptor>();
        _evaluatableFilters = new Dictionary<Type, ServiceDescriptor>();
        _typeMappingSource = new Dictionary<Type, ServiceDescriptor>();
    }

    public QueryEnhancementOptions(QueryEnhancementOptions options) {
        _memberRewriters = new Dictionary<Type, ServiceDescriptor>(options._memberRewriters);
        _methodRewriters = new Dictionary<Type, ServiceDescriptor>(options._methodRewriters);
        _expressionRewriters = new Dictionary<Type, ServiceDescriptor>(options._expressionRewriters);
        _memberTranslators = new Dictionary<Type, ServiceDescriptor>(options._memberTranslators);
        _methodTranslators = new Dictionary<Type, ServiceDescriptor>(options._methodTranslators);
        _evaluatableFilters = new Dictionary<Type, ServiceDescriptor>(options._evaluatableFilters);
        _typeMappingSource = new Dictionary<Type, ServiceDescriptor>(options._evaluatableFilters);
    }

    #region MemberRewriters
    public QueryEnhancementOptions UseMemberRewriter<TBase, T>()
        where TBase : class, IMemberRewriter
        where T : class, TBase {
        AddOrReplaceDescriptor<IMemberRewriter, TBase, T>(_memberRewriters);
        return this;
    }

    public QueryEnhancementOptions UseMemberRewriter<T>() where T : class, IMemberRewriter {
        AddOrReplaceDescriptor<IMemberRewriter, T>(_memberRewriters);
        return this;
    }

    public QueryEnhancementOptions UseMemberRewriter<T>(T instance) where T : class, IMemberRewriter {
        AddOrReplaceDescriptor<IMemberRewriter, T>(_memberRewriters, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseMemberRewriter<T>(Func<IServiceProvider, T> factory) where T : class, IMemberRewriter {
        AddOrReplaceDescriptor<IMemberRewriter, T>(_memberRewriters, factory);
        return this;
    }
    #endregion

    #region MethodRewriters
    public QueryEnhancementOptions UseMethodCallRewriter<TBase, T>()
        where TBase : class, IMethodCallRewriter
        where T : class, TBase {
        AddOrReplaceDescriptor<IMethodCallRewriter, TBase, T>(_methodRewriters);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallRewriter<T>() where T : class, IMethodCallRewriter {
        AddOrReplaceDescriptor<IMethodCallRewriter, T>(_methodRewriters);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallRewriter<T>(T instance) where T : class, IMethodCallRewriter {
        AddOrReplaceDescriptor<IMethodCallRewriter, T>(_methodRewriters, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallRewriter<T>(Func<IServiceProvider, T> factory) where T : class, IMethodCallRewriter {
        AddOrReplaceDescriptor<IMethodCallRewriter, T>(_methodRewriters, factory);
        return this;
    }
    #endregion

    #region ExpressionRewriters
    public QueryEnhancementOptions UseExpressionRewriter<TBase, T>()
        where TBase : class, IExpressionRewriter
        where T : class, TBase {
        AddOrReplaceDescriptor<IExpressionRewriter, TBase, T>(_expressionRewriters);
        return this;
    }

    public QueryEnhancementOptions UseExpressionRewriter<T>() where T : class, IExpressionRewriter {
        AddOrReplaceDescriptor<IExpressionRewriter, T>(_expressionRewriters);
        return this;
    }

    public QueryEnhancementOptions UseExpressionRewriter<T>(T instance) where T : class, IExpressionRewriter {
        AddOrReplaceDescriptor<IExpressionRewriter, T>(_expressionRewriters, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseExpressionRewriter<T>(Func<IServiceProvider, T> factory) where T : class, IExpressionRewriter {
        AddOrReplaceDescriptor<IExpressionRewriter, T>(_expressionRewriters, factory);
        return this;
    }
    #endregion

    #region MemberTranslators
    public QueryEnhancementOptions UseMemberTranslator<TBase, T>()
        where TBase : class, IMemberTranslator
        where T : class, TBase {
        AddOrReplaceDescriptor<IMemberTranslator, TBase, T>(_memberTranslators);
        return this;
    }

    public QueryEnhancementOptions UseMemberTranslator<T>() where T : class, IMemberTranslator {
        AddOrReplaceDescriptor<IMemberTranslator, T>(_memberTranslators);
        return this;
    }

    public QueryEnhancementOptions UseMemberTranslator<T>(T instance) where T : class, IMemberTranslator {
        AddOrReplaceDescriptor<IMemberTranslator, T>(_memberTranslators, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseMemberTranslator<T>(Func<IServiceProvider, T> factory) where T : class, IMemberTranslator {
        AddOrReplaceDescriptor<IMemberTranslator, T>(_memberTranslators, factory);
        return this;
    }
    #endregion

    #region MethodRewriters
    public QueryEnhancementOptions UseMethodCallTranslator<TBase, T>()
        where TBase : class, IMethodCallTranslator
        where T : class, TBase {
        AddOrReplaceDescriptor<IMethodCallTranslator, TBase, T>(_methodTranslators);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallTranslator<T>() where T : class, IMethodCallTranslator {
        AddOrReplaceDescriptor<IMethodCallTranslator, T>(_methodTranslators);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallTranslator<T>(T instance) where T : class, IMethodCallTranslator {
        AddOrReplaceDescriptor<IMethodCallTranslator, T>(_methodTranslators, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseMethodCallTranslator<T>(Func<IServiceProvider, T> factory) where T : class, IMethodCallTranslator {
        AddOrReplaceDescriptor<IMethodCallTranslator, T>(_methodTranslators);
        return this;
    }
    #endregion

    #region EvaluatableFilters
    public QueryEnhancementOptions UseEvaluatableFilter<TBase, T>()
        where TBase : class, IEvaluatableFilter
        where T : class, TBase {
        AddOrReplaceDescriptor<IEvaluatableFilter, TBase, T>(_evaluatableFilters);
        return this;
    }

    public QueryEnhancementOptions UseEvaluatableFilter<T>() where T : class, IEvaluatableFilter {
        AddOrReplaceDescriptor<IEvaluatableFilter, T>(_evaluatableFilters);
        return this;
    }

    public QueryEnhancementOptions UseEvaluatableFilter<T>(T instance) where T : class, IEvaluatableFilter {
        AddOrReplaceDescriptor<IEvaluatableFilter, T>(_evaluatableFilters, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseEvaluatableFilter<T>(Func<IServiceProvider, T> factory) where T : class, IEvaluatableFilter {
        AddOrReplaceDescriptor<IEvaluatableFilter, T>(_evaluatableFilters);
        return this;
    }
    #endregion

    #region TypeMappingSources
    public QueryEnhancementOptions UseTypeMappingSource<TBase, T>()
        where TBase : class, ITypeMappingSource
        where T : class, TBase {
        AddOrReplaceDescriptor<ITypeMappingSource, TBase, T>(_typeMappingSource);
        return this;
    }

    public QueryEnhancementOptions UseTypeMappingSource<T>() where T : class, ITypeMappingSource {
        AddOrReplaceDescriptor<ITypeMappingSource, T>(_typeMappingSource);
        return this;
    }

    public QueryEnhancementOptions UseTypeMappingSource<T>(T instance) where T : class, ITypeMappingSource {
        AddOrReplaceDescriptor<ITypeMappingSource, T>(_typeMappingSource, _ => instance);
        return this;
    }

    public QueryEnhancementOptions UseTypeMappingSource<T>(Func<IServiceProvider, T> factory) where T : class, ITypeMappingSource {
        AddOrReplaceDescriptor<ITypeMappingSource, T>(_typeMappingSource);
        return this;
    }
    #endregion

    #region Extension
    protected override void ApplyServices(IServiceCollection services) {
        services.TryAddEnumerable(_expressionRewriters.Values);
        services.TryAddEnumerable(_memberRewriters.Values);
        services.TryAddEnumerable(_methodRewriters.Values);
        services.TryAddEnumerable(_memberTranslators.Values);
        services.TryAddEnumerable(_methodTranslators.Values);
        services.TryAddEnumerable(_evaluatableFilters.Values);
        services.TryAddEnumerable(_typeMappingSource.Values);
        services.TryAddSingleton<IEntityQueryRewriterPlugin, EntityQueryRewriterPlugin>();

        var relationalServicesBuilder = new EntityFrameworkRelationalServicesBuilder(services);
        relationalServicesBuilder
           .TryAdd<IQueryCompiler, EntityQueryCompiler>()
           .TryAdd<IMemberTranslatorPlugin, EntityMemberTranslatorPlugin>()
           .TryAdd<IMethodCallTranslatorPlugin, EntityMethodCallTranslatorPlugin>()
           .TryAdd<IEvaluatableExpressionFilterPlugin, EntityEvaluatableFilterPlugin>()
           .TryAdd<IRelationalTypeMappingSourcePlugin, EntityTypeMappingSourcePlugin>();
    }

    protected override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
        if(_expressionRewriters.Count > 0) {
            debugInfo["ExpressionRewriters"] = string.Join(", ", _expressionRewriters.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_methodRewriters.Count > 0) {
            debugInfo["MethodCallRewriters"] = string.Join(", ", _methodRewriters.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_memberRewriters.Count > 0) {
            debugInfo["MemberRewriters"] = string.Join(", ", _memberRewriters.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_methodTranslators.Count > 0) {
            debugInfo["MethodCallTranslators"] = string.Join(", ", _methodTranslators.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_memberTranslators.Count > 0) {
            debugInfo["MemberTranslators"] = string.Join(", ", _memberTranslators.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_evaluatableFilters.Count > 0) {
            debugInfo["EvaluatableFilters"] = string.Join(", ", _evaluatableFilters.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }

        if(_typeMappingSource.Count > 0) {
            debugInfo["TypeMappingSources"] = string.Join(", ", _typeMappingSource.Keys.OrderBy(t => t.Name).Select(t => t.GetFriendlyName()));
        }
    }

    protected override int GetServiceProviderHashCode() {
        var hashCode = new HashCode();
        hashCode.Add(DescriptorsHashCode(_expressionRewriters));
        hashCode.Add(DescriptorsHashCode(_memberRewriters));
        hashCode.Add(DescriptorsHashCode(_methodRewriters));
        hashCode.Add(DescriptorsHashCode(_memberTranslators));
        hashCode.Add(DescriptorsHashCode(_methodTranslators));
        hashCode.Add(DescriptorsHashCode(_evaluatableFilters));
        hashCode.Add(DescriptorsHashCode(_typeMappingSource));
        return hashCode.ToHashCode();

        static int DescriptorsHashCode(IReadOnlyDictionary<Type, ServiceDescriptor> descriptors) {
            if(descriptors.Count == 0) {
                return 0;
            }

            var descriptorHashCode = new HashCode();
            foreach(var type in descriptors.Keys.OrderBy(t => t!.Name)) {
                descriptorHashCode.Add(type);
            }

            return descriptorHashCode.ToHashCode();
        }
    }

    protected override bool ShouldUseSameServiceProvider(IDbContextExtensionOptions options) {
        return options is QueryEnhancementOptions enhancementOptions && DescriptorsEqual(enhancementOptions._expressionRewriters, _expressionRewriters) && DescriptorsEqual(enhancementOptions._memberRewriters, _memberRewriters) && DescriptorsEqual(enhancementOptions._methodRewriters, _methodRewriters) && DescriptorsEqual(enhancementOptions._memberTranslators, _memberTranslators) && DescriptorsEqual(enhancementOptions._methodTranslators, _methodTranslators);

        static bool DescriptorsEqual(IReadOnlyDictionary<Type, ServiceDescriptor> descriptors, IReadOnlyDictionary<Type, ServiceDescriptor> otherDescriptors) {
            if(descriptors.Count != otherDescriptors.Count) {
                return false;
            }

            return (descriptors.Count == 0) || descriptors.Keys.All(otherDescriptors.ContainsKey);
        }
    }
    #endregion

    #region Helper
    private static void AddOrReplaceDescriptor<TService, TBase, TImplementation>(Dictionary<Type, ServiceDescriptor> descriptors)
        where TService : class
        where TBase : class, TService
        where TImplementation : class, TBase {
        var descriptor = ServiceDescriptor.Singleton<TService, TImplementation>();
        descriptors[typeof(TBase)] = descriptor;
    }

    private static void AddOrReplaceDescriptor<TService, TImplementation>(Dictionary<Type, ServiceDescriptor> descriptors)
        where TService : class
        where TImplementation : class, TService {
        var descriptor = ServiceDescriptor.Singleton<TService, TImplementation>();
        descriptors[descriptor.ImplementationType!] = descriptor;
    }

    private static void AddOrReplaceDescriptor<TService, TImplementation>(Dictionary<Type, ServiceDescriptor> descriptors, Func<IServiceProvider, TImplementation> factoryFunc)
        where TService : class
        where TImplementation : class, TService {
        var descriptor = ServiceDescriptor.Singleton<TService, TImplementation>(factoryFunc);
        descriptors[descriptor.ImplementationType!] = descriptor;
    }
    #endregion
}