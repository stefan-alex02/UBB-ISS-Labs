﻿using System.Security.Cryptography;
using System.Text;

namespace Business.Utils;

public class PasswordService {
    public static bool VerifyPasswordHash(string password, string hash) {
        using (SHA256 sha256Hash = SHA256.Create()) {
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }
            string encodedPassword = builder.ToString();
            
            return encodedPassword.Equals(hash);
        }
    }
}