using Extreme.Mathematics;

namespace EmediaWPF
{
    class PrimeNumbers
    {
        public static int NextPrime(int N) => PrimeNumbers.NextPrime(N);
        public static BigInteger NextPrime(BigInteger N) => PrimeNumbers.NextPrime(N);
        public static int PreviousPrime(int N) => PrimeNumbers.PreviousPrime(N);
        public static BigInteger PreviousPrime(BigInteger N) => PrimeNumbers.PreviousPrime(N);
        public static bool AreCoPrime(BigInteger a, BigInteger b) => (GreatestCommonDivisor(a, b) == 1);
        private static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b) => PrimeNumbers.GreatestCommonDivisor(a, b);
        private static bool IsPrime(int n) => PrimeNumbers.IsPrime(n);
        private static bool IsPrime(BigInteger n) => PrimeNumbers.IsPrime(n);
    }
}
