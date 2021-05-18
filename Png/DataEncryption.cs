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

        public int KeyLength {get => 1234;} // todo 

        public DataEncryption()
        {
            rng = new RandomNumberGenerator();
            PrepareKeys();
        }

        public DataEncryption(EncryptionSave keys)
        {
            rng = new RandomNumberGenerator();
            d = keys.d;
            e = keys.e;
            n = keys.n;
        }

        public EncryptionSave GetKeys()
        {
            return new EncryptionSave
            {
                d = d,
                e = e,
                n = n
            };
        }

        public byte[] EncryptData(byte[] chunkData)
        {
            int keyLength = n.ToByteArray().Length - 1;
            List<byte> partToEncrypt = new List<byte>();
            List<byte> encryptedData = new List<byte>();

            foreach (byte byteOfData in chunkData)
            {
                partToEncrypt.Add(byteOfData);
                if (partToEncrypt.Count == keyLength)
                {
					List<byte> partOfEncryptedData = Encrypt(partToEncrypt.ToArray()).ToList();
					while (partOfEncryptedData.Count < n.ToByteArray().Length)
					{
                        if (new BigInteger(partOfEncryptedData.ToArray()).Sign == -1) //Jeśli ujemna
                            partOfEncryptedData.Add(255);
                        else
                            partOfEncryptedData.Add(0);
					}

					encryptedData.AddRange(partOfEncryptedData);
                    partToEncrypt.Clear();
                }
            }

            if (partToEncrypt.Count > 0)
            {
                List<byte> partOfEncryptedData = Encrypt(partToEncrypt.ToArray()).ToList();
                while (partOfEncryptedData.Count < n.ToByteArray().Length)
                {
                    if (new BigInteger(partOfEncryptedData.ToArray()).Sign == -1) //Jeśli ujemna
                        partOfEncryptedData.Add(255);
                    else
                        partOfEncryptedData.Add(0);
                }

                encryptedData.AddRange(partOfEncryptedData);
                partToEncrypt.Clear();
            }

            return encryptedData.ToArray();
        }

        public byte[] DecryptData(byte[] chunkData)
        {
            int keyLength = n.ToByteArray().Length;
            List<byte> partToDecrypt = new List<byte>();
            List<byte> decryptedData = new List<byte>();

            foreach (byte byteOfData in chunkData)
            {
                partToDecrypt.Add(byteOfData);
                if (partToDecrypt.Count == keyLength)
                {
                    decryptedData.AddRange(Decrypt(partToDecrypt.ToArray()));
                    partToDecrypt.Clear();
                }
            }

            return decryptedData.ToArray();
        }

        public byte[] Encrypt(byte[] data)
        {
            BigInteger dataAsNumber = new BigInteger(data);
            return BigInteger.ModPow(dataAsNumber, e, n).ToByteArray();
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            BigInteger encryptedDataAsNumber = new BigInteger(encryptedData);
            return BigInteger.ModPow(encryptedDataAsNumber, d, n).ToByteArray();
        }

        private async void PrepareKeys()
        {
            p = await PrimeNumbers.NextPrimeAsync(rng.Next(2, 1000, 999999999));
            q = await PrimeNumbers.NextPrimeAsync(rng.Next(2, 1000, 999999999));

            if (p == q)
                p = await PrimeNumbers .NextPrimeAsync(p);

            n = p * q;
            totient = (p - 1) * (q - 1);

            do
            {
                e = rng.Next(2, 1000, totient);
            }
            while (!PrimeNumbers.AreCoPrime(e, totient));

            d = ExtendedEuclideanAlgorithm(e, totient);
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
