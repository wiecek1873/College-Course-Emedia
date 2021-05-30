using System;
using System.Text;
using System.Security.Cryptography;

namespace EmediaWPF
{
	class RSAcsp
	{
		//public void Run()
		//{
		//	try
		//	{
		//		UnicodeEncoding ByteConverter = new UnicodeEncoding();

		//		byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
		//		byte[] encryptedData;
		//		byte[] decryptedData;

		//		using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
		//		{

		//			//Pass the data to ENCRYPT, the public key information 
		//			//(using RSACryptoServiceProvider.ExportParameters(false),
		//			//and a boolean flag specifying no OAEP padding.
		//			encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

		//			//Pass the data to DECRYPT, the private key information 
		//			//(using RSACryptoServiceProvider.ExportParameters(true),
		//			//and a boolean flag specifying no OAEP padding.
		//			decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

		//			//Display the decrypted plaintext to the console. 
		//			Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
		//		}
		//	}
		//	catch (ArgumentNullException)
		//	{
		//		Console.WriteLine("Encryption failed.");
		//	}
		//}

	public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
	{
		try
		{
			byte[] encryptedData;
			using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
			{

				//Import the RSA Key information. This only needs
				//toinclude the public key information.
				RSA.ImportParameters(RSAKeyInfo);

				//Encrypt the passed byte array and specify OAEP padding.  
				//OAEP padding is only available on Microsoft Windows XP or
				//later.  
				encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
			}
			return encryptedData;
		}
		catch (CryptographicException e)
		{
			Console.WriteLine(e.Message);
			return null;
		}
	}

	public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
	{
		try
		{
			byte[] decryptedData;
			using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
			{
				//Import the RSA Key information. This needs
				//to include the private key information.
				RSA.ImportParameters(RSAKeyInfo);

				//Decrypt the passed byte array and specify OAEP padding.  
				//OAEP padding is only available on Microsoft Windows XP or
				//later.  
				decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
			}
			return decryptedData;
		}
		catch (CryptographicException e)
		{
			Console.WriteLine(e.ToString());

			return null;
		}
	}
}
	}
