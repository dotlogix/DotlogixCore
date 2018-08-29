namespace DotLogix.Core.Rest.Services.Processors.Authentication.Jwt.Algorithms
{
    public interface ISigningAlgorithm {
        string Name { get; }
        byte[] CalculateSignature(byte[] data);
        bool ValidateSignature(byte[] data, byte[] signature);
    }
}
