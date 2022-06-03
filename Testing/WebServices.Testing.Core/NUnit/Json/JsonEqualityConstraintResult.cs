#region using
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
#endregion

namespace DotLogix.WebServices.Testing.NUnit.Json
{
    public class JsonEqualityConstraintResult : ConstraintResult
    {
        private readonly JsonDiffResult _result;

        public JsonEqualityConstraintResult(IConstraint constraint, JsonDiffResult result, object actualValue)
            : base(constraint, actualValue, result?.HasDifferences != true)
        {
            _result = result;
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteLine(Description);
            writer.WriteLine();
            writer.Write($"Expected: {_result.Expected.ToString(Formatting.None)}");
            writer.WriteLine();
            writer.WriteLine();
            writer.Write($"But was: {_result.Actual.ToString(Formatting.None)}");
            writer.WriteLine();
            writer.WriteLine();

            writer.Write($"Missing: {_result.ExpectedDifferences}");
            writer.WriteLine();
            writer.Write($"Additional: {_result.ActualDifferences}");
        }
    }
}