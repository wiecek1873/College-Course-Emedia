using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace EmediaWPF
{
	class DataEncryption
	{
		private RandomNumberGenerator rng = new RandomNumberGenerator();

		BigInteger p;
		BigInteger q;
		BigInteger n;
		BigInteger totient; //To jest to przekreślone o
		BigInteger e;
		BigInteger d;

		public DataEncryption()
		{
			rng = new RandomNumberGenerator();
			PrepareKeys();
		}

		//public byte[] EncryptData(byte[] chunkData)
		//{
		//	int keyLegth = n.ToByteArray().Length;
		//	int iterator = 0;
		//	List<byte> partToEncrypt = new List<byte>();
		//	List<byte> encryptedData = new List<byte>();
		//	foreach (byte byteOfData in chunkData)
		//	{
		//		partToEncrypt.Add(byteOfData);
		//		iterator++;
		//		if (iterator == keyLegth)
		//		{

		//			encryptedData.AddRange(Decrypt(partToEncrypt.ToArray()));
		//			partToEncrypt.Clear();
		//			iterator = 0;
		//		}
		//	}
		//	if (partToEncrypt.Count > 0)
		//	{
		//		encryptedData.AddRange(Encrypt(partToEncrypt.ToArray()));
		//		partToEncrypt.Clear();
		//	}
		//	return encryptedData.ToArray();
		//}

		//public byte[] DecryptData(byte[] chunkData)
		//{
		//	int keyLegth = n.ToByteArray().Length;
		//	int iterator = 0;
		//	List<byte> partToDecrypt = new List<byte>();
		//	List<byte> decryptedData = new List<byte>();
		//	foreach (byte byteOfData in chunkData)
		//	{
		//		partToDecrypt.Add(byteOfData);
		//		iterator++;
		//		if (iterator == keyLegth)
		//		{
		//			decryptedData.AddRange(Decrypt(partToDecrypt.ToArray()));
		//			partToDecrypt.Clear();
		//			iterator = 0;
		//		}
		//	}
		//	if (partToDecrypt.Count > 0)
		//	{
		//		decryptedData.AddRange(Decrypt(partToDecrypt.ToArray()));
		//		partToDecrypt.Clear();
		//	}
		//	return decryptedData.ToArray();
		//}

		private void PrepareKeys()
		{
			p = PrimeNumbers.NextPrime(rng.Next(2, 1000, 999999999));
			q = PrimeNumbers.NextPrime(rng.Next(2, 1000, 999999999));
			n = p * q;
			totient = (p - 1) * (q - 1);

			do
			{
				e = rng.Next(2, 1000, totient);
			} while (!PrimeNumbers.AreCoPrime(e, totient));

			e = 834781;

			d = ExtendedEuclideanAlgorithm(e, totient);
		}

		public byte[] Encrypt(byte[] data)
		{
			BigInteger dataAsNumber = new BigInteger(data);
			var encryptedData = BigInteger.ModPow(dataAsNumber, e, n).ToByteArray().ToList();
			return encryptedData.ToArray();
		}

		public byte[] Decrypt(byte[] encryptedData)
		{
			BigInteger encryptedDataAsNumber = new BigInteger(encryptedData);
			var decryptedData = BigInteger.ModPow(encryptedDataAsNumber, d, n).ToByteArray().ToList();
			return decryptedData.ToArray();
		}

		private BigInteger ExtendedEuclideanAlgorithm(BigInteger e, BigInteger totient)
		{
			BigInteger k = 0;
			BigInteger value;
			do
			{
				k++;
				value = (1 + k * totient) % e;
			} while (value != 0);
			return (1 + k * totient) / e;
		}

	}
}
