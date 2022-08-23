#region using
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#endregion

namespace DotLogix.WebServices.Testing.NUnit.Json; 

public class JsonDiffOptions : ICloneable
{
    /// <summary>
    ///     The default diff options, based on common usage
    /// </summary>
    public static JsonDiffOptions Default => new JsonDiffOptions();

    /// <summary>
    ///     A more restricted version of diff options
    /// </summary>
    public static JsonDiffOptions Exact => new JsonDiffOptions
    {
        DateTimeTolerance = TimeSpan.Zero,
        TimeSpanTolerance = TimeSpan.Zero,
        DecimalTolerance = float.Epsilon
    };

    /// <summary>
    ///     Determines if the comparer should throw for additional properties in actual
    /// </summary>
    public bool UseAdditionalProperties { get; set; }

    /// <summary>
    ///     Determines if the comparer should throw for additional properties in expected
    /// </summary>
    public bool UseMissingProperties { get; set; }

    /// <summary>
    ///     Allows to configure a range of properties to include (full hierarchy, including dictionary keys)
    /// </summary>
    public HashSet<string> PropertyWhitelist { get; set; }

    /// <summary>
    ///     Allows to configure a range of properties to exclude (full hierarchy, including dictionary keys)
    /// </summary>
    public HashSet<string> PropertyBlacklist { get; set; }

    /// <summary>
    ///     Allows to treat empty values the same as null (collections, strings, nullables)
    /// </summary>
    public bool TreatEmptyAsNull { get; set; } = true;

    /// <summary>
    ///     Allows to set a tolerance for date time values
    /// </summary>
    public TimeSpan DateTimeTolerance { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Allows to set a tolerance for time values
    /// </summary>
    public TimeSpan TimeSpanTolerance { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Allows to set a tolerance for floating point values
    /// </summary>
    public double DecimalTolerance { get; set; } = 0.0001d;

    /// <summary>
    ///     Allows to set a tolerance for integer values
    /// </summary>
    public long IntegerTolerance { get; set; }

    /// <summary>
    ///     Allows to set a comparison for string values
    /// </summary>
    public StringComparison StringComparison { get; set; } = StringComparison.Ordinal;

    /// <summary>
    ///     Allows to set specific serializer settings for the comparison
    /// </summary>
    public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings {Formatting = Formatting.Indented};

    /// <summary>
    ///     Creates a new object that is a copy of the current instance.
    /// </summary>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    ///     Creates a new object that is a copy of the current instance.
    /// </summary>
    public JsonDiffOptions Clone()
    {
        return (JsonDiffOptions) MemberwiseClone();
    }
}