using System;
using System.Security.Cryptography;
using System.Numerics;

namespace EmediaWPF
{
    public class RandomNumberGenerator
    {
        private static RNGCryptoServiceProvider rngCsp;

        public RandomNumberGenerator()
        {
            rngCsp = new RNGCryptoServiceProvider();
        }

        //todo nie wiem jak to zrobic, zeby w jakimś zasiegu dawało liczbe
        public BigInteger Next(int bytesLength, BigInteger minExlusiveValue, BigInteger maxExclusiveValue)
        {
            if (minExlusiveValue > maxExclusiveValue)
                throw new Exception();

            BigInteger value;
            do
            {
                value = GetRandomNumber(bytesLength);
            }
            while (value <= minExlusiveValue || maxExclusiveValue <= value);

            return value;
        }

        public uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        public BigInteger GetRandomNumber(int bytesLength)
        {
            var randomBytes = GenerateRandomBytes(bytesLength);
            return new BigInteger(randomBytes);
        }

        public byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            rngCsp.GetBytes(buffer);
            return buffer;
        }
    }
}
