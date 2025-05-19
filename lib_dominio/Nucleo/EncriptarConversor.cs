using System.Security.Cryptography;
using System.Text;

namespace lib_dominio.Nucleo
{
    public class EncriptarConversor
    {
        private static string key = "t14df5898h5t7522bbce1ea7535g1686";
        private static Aes? aes = null;

        public static void Crear()
        {
            if (aes != null)
                return;
            aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];
        }

        public static string Encriptar(string value)
        {
            Crear();
            ICryptoTransform encryptor = aes!.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        writer.Write(value);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string Desencriptar(string value)
        {
            Crear();
            byte[] buffer = Convert.FromBase64String(value);
            ICryptoTransform decryptor = aes!.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        public static bool EstaEncriptado(string value)
        {
            try
            {
                Desencriptar(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
