using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime;
using ThirdParty.MD5;

namespace Amazon.Util
{
	public static class CryptoUtilFactory
	{
		private class CryptoUtil : ICryptoUtil
		{
			[ThreadStatic]
			private static HashAlgorithm _hashAlgorithm;

			private static HashAlgorithm SHA256HashAlgorithmInstance
			{
				get
				{
					if (_hashAlgorithm == null)
					{
						try
						{
							_hashAlgorithm = HashAlgorithm.Create("SHA-256");
						}
						catch (Exception)
						{
							throw;
						}
					}
					return _hashAlgorithm;
				}
			}

			internal CryptoUtil()
			{
			}

			public string HMACSign(string data, string key, SigningAlgorithm algorithmName)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				return HMACSign(bytes, key, algorithmName);
			}

			public string HMACSign(byte[] data, string key, SigningAlgorithm algorithmName)
			{
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("key", "Please specify a Secret Signing Key.");
				}
				if (data == null || data.Length == 0)
				{
					throw new ArgumentNullException("data", "Please specify data to sign.");
				}
				KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(algorithmName.ToString().ToUpper(CultureInfo.InvariantCulture));
				if (keyedHashAlgorithm == null)
				{
					throw new InvalidOperationException("Please specify a KeyedHashAlgorithm to use.");
				}
				try
				{
					keyedHashAlgorithm.Key = Encoding.UTF8.GetBytes(key);
					byte[] inArray = keyedHashAlgorithm.ComputeHash(data);
					return Convert.ToBase64String(inArray);
				}
				finally
				{
					keyedHashAlgorithm.Clear();
				}
			}

			public byte[] ComputeSHA256Hash(byte[] data)
			{
				return SHA256HashAlgorithmInstance.ComputeHash(data);
			}

			public byte[] ComputeSHA256Hash(Stream steam)
			{
				return SHA256HashAlgorithmInstance.ComputeHash(steam);
			}

			public byte[] ComputeMD5Hash(byte[] data)
			{
				return new MD5Managed().ComputeHash(data);
			}

			public byte[] ComputeMD5Hash(Stream steam)
			{
				return new MD5Managed().ComputeHash(steam);
			}

			public byte[] HMACSignBinary(byte[] data, byte[] key, SigningAlgorithm algorithmName)
			{
				if (key == null || key.Length == 0)
				{
					throw new ArgumentNullException("key", "Please specify a Secret Signing Key.");
				}
				if (data == null || data.Length == 0)
				{
					throw new ArgumentNullException("data", "Please specify data to sign.");
				}
				KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(algorithmName.ToString().ToUpper(CultureInfo.InvariantCulture));
				if (keyedHashAlgorithm == null)
				{
					throw new InvalidOperationException("Please specify a KeyedHashAlgorithm to use.");
				}
				try
				{
					keyedHashAlgorithm.Key = key;
					return keyedHashAlgorithm.ComputeHash(data);
				}
				finally
				{
					keyedHashAlgorithm.Clear();
				}
			}
		}

		private static CryptoUtil util = new CryptoUtil();

		public static ICryptoUtil CryptoInstance
		{
			get
			{
				return util;
			}
		}
	}
}
