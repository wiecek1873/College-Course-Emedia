using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Extreme.Mathematics;

namespace EmediaWPF
{
	class DataEncryption
	{
		private RandomNumberGenerator rng = new RandomNumberGenerator();

		public BigInteger p;
		public BigInteger q;
		public BigInteger n;
		public BigInteger totient; //To jest to przekreślone o
		public BigInteger e;
		public BigInteger d;

		public DataEncryption()
		{
			rng = new RandomNumberGenerator();
			PrepareKeys();
		}

		public byte[] EncryptData(byte[] chunkData)
		{
			Console.WriteLine("p: " + p);
			Console.WriteLine("q: " + q);
			Console.WriteLine("n: " + n);
			Console.WriteLine("totient: " + totient);
			Console.WriteLine("e: " + e);
			Console.WriteLine("d: " + d);
			Console.WriteLine();
			int keyLegth = n.ToByteArray().Length - 1;
			List<byte> partToEncrypt = new List<byte>();
			List<byte> encryptedData = new List<byte>();
			foreach (byte byteOfData in chunkData)
			{
				partToEncrypt.Add(byteOfData);
				if (partToEncrypt.Count == keyLegth)
				{

					var xd = Encrypt(partToEncrypt.ToArray()).ToList();
					while (xd.Count < n.ToByteArray().Length)
					{
						xd.Add(0);
					}

					//Console.WriteLine("Zaszyfrowane bajty: " + string.Join(" ", xd) + " A jako big int: " + new BigInteger(xd.ToArray()));
					encryptedData.AddRange(xd);
					partToEncrypt.Clear();
				}
			}
			if (partToEncrypt.Count > 0)
			{


				var xd = Encrypt(partToEncrypt.ToArray()).ToList();
				while (xd.Count < n.ToByteArray().Length)
				{
					xd.Add(0);
				}
				//Console.WriteLine("Zaszyfrowane bajty: " + string.Join(" ", xd) + " A jako big int: " + new BigInteger(xd.ToArray()));
				encryptedData.AddRange(xd);
				partToEncrypt.Clear();
			}
			return encryptedData.ToArray();
		}

		public byte[] DecryptData(byte[] chunkData)
		{
			int keyLegth = n.ToByteArray().Length;
			List<byte> partToDecrypt = new List<byte>();
			List<byte> decryptedData = new List<byte>();
			foreach (byte byteOfData in chunkData)
			{
				partToDecrypt.Add(byteOfData);
				if (partToDecrypt.Count == keyLegth)
				{

					//Console.WriteLine("Deszyfrowane bajty: " + string.Join(" ", partToDecrypt) + " A jako big int: " + new BigInteger(partToDecrypt.ToArray()));
					decryptedData.AddRange(Decrypt(partToDecrypt.ToArray()));
					partToDecrypt.Clear();
				}
			}
			return decryptedData.ToArray();
		}

		public byte[] Encrypt(byte[] data)
		{
			BigInteger dataAsNumber = new BigInteger(data);
			return BigInteger.ModularPow(dataAsNumber, e, n).ToByteArray();
		}

		public byte[] Decrypt(byte[] encryptedData)
		{
			BigInteger encryptedDataAsNumber = new BigInteger(encryptedData);
			return BigInteger.ModularPow(encryptedDataAsNumber, d, n).ToByteArray();
		}

		private void PrepareKeys()
		{
			p = PrimeNumbers.NextPrime(rng.Next(2, 1000, 999999999));
			q = PrimeNumbers.NextPrime(rng.Next(2, 1000, 999999999));
			if (p == q)
				p = PrimeNumbers.NextPrime(p);
			n = p * q;
			totient = (p - 1) * (q - 1);

			do
			{
				e = rng.Next(2, 1000, totient);
			} while (!PrimeNumbers.AreCoPrime(e, totient));

			d = ExtendedEuclideanAlgorithm(e, totient);
		}

		private BigInteger ExtendedEuclideanAlgorithm(BigInteger e, BigInteger totient)
		{
			BigInteger k = 0;
			BigInteger value;
			do
			{
				k += 1;
				value = (1 + k * totient) % e;
			} while (value != 0);
			Console.WriteLine("k: " + k);
			return (1 + k * totient) / e;
		}

	}
}
