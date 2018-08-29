using System;
using System.Security.Cryptography;
using System.Text;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Jwt.Algorithms {
    public class HmacSigningAlgorithm : SigningAlgorithmBase {
        private readonly HMAC _hmac;
        public HmacSigningAlgorithm(string key, HmacAlgorithm algorithm) : base(algorithm.ToString().ToLower()) {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            switch(algorithm) {
                case HmacAlgorithm.Hs256:
                    _hmac = new HMACSHA256(keyBytes);
                    break;
                case HmacAlgorithm.Hs384:
                    _hmac = new HMACSHA384(keyBytes);
                    break;
                case HmacAlgorithm.Hs512:
                    _hmac = new HMACSHA512(keyBytes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
        }
        public override byte[] CalculateSignature(byte[] data) {
            return _hmac.ComputeHash(data);
        }
    }
}