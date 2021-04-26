using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmediaWPF
{
	class PrimeNumbers
	{
        public static int NextPrime(int N)
        {
            // Base case
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
    }
}
