using System;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace MALShowIDCrawler
{
	[Serializable]
	internal class Encrypter
	{
		private readonly string _encryptedPassword;
		private readonly string _salt1;
		private readonly string _salt2;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="password"></param>
		private Encrypter(string password)
		{
			_salt1 = GetSalt();
			_salt2 = GetSalt();
			_encryptedPassword = Encrypt(_salt1 + password + _salt2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private static string GetSalt()
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			var sb = new StringBuilder(32);
			var r = new Random();
			for (int i = 0; i < 32; i++)
			{
				sb.Append(chars[r.Next(0, chars.Length)]);
			}
			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="password"></param>
		public static void SavePassword(ZipArchive archive, string password)
		{
			var e = new Encrypter(password);
			e.SaveData(archive);
		}

		/// <summary>
		/// Decrypts a saved password.
		/// </summary>
		/// <param name="archive"></param>
		/// <returns></returns>
		public static string GetPassword(ZipArchive archive)
		{
			try
			{
				var entry = archive.GetEntry("userinfo");
				if (entry == null)
					return string.Empty;

				var formatter = new BinaryFormatter();
				var e = formatter.Deserialize(entry.Open()) as Encrypter;
				if (e == null)
					return string.Empty;

				var decrypted = Decrypt(e._encryptedPassword);
				return decrypted.Substring(e._salt1.Length, decrypted.Length - e._salt2.Length - e._salt1.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return string.Empty;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void SaveData(ZipArchive archive)
		{
			try
			{
				var formatter = new BinaryFormatter();
				var entry = archive.GetEntry("userinfo") ??
					archive.CreateEntry("userinfo");

				formatter.Serialize(entry.Open(), this);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Encrypts a given password and returns the encrypted data
		/// as a base64 string.
		/// </summary>
		private static string Encrypt(string plainText)
		{
			//encrypt data
			var data = Encoding.Unicode.GetBytes(plainText);
			byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);

			//return as base64 string
			return Convert.ToBase64String(encrypted);
		}

		/// <summary>
		/// Decrypts a given string.
		/// </summary>
		private static string Decrypt(string cipher)
		{
			//parse base64 string
			byte[] data = Convert.FromBase64String(cipher);

			//decrypt data
			byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
			return Encoding.Unicode.GetString(decrypted);
		}
	}
}
