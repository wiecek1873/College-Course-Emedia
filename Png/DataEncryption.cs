using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EmediaWPF
{
    class DataEncryption : Singleton<DataEncryption>
    {
        private RandomNumberGenerator rng = new RandomNumberGenerator();

        public BigInteger p;
        public BigInteger q;
        public BigInteger n;
        public BigInteger totient; //To jest to przekreślone o
        public BigInteger e;
        public BigInteger d;

        public int KeyLength {get => n.ToByteArray().Length;} // todo

        public DataEncryption()
        {
            rng = new RandomNumberGenerator();
        }

        public DataEncryption(EncryptionSave keys)
        {
            rng = new RandomNumberGenerator();
            d = new BigInteger(keys.d,true);
            e = new BigInteger(keys.e,true);
            n = new BigInteger(keys.n,true);
        }

		public EncryptionSave GetKeys()
		{
			return new EncryptionSave
			{
				d = d.ToByteArray(true),
				e = e.ToByteArray(true),
				n = n.ToByteArray(true)
			};
		}

        public void PrepareKeys(int bytesLength)
        {
            byte[] numberBytes = new byte[bytesLength/2];

            for(int i = 0; i < numberBytes.Length; i++)
			{
                numberBytes[i] = byte.MaxValue;
			}

            numberBytes[(bytesLength / 2) - 1] = 1;

            BigInteger maxValue = new BigInteger(numberBytes, true);

            numberBytes[(bytesLength / 2) - 1] = 0;

            BigInteger minValue = new BigInteger(numberBytes, true);

            BigInteger randomNum = rng.Next(bytesLength / 2, minValue, maxValue);
            var Tp = PrimeNumbers.NextPrimeAsync(randomNum);
            Tp.Wait();
            p = Tp.Result;

            Console.WriteLine("Jest p");

			randomNum = rng.Next(bytesLength / 2, minValue, maxValue);
            var Tq = PrimeNumbers.NextPrimeAsync(randomNum);
            Tq.Wait();
            q = Tq.Result;

            Console.WriteLine("Jest q");

            if (p == q)
            {
                var Tpp = PrimeNumbers.NextPrimeAsync(p);
                Tpp.Wait();
                p = Tpp.Result;
            }

            n = p * q;
            totient = (p - 1) * (q - 1);

            Console.WriteLine("Jeszcze EEA i koniec");

            do
            {
                e = rng.Next(bytesLength / 2, minValue,totient);
            }
            while (!PrimeNumbers.AreCoPrime(e, totient));

            d = ExtendedEuclideanAlgorithm(e, totient);
        }

        public byte[] EncryptData(byte[] chunkData)
        {
            int dataLength = KeyLength - 1;
            List<byte> partToEncrypt = new List<byte>();
            List<byte> encryptedData = new List<byte>();

            foreach (byte byteOfData in chunkData)
            {
                partToEncrypt.Add(byteOfData);
                if (partToEncrypt.Count == dataLength)
                {
					byte[] encrypted = Encrypt(partToEncrypt.ToArray());
                    Array.Resize<byte>(ref encrypted, KeyLength);
					encryptedData.AddRange(encrypted);
                    partToEncrypt.Clear();
                }
            }

            if (partToEncrypt.Count > 0)
            {
                byte[] encrypted = Encrypt(partToEncrypt.ToArray());
                Array.Resize<byte>(ref encrypted, KeyLength);
                encryptedData.AddRange(encrypted);
                partToEncrypt.Clear();
            }

            return encryptedData.ToArray();
        }

        public byte[] DecryptData(byte[] chunkData)
        {
            int dataLength = KeyLength - 1;
            List<byte> partToDecrypt = new List<byte>();
            List<byte> decryptedData = new List<byte>();

            foreach (byte data in chunkData)
            {
                partToDecrypt.Add(data);
                if (partToDecrypt.Count == KeyLength)
                {
                    byte[] decrypted = Decrypt(partToDecrypt.ToArray());
                    Array.Resize<byte>(ref decrypted, dataLength);
                    decryptedData.AddRange(decrypted);
                    partToDecrypt.Clear();
                }
            }

            return decryptedData.ToArray();
        }

        public byte[] Encrypt(byte[] data)
        {
            return BigInteger.ModPow(new BigInteger(data, true), e, n).ToByteArray(true);
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            return BigInteger.ModPow(new BigInteger(encryptedData, true), d, n).ToByteArray(true);
        }



        private BigInteger ExtendedEuclideanAlgorithm(BigInteger e, BigInteger totient)
        {
            BigInteger k = 0;
            BigInteger value;
            do
            {
                k++;
                value = (1 + k * totient) % e;
            }
            while (value != 0);

            return (1 + k * totient) / e;
        }
    }
}
