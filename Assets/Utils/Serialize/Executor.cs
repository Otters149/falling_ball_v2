using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace utilpackages
{
    namespace serialize
    {
        public enum ActionExecute
        {
            SERIALIZE,
            DESERIALIZE
        }
        public class Executor
        {
            private static int _iterations = 2;
            private static int _keySize = 256;

            private static string _hash = "SHA1";
            private static string _salt = "aselrias38490a32"; // Random
            private static string _vector = "8947az34awl34kjq"; // Random
            private static string _private = "nikolatesla";
            static private Executor s_instance;
            private Aes _aes = null;
            private qlog.QLog _logger;

            static public Executor GetInstance()
            {
                if(s_instance == null)
                {
                    s_instance = new Executor();
                }
                return s_instance;
            }

            private Executor()
            {
                _aes = new AesCryptoServiceProvider();
                _logger = qlog.QLog.GetInstance();
            }

            public string Execute(ActionExecute action, string data)
            {

                switch(action)
                {
                    case ActionExecute.SERIALIZE:
                        string encrypted = Encrypt<AesManaged>(data);
                        _logger.LogInfo(this.GetType().FullName, encrypted);
                        return encrypted;
                    case ActionExecute.DESERIALIZE:
                        string decrypted = Decrypt<AesManaged>(data);
                        return decrypted;
                    default:
                        _logger.LogError(this.GetType().FullName, "Action Not Found");
                        return string.Empty;
                }
            }

            private string Encrypt<T>(string data) where T : SymmetricAlgorithm, new()
            {
                byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
                byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
                byte[] valueBytes = Encoding.UTF8.GetBytes(data);

                byte[] encrypted;
                using (T cipher = new T())
                {
                    PasswordDeriveBytes _passwordBytes =
                        new PasswordDeriveBytes(_private, saltBytes, _hash, _iterations);
                    byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                    cipher.Mode = CipherMode.CBC;

                    using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream to = new MemoryStream())
                        {
                            using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                            {
                                writer.Write(valueBytes, 0, valueBytes.Length);
                                writer.FlushFinalBlock();
                                encrypted = to.ToArray();
                            }
                        }
                    }
                    cipher.Clear();
                }
                return Convert.ToBase64String(encrypted);
            }

            private string Decrypt<T>(string data) where T : SymmetricAlgorithm, new()
            {
                byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
                byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
                byte[] valueBytes = Convert.FromBase64String(data);

                byte[] decrypted;
                int decryptedByteCount = 0;

                using (T cipher = new T())
                {
                    PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(_private, saltBytes, _hash, _iterations);
                    byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                    cipher.Mode = CipherMode.CBC;

                    try
                    {
                        using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                        {
                            using (MemoryStream from = new MemoryStream(valueBytes))
                            {
                                using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                                {
                                    decrypted = new byte[valueBytes.Length];
                                    decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(this.GetType().FullName, ex.Message);
                        return String.Empty;
                    }

                    cipher.Clear();
                }
                return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
            }
        }
    }
}
