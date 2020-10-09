﻿using System;
using System.Security.Cryptography;
using System.Text;
namespace HyperMarket
{
    public class EncryptionHelper
    {
        public String EncryptedHash(String input, String key, EncryptionProvider provider = EncryptionProvider.HMACSHA384, ByteSerializationType serializationType = ByteSerializationType.X2, Encoding encoding = null, Encoding keyEncoder = null)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (input == null)
                throw new ArgumentNullException("input");
            encoding = encoding ?? Encoding.UTF8;
            keyEncoder = keyEncoder ?? encoding;
            using var hashProvider = GetEncryptor(provider);
            hashProvider.Key = keyEncoder.GetBytes(key);
            var result = hashProvider.ComputeHash(encoding.GetBytes(input));
            if (serializationType == ByteSerializationType.Base64)
                return Convert.ToBase64String(result);
            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("X2"));
            return sb.ToString();
        }
        public String Hash(String input, HashProvider provider = HashProvider.SHA384, ByteSerializationType type = ByteSerializationType.X2, Encoding encoding = null)
        {
            if (input == null)
                throw new ArgumentException("input");
            encoding = encoding ?? Encoding.UTF8;
            var hasher = GetHasher(provider);
            var result = hasher.ComputeHash(encoding.GetBytes(input));
            if (type == ByteSerializationType.Base64)
                return Convert.ToBase64String(result);
            var sb = new StringBuilder(result.Length * 2);
            for (var i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("X2"));
            return sb.ToString();
        }
        public String Encryption(String input, String key, SymmetricEncryptionProvider provider = SymmetricEncryptionProvider.TrippleDes)
        {
            return Encryption(input.ToByte(), key, provider).ToBase64();
        }
        public Byte[] Encryption(Byte[] input, String key, SymmetricEncryptionProvider provider = SymmetricEncryptionProvider.TrippleDes)
        {
            var enc = GetAlgorithm(key, provider);
            var cryptoTransform = enc.CreateEncryptor();
            return cryptoTransform.TransformFinalBlock(input, 0, input.Length);
        }

        private SymmetricAlgorithm GetAlgorithm(string key, SymmetricEncryptionProvider provider)
        {
            var enc = (SymmetricAlgorithm)null;
            switch (provider)
            {
                case SymmetricEncryptionProvider.TrippleDes:
                    enc = CreateDES(key);
                    break;
                case SymmetricEncryptionProvider.Aes256:
                default:
                    enc = CreateAes256(key);
                    break;
            }

            return enc;
        }

        public Byte[] Decryption(Byte[] input, String key, SymmetricEncryptionProvider provider = SymmetricEncryptionProvider.TrippleDes)
        {
            var enc = GetAlgorithm(key, provider);
            var cryptoTransform = enc.CreateDecryptor();
            return cryptoTransform.TransformFinalBlock(input, 0, input.Length);
        }

        public String Decryption(String input, String key, SymmetricEncryptionProvider provider = SymmetricEncryptionProvider.TrippleDes)
        {
            return Decryption(Convert.FromBase64String(input), key, provider).ToText();
        }

        private static HashAlgorithm GetHasher(HashProvider provider)
        {
            switch (provider)
            {
                case HashProvider.MD5:
                    return new MD5CryptoServiceProvider();
                case HashProvider.SHA1:
                    return new SHA1CryptoServiceProvider();
                case HashProvider.SHA384:
                    return new SHA384CryptoServiceProvider();
                case HashProvider.SHA512:
                    return new SHA512CryptoServiceProvider();
                default:
                    return new SHA256CryptoServiceProvider();
            }
        }
        private static HMAC GetEncryptor(EncryptionProvider type)
        {
            switch (type)
            {
                case EncryptionProvider.HMACMD5:
                    return new HMACMD5();
                case EncryptionProvider.HMACSHA1:
                    return new HMACSHA1();
                case EncryptionProvider.HMACSHA384:
                    return new HMACSHA384();
                case EncryptionProvider.HMACSHA512:
                    return new HMACSHA512();
                default:
                    return new HMACSHA256();
            }
        }
        private SymmetricAlgorithm CreateDES(String key)
        {
            var md5 = new MD5CryptoServiceProvider();
            var des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new Byte[des.BlockSize / 8];
            return des;
        }
        private SymmetricAlgorithm CreateAes256(String key)
        {
            var md5 = new MD5CryptoServiceProvider();
            var provider = new AesCryptoServiceProvider();
            provider.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            provider.IV = new Byte[provider.BlockSize / 8];
            return provider;
        }
    }
}