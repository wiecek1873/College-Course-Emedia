using Extreme.Mathematics;

namespace EmediaWPF
{
    class PrimeNumbers
    {
        public static int NextPrime(int N) => IntegerMath.NextPrime(N);
        public static BigInteger NextPrime(BigInteger N) => IntegerMath.NextPrime((int)N);
        public static int PreviousPrime(int N) => IntegerMath.PreviousPrime(N);
        public static BigInteger PreviousPrime(BigInteger N) => IntegerMath.PreviousPrime((int)N);
        public static bool AreCoPrime(BigInteger a, BigInteger b) => (GreatestCommonDivisor(a, b) == 1);
        private static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b) => IntegerMath.GreatestCommonDivisor(a, b);
        private static bool IsPrime(int n) => IntegerMath.IsPrime(n);
        private static bool IsPrime(BigInteger n) => IntegerMath.IsPrime((int)n);
    }
}
