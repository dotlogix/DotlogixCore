namespace DotLogix.Core.Rest.Authentication.Jwt.Algorithms
{
    public interface ISigningAlgorithm {
        string Name { get; }
        byte[] CalculateSignature(byte[] data);
        bool ValidateSignature(byte[] data, byte[] signature);
    }
}
