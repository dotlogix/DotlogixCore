using System;
using System.Security.Cryptography;
using System.Text;

namespace DotLogix.Core.Rest.Authentication.Jwt.Algorithms {
    public class HmacSigningAlgorithm : SigningAlgorithmBase {
        public HmacAlgorithm Algorithm { get; }
        public byte[] KeyBytes { get; }

        public HmacSigningAlgorithm(string key, HmacAlgorithm algorithm) : base(algorithm.ToString().ToLower()) {
            Algorithm = algorithm;
            KeyBytes = Encoding.UTF8.GetBytes(key);
        }
        public override byte[] CalculateSignature(byte[] data) {
            HMAC hmac;
            switch (Algorithm) {
                case HmacAlgorithm.Hs256:
                    hmac = new HMACSHA256(KeyBytes);
                    break;
                case HmacAlgorithm.Hs384:
                    hmac = new HMACSHA384(KeyBytes);
                    break;
                case HmacAlgorithm.Hs512:
                    hmac = new HMACSHA512(KeyBytes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Algorithm), Algorithm, null);
            }
            return hmac.ComputeHash(data);
        }
    }
}