#region using
using DotLogix.WebServices.Testing.NUnit.Json;
using NUnit.Framework.Constraints;
#endregion

namespace DotLogix.WebServices.Testing.NUnit
{
    public static class EqualityConstraintExtensions
    {
        public static JsonDiffConstraint ByJsonDiff(this EqualConstraint constraint, JsonDiffOptions options = null) =>
            constraint.Append(new JsonDiffConstraint(constraint, options));

        private static T Append<T>(this Constraint self, T other) where T : Constraint
        {
            self.Builder?.Append(other);
            return other;
        }
    }
}