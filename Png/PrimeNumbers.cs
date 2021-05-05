using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extreme.Mathematics;

namespace EmediaWPF
{
	class PrimeNumbers
	{
		public static int NextPrime(int N)
		{
			if (N <= 1)
				return 2;

			int prime = N;
			bool found = false;

			while (!found)
			{
				prime++;

				if (IsPrime(prime))
					found = true;
			}
			return prime;
		}

		public static BigInteger NextPrime(BigInteger N)
		{
			if (N <= 1)
				return 2;

			BigInteger prime = N;
			bool found = false;

			while (!found)
			{
				prime += 1;

				if (IsPrime(prime))
					found = true;
			}
			return prime;
		}

		public static int PreviousPrime(int N)
		{
			if (N <= 1)
				return 1;

			int prime = N;
			bool found = false;

			while (!found)
			{
				prime--;

				if (IsPrime(prime))
					found = true;
			}
			return prime;
		}

		public static BigInteger PreviousPrime(BigInteger N)
		{
			if (N <= 1)
				return 1;

			BigInteger prime = N;
			bool found = false;

			while (!found)
			{
                prime += 1;

				if (IsPrime(prime))
					found = true;
			}
			return prime;
		}

		public static bool AreCoPrime(BigInteger a, BigInteger b)
		{

			if (GreatestCommonDivisor(a, b) == 1)
				return true;
			else
				return false;
		}

		private static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
		{
			while (a > b || b > a)
			{
				if (a > b)
					a = a - b;
				else
					b = b - a;
			}

			if (a == 1 && b == 1)
				return 1;
			if (a == 0 || b == 0 || a == b)
				return 0;

			return -1;
		}

		private static bool IsPrime(int n)
		{
			// Corner cases
			if (n <= 1) return false;
			if (n <= 3) return true;

			// This is checked so that we can skip
			// middle five numbers in below loop
			if (n % 2 == 0 || n % 3 == 0)
				return false;

			for (int i = 5; i * i <= n; i = i + 6)
				if (n % i == 0 ||
					n % (i + 2) == 0)
					return false;

			return true;
		}

		private static bool IsPrime(BigInteger n)
		{
			// Corner cases
			if (n <= 1) return false;
			if (n <= 3) return true;

			// This is checked so that we can skip
			// middle five numbers in below loop
			if (n % 2 == 0 || n % 3 == 0)
				return false;

			for (int i = 5; i * i <= n; i = i + 6)
				if (n % i == 0 ||
					n % (i + 2) == 0)
					return false;

			return true;
		}
	}
}
