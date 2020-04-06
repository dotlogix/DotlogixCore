using DotLogix.Core.Extensions;

namespace DotLogix.Core.Rest.Authentication.Jwt.Algorithms {
    public abstract class SigningAlgorithmBase : ISigningAlgorithm {
        protected SigningAlgorithmBase(string name) {
            Name = name;
        }
        public string Name { get; }
        public abstract byte[] CalculateSignature(byte[] data);

        public virtual bool ValidateSignature(byte[] data, byte[] signature) {
            var newSignature = CalculateSignature(data);
            return newSignature.EqualBytesLongUnrolled(signature);
        }
    }
}