using System.Numerics;
using System.Threading.Tasks;
using BigPrimeNumber;
using BigPrimeNumber.Primality.Heuristic;

namespace EmediaWPF
{
    class PrimeNumbers
    {
        public static async Task<int> NextPrimeAsync(int N)
        {
            if (N <= 1)
                return 2;

            int prime = N;
            bool found = false;

            while (!found)
            {
                prime++;

                if (await IsPrimeAsync(prime))
                    found = true;
            }
            return prime;
        }

        public static async Task<BigInteger> NextPrimeAsync(BigInteger N)
        {
            if (N <= 1)
                return 2;

            BigInteger prime = N;
            bool found = false;

            while (!found)
            {
                prime++;

                if (await IsPrimeAsync(prime))
                    found = true;
            }
            return prime;
        }

        public static async Task<int> PreviousPrimeAsync(int N)
        {
            if (N <= 1)
                return 1;

            int prime = N;
            bool found = false;

            while (!found)
            {
                prime--;

                if (await IsPrimeAsync(prime))
                    found = true;
            }
            return prime;
        }

        public static async Task<BigInteger> PreviousPrimeAsync(BigInteger N)
        {
            if (N <= 1)
                return 1;

            BigInteger prime = N;
            bool found = false;

            while (!found)
            {
                prime--;

                if (await IsPrimeAsync(prime))
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

        //todo to może wywalić stacka jeśli liczby będą od siebie bardzo różne
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
            //// Everything divides 0
            //if (a == 0 || b == 0)
            //    return 0;

            //// base case
            //if (a == b)
            //    return 0;

            //// a is greater
            //if (a > b)
            //    return GreatestCommonDivisor(a - b, b);

            //return GreatestCommonDivisor(a, b - a);
        }

        private static Task<bool> IsPrimeAsync(int n)
        {
            BigInteger big = n;
            return big.IsPrime(new FermatTest(1234));
        }

        private static Task<bool> IsPrimeAsync(BigInteger n)
        {
            return n.IsPrime(new FermatTest(1234));
        }
    }
}
