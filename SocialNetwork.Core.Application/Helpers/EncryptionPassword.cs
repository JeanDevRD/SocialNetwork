using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Helpers
{
    public static class EncryptionPassword
    {
        public static string Sha256Hash(string password)
        {

            using SHA256 sha256Hash = SHA256.Create();

            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));


            StringBuilder sb = new();

            foreach (var pass in bytes)
            {
                sb.Append(pass.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
