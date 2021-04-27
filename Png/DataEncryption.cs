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
		BigInteger c;

		public DataEncryption()
		{
			rng = new RandomNumberGenerator();
		}

		public void Run()
		{
			PrepareKeys();

		}

		private void PrepareKeys()
		{
			p = 1123;
			q = 1237;
			//p = PrimeNumbers.NextPrime(rng.Next(2, 3, 999999999));
			//q = PrimeNumbers.NextPrime(rng.Next(2, 3, 999999999));
			n = p * q;
			totient = (p - 1) * (q - 1);

			do
			{
				e = rng.Next(2, 3, totient);
			} while (!PrimeNumbers.AreCoPrime(e, totient));

			e = 834781;

			d = ExtendedEuclideanAlgorithm(e, totient);
		}

		private byte[] Encrypt(byte[] data, BigInteger e, BigInteger n)
		{
			BigInteger dataAsNumber = new BigInteger(data);
			return BigInteger.ModPow(dataAsNumber, d, n).ToByteArray();
		}

		private byte[] Decrypt(byte[] encryptedData, BigInteger d, BigInteger n)
		{
			BigInteger encryptedDataAsNumber = new BigInteger(encryptedData);
			return BigInteger.ModPow(encryptedDataAsNumber, d, n).ToByteArray();
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
