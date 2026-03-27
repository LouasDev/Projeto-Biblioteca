using System;
using System.Security.Cryptography;

namespace BibliotecaApp.Utils
{
    public static class CriptografiaSenha
    {
        public static void CriarHash(string senha, out string hash, out string salt)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);

                using (var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, 10000))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(32);
                    hash = Convert.ToBase64String(hashBytes);
                }
            }
        }

        public static bool VerificarSenha(string senhaDigitada, string hashSalvo, string saltSalvo)
        {
            byte[] saltBytes = Convert.FromBase64String(saltSalvo);
            using (var pbkdf2 = new Rfc2898DeriveBytes(senhaDigitada, saltBytes, 10000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string hashCalculado = Convert.ToBase64String(hashBytes);
                return hashCalculado == hashSalvo;
            }
        }
    }
}
    