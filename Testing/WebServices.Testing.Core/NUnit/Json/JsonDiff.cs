#region using
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endregion

namespace DotLogix.WebServices.Testing.NUnit.Json; 

public static class JsonDiff
{
    public static JsonDiffResult Diff(object expected, object actual, JsonDiffOptions options = null)
    {
        if (options == null)
        {
            options = JsonDiffOptions.Default;
        }

        var serializer = JsonSerializer.CreateDefault(options.SerializerSettings);
        var expectedToken = ToJToken(expected, serializer);
        var actualToken = ToJToken(actual, serializer);
        return Diff(expectedToken, actualToken, options);
    }

    public static JsonDiffResult Diff(JToken expected, JToken actual, JsonDiffOptions options = null)
    {
        if (options == null)
        {
            options = JsonDiffOptions.Default;
        }
        return DiffRecursive(expected, actual, options);
    }


    private static JsonDiffResult DiffRecursive(JToken expected, JToken actual, JsonDiffOptions options) {
        if (HasTypeDifferences(ref expected, ref actual, options))
            return new JsonDiffResult
            {
                Expected = expected,
                Actual = actual,
                Options = options,
                ExpectedDifferences = expected.DeepClone(),
                ActualDifferences = actual.DeepClone()
            };

        return expected.Type switch {
            JTokenType.Object => DiffObjectRecursive((JObject)expected, (JObject)actual, options),
            JTokenType.Array => DiffArrayRecursive((JArray)expected, (JArray)actual, options),
            _ => DiffValueRecursive((JValue)expected, (JValue)actual, options)
        };
    }

    private static JsonDiffResult DiffObjectRecursive(JObject expected, JObject actual, JsonDiffOptions options)
    {
        var expectedDiff = new JObject();
        var actualDiff = new JObject();

        var expectedOnly = new HashSet<string>(((IDictionary<string, JToken>) expected).Keys);
        var actualOnly = new HashSet<string>(((IDictionary<string, JToken>) actual).Keys);

        if (options.PropertyWhitelist is { Count: > 0 })
        {
            expectedOnly.IntersectWith(options.PropertyWhitelist);
            actualOnly.IntersectWith(options.PropertyWhitelist);
        }

        if (options.PropertyBlacklist is { Count: > 0 })
        {
            expectedOnly.ExceptWith(options.PropertyBlacklist);
            actualOnly.ExceptWith(options.PropertyBlacklist);
        }

        var common = new HashSet<string>(expectedOnly);
        common.IntersectWith(actualOnly);

        var hasDifferences = false;
        if (options.UseMissingProperties)
        {
            // add missing properties
            expectedOnly.ExceptWith(common);
            foreach (var propertyName in expectedOnly)
            {
                var expectedProperty = expected.GetValue(propertyName);
                expectedDiff.Add(propertyName, expectedProperty.DeepClone());
                hasDifferences = true;
            }
        }

        if (options.UseAdditionalProperties)
        {
            // add additional properties
            actualOnly.ExceptWith(common);
            foreach (var propertyName in actualOnly)
            {
                var actualProperty = actual.GetValue(propertyName).DeepClone();
                actualDiff.Add(propertyName, actualProperty);
                hasDifferences = true;
            }
        }

        // diff common properties
        foreach (var propertyName in common)
        {
            var expectedProperty = expected.GetValue(propertyName);
            var actualProperty = actual.GetValue(propertyName).DeepClone();

            var nestedResult = DiffRecursive(expectedProperty, actualProperty, options);
            if (nestedResult.HasDifferences == false) continue;

            expectedDiff.Add(propertyName, nestedResult.ExpectedDifferences);
            actualDiff.Add(propertyName, nestedResult.ActualDifferences);
            hasDifferences = true;
        }

        return new JsonDiffResult
        {
            HasDifferences = hasDifferences,
            Expected = expected,
            Actual = actual,
            Options = options,
            ExpectedDifferences = expectedDiff,
            ActualDifferences = actualDiff
        };
    }

    private static JsonDiffResult DiffArrayRecursive(JArray expected, JArray actual, JsonDiffOptions options)
    {
        var expectedDiff = new JArray();
        var actualDiff = new JArray();
        var hasDifferences = false;

        // add missing values
        var itemsToDiff = Math.Min(expected.Count, actual.Count);
        for (var i = 0; i < itemsToDiff; i++)
        {
            var nestedResult = DiffRecursive(expected[i], actual[i], options);
            if (nestedResult.HasDifferences == false) continue;

            expectedDiff.Add(nestedResult.ExpectedDifferences);
            actualDiff.Add(nestedResult.ActualDifferences);
            hasDifferences = true;
        }

        // add additional values
        for (var i = actual.Count; i < expected.Count; i++)
        {
            expectedDiff.Add(expected[i].DeepClone());
            hasDifferences = true;
        }

        // add additional values
        for (var i = expected.Count; i < actual.Count; i++)
        {
            actualDiff.Add(actual[i].DeepClone());
            hasDifferences = true;
        }

        return new JsonDiffResult
        {
            HasDifferences = hasDifferences,
            Expected = expected,
            Actual = actual,
            Options = options,
            ExpectedDifferences = expectedDiff,
            ActualDifferences = actualDiff
        };
    }

    private static JsonDiffResult DiffValueRecursive(JValue expected, JValue actual, JsonDiffOptions options)
    {
        var hasDifferences = false;
        switch (expected.Type)
        {
            case JTokenType.Null:
                break;
            case JTokenType.Bytes:
                var expectedValueB = expected.Value<byte[]>();
                var actualValueB = actual.Value<byte[]>();

                hasDifferences = expectedValueB.SequenceEqual(actualValueB) == false;
                break;
            case JTokenType.Float:
                var expectedValueF = expected.Value<double>();
                var actualValueF = actual.Value<double>();

                hasDifferences = Math.Abs(expectedValueF - actualValueF) > options.DecimalTolerance;
                break;
            case JTokenType.Integer:
                var expectedValueL = expected.Value<long>();
                var actualValueL = actual.Value<long>();

                hasDifferences = Math.Abs(expectedValueL - actualValueL) > options.IntegerTolerance;
                break;
            case JTokenType.String:
                var expectedValueS = expected.Value<string>();
                var actualValueS = actual.Value<string>();

                hasDifferences = string.Equals(expectedValueS, actualValueS, options.StringComparison) == false;
                break;
            case JTokenType.TimeSpan:
                var expectedValueTs = expected.Value<TimeSpan>();
                var actualValueTs = actual.Value<TimeSpan>();

                hasDifferences = Math.Abs((expectedValueTs - actualValueTs).TotalSeconds) > options.TimeSpanTolerance.TotalSeconds;
                break;
            case JTokenType.Date:
                var expectedValueDt = expected.Value<DateTime>();
                var actualValueDt = actual.Value<DateTime>();

                hasDifferences = Math.Abs((expectedValueDt - actualValueDt).TotalSeconds) > options.DateTimeTolerance.TotalSeconds;
                break;
            default:
                var expectedValue = expected;
                var actualValue = actual;
                hasDifferences = Equals(expectedValue.Value, actualValue.Value) == false;
                break;
        }

        return new JsonDiffResult
        {
            HasDifferences = hasDifferences,
            Expected = expected,
            Actual = actual,
            Options = options,
            ExpectedDifferences = expected.DeepClone(),
            ActualDifferences = actual.DeepClone()
        };
    }

    private static bool HasTypeDifferences(ref JToken expected, ref JToken actual, JsonDiffOptions options)
    {
        if (expected.Type == actual.Type)
        {
            return false;
        }

        if (options.TreatEmptyAsNull == false)
        {
            return true;
        }

        if (expected.Type == JTokenType.Null && IsEquivalentToNull(actual))
        {
            actual = JValue.CreateNull();
            return false;
        }

        if (actual.Type == JTokenType.Null && IsEquivalentToNull(expected))
        {
            expected = JValue.CreateNull();
            return false;
        }

        return true;
    }

    private static bool IsEquivalentToNull(JToken value)
    {
        switch (value.Type)
        {
            case JTokenType.None:
            case JTokenType.Null:
            case JTokenType.Undefined:
                return true;
            case JTokenType.Array:
                return ((JArray) value).Count == 0;
            case JTokenType.String:
                return string.IsNullOrEmpty(((JValue) value).Value<string>());
            default:
                return false;
        }
    }

    private static JToken ToJToken(object expected, JsonSerializer serializer) {
        return expected switch {
            null => JValue.CreateNull(),
            JToken jToken => jToken,
            _ => JToken.FromObject(expected, serializer)
        };
    }
}