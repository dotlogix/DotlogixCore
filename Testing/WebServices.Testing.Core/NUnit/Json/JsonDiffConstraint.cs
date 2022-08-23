#region using
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
#endregion

namespace DotLogix.WebServices.Testing.NUnit.Json; 

public class JsonDiffConstraint : Constraint
{
    private readonly JsonDiffOptions _options;

    public JsonDiffConstraint(EqualConstraint parent, JsonDiffOptions options = null)
        : base(parent.Arguments)
    {
        _options = options is not null ? options.Clone() : JsonDiffOptions.Default;
    }

    public override string DisplayName => "Equivalent by serialized json model";
    public override string Description => "Required equivalence by serialized json model";

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var expected = Arguments.FirstOrDefault();
        var result = JsonDiff.Diff(expected, actual, _options);
        return new JsonEqualityConstraintResult(this, result, actual);
    }

    /// <summary>
    ///     Determines if the comparer should throw for additional properties in actual
    /// </summary>
    public JsonDiffConstraint WithAdditionalProperties(bool shouldThrow = true)
    {
        _options.UseAdditionalProperties = shouldThrow;
        return this;
    }

    /// <summary>
    ///     Determines if the comparer should throw for additional properties in expected
    /// </summary>
    public JsonDiffConstraint WithMissingProperties(bool shouldThrow = true)
    {
        _options.UseMissingProperties = shouldThrow;
        return this;
    }

    /// <summary>
    ///     Determines if the comparer should throw for additional/missing properties in actual
    ///     Shortcut for WithAdditionalProperties(!ignoreOthers).WithAdditionalProperties(!ignoreOthers)
    /// </summary>
    public JsonDiffConstraint WithCommonProperties(bool ignoreOthers = true)
    {
        _options.UseAdditionalProperties = !ignoreOthers;
        _options.UseMissingProperties = !ignoreOthers;
        return this;
    }

    /// <summary>
    ///     Allows to configure a range of properties to include (full hierarchy, including dictionary keys)
    /// </summary>
    public JsonDiffConstraint WithPropertyWhitelist(params string[] whitelist)
    {
        return WithPropertyWhitelist(whitelist.AsEnumerable());
    }

    /// <summary>
    ///     Allows to configure a range of properties to include (full hierarchy, including dictionary keys)
    /// </summary>
    public JsonDiffConstraint WithPropertyWhitelist(IEnumerable<string> whitelist)
    {
        if (_options.PropertyWhitelist == null) _options.PropertyWhitelist = new HashSet<string>();
        _options.PropertyWhitelist.UnionWith(whitelist);
        return this;
    }

    /// <summary>
    ///     Allows to configure a range of properties to exclude (full hierarchy, including dictionary keys)
    /// </summary>
    public JsonDiffConstraint WithPropertyBlacklist(params string[] blacklist)
    {
        return WithPropertyBlacklist(blacklist.AsEnumerable());
    }

    /// <summary>
    ///     Allows to configure a range of properties to exclude (full hierarchy, including dictionary keys)
    /// </summary>
    public JsonDiffConstraint WithPropertyBlacklist(IEnumerable<string> blacklist)
    {
        if (_options.PropertyBlacklist == null) _options.PropertyBlacklist = new HashSet<string>();
        _options.PropertyBlacklist.UnionWith(blacklist);
        return this;
    }

    /// <summary>
    ///     Allows to treat empty values the same as null (collections, strings, nullables)
    /// </summary>
    public JsonDiffConstraint WithEmptyAsNull(bool treatAsEqual = true)
    {
        _options.TreatEmptyAsNull = treatAsEqual;
        return this;
    }

    /// <summary>
    ///     Allows to set a tolerance for date time values
    /// </summary>
    public JsonDiffConstraint WithDateTolerance(TimeSpan tolerance)
    {
        _options.DateTimeTolerance = tolerance;
        return this;
    }

    /// <summary>
    ///     Allows to set a tolerance for time values
    /// </summary>
    public JsonDiffConstraint WithTimeTolerance(TimeSpan tolerance)
    {
        _options.DateTimeTolerance = tolerance;
        return this;
    }

    /// <summary>
    ///     Allows to set a tolerance for floating-point values
    /// </summary>
    public JsonDiffConstraint WithDecimalTolerance(double tolerance)
    {
        _options.DecimalTolerance = tolerance;
        return this;
    }

    /// <summary>
    ///     Allows to set a tolerance for integer values
    /// </summary>
    public JsonDiffConstraint WithIntegerTolerance(long tolerance)
    {
        _options.IntegerTolerance = tolerance;
        return this;
    }

    /// <summary>
    ///     Allows to set a comparison for string values
    /// </summary>
    public JsonDiffConstraint WithStringComparison(StringComparison comparison)
    {
        _options.StringComparison = comparison;
        return this;
    }
}