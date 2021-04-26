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

		public DataEncryption()
		{
			rng = new RandomNumberGenerator();
		}

		public void Run()
		{
			Console.WriteLine(PrimeNumbers.NextPrime(2));
			Console.WriteLine(PrimeNumbers.NextPrime(3));
			Console.WriteLine(PrimeNumbers.NextPrime(43243));
		}


		BigInteger p;
		BigInteger q;
		BigInteger n;
		BigInteger totient; //To jest to przekreślone o
		BigInteger e;
		BigInteger d;

		private void PrepareKeys()
		{
			p = PrimeNumbers.NextPrime(rng.Next(1000, 10000));
			q = PrimeNumbers.NextPrime(rng.Next(1000, 10000));
			n = p * q;
			totient = (p - 1) * (q - 1);
			//e = rng.Next(2, totient);

		}



	}
}
