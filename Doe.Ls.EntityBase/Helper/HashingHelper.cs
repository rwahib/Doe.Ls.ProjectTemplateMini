using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.EntityBase.Helper
    {
    public static class HashingHelper
        {
        public static string HashPasswordOneWay(string pasword)
            {
            return HashString(pasword);
            //SHA256 hash = new SHA256CryptoServiceProvider();
            //byte[] arrbyte = hash.ComputeHash(Encoding.UTF8.GetBytes(pasword));
            //return Convert.ToBase64String(arrbyte);
            }
        private static string HashString(string stringToHash)
            {
            SHA256 hash = new SHA256CryptoServiceProvider();
            byte[] arrbyte = hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            return Convert.ToBase64String(arrbyte);
            }
        }
    }
