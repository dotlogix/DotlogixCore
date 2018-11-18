using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

    public class RsaSigningAlgorithm : ISigningAlgorithm {
        private HashAlgorithmName _algorithmName;
        private RSASignaturePadding _padding;

        public X509Certificate2 Certificate { get; }
        public RsaAlgorithm Algorithm { get; }

        public RsaSigningAlgorithm(X509Certificate2 certificate, RsaAlgorithm algorithm) {
            Certificate = certificate;
            Algorithm = algorithm;
            Name = algorithm.ToString().ToLower();

            _algorithmName = GetAlgorithmName(algorithm);
            _padding = GetAlgorithmPadding(algorithm);
        }

        public string Name { get; }


        public byte[] CalculateSignature(byte[] data) {
            var privateKey = (RSA)Certificate.PrivateKey;
            return privateKey.SignData(data, _algorithmName, _padding);
        }

        public bool ValidateSignature(byte[] data, byte[] signature) {
            var publicKey = (RSACryptoServiceProvider)Certificate.PublicKey.Key;
            return publicKey.VerifyData(data, signature, _algorithmName, _padding);
        }

        private static HashAlgorithmName GetAlgorithmName(RsaAlgorithm algorithm) {
            switch(algorithm) {
                case RsaAlgorithm.Rs256:
                case RsaAlgorithm.Ps256:
                    return HashAlgorithmName.SHA256;
                case RsaAlgorithm.Rs384:
                case RsaAlgorithm.Ps384:
                    return HashAlgorithmName.SHA384;
                case RsaAlgorithm.Rs512:
                case RsaAlgorithm.Ps512:
                    return HashAlgorithmName.SHA512;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static RSASignaturePadding GetAlgorithmPadding(RsaAlgorithm algorithm) {
            switch(algorithm) {
                case RsaAlgorithm.Rs256:
                case RsaAlgorithm.Rs384:
                case RsaAlgorithm.Rs512:
                    return RSASignaturePadding.Pkcs1;
                case RsaAlgorithm.Ps256:
                case RsaAlgorithm.Ps384:
                case RsaAlgorithm.Ps512:
                    return RSASignaturePadding.Pkcs1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}