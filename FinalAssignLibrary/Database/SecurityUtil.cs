using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Database
{
    /// <summary>
    /// Class which is used in the processing of Account registrations or log in
    /// requests. Stores in the Database folder as it is used in conjunction with
    /// the AccountDAL class.
    /// </summary>
    public class SecurityUtil
    {
        /// <summary>
        /// Used to generate a salt and hash the password of a newly set member
        /// password.
        /// </summary>
        /// <param name="password">String containing the provided user password</param>
        /// <returns>String array containing the salt[0] and the hashed password[1]</returns>
        public static string[] HashNewPass(string password)
        {
            string[] toRet = new string[2];
            RNGCryptoServiceProvider rNG = new RNGCryptoServiceProvider();

            // getting salt in bytes
            byte[] bSalt = new byte[32];
            rNG.GetBytes(bSalt);

            // converting password to bytes
            byte[] bPass = Encoding.UTF8.GetBytes(password);

            // combining salt & password
            byte[] bCombined = new byte[bSalt.Length + bPass.Length];
            bSalt.CopyTo(bCombined, 0);
            bPass.CopyTo(bCombined, bSalt.Length);

            // hashing service
            SHA512 sHA512 = SHA512.Create();
            byte[] bHash = sHA512.ComputeHash(bCombined);

            // preparing output
            string hash = Convert.ToBase64String(bHash);
            string salt = Convert.ToBase64String(bSalt);

            // using a String[] due to having to return both the salt & the password
            // hash back to the Member to be stored in the database
            toRet[0] = salt;
            toRet[1] = hash;

            return toRet;
        }

        /// <summary>
        /// Used to get the hashed version of a password string. Used when validating
        /// whether a log in request matches the credentials stored inside the database.
        /// </summary>
        /// <param name="password">String containing the password from log in form.</param>
        /// <param name="salt">String containing the user's password salt from the database.</param>
        /// <returns></returns>
        public static string HashPass(string password, string salt)
        {
            // converting password & salt to bytes
            // uses UTF8 encoding because value is from webform input
            byte[] bPass = Encoding.UTF8.GetBytes(password);
            // uses Convert from String method because value is from database
            byte[] bSalt = Convert.FromBase64String(salt);

            // combining salt & password
            byte[] bCombined = new byte[bSalt.Length + bPass.Length];
            bSalt.CopyTo(bCombined, 0);
            bPass.CopyTo(bCombined, bSalt.Length);

            // hashing service
            SHA512 sHA512 = SHA512.Create();
            byte[] bHash = sHA512.ComputeHash(bCombined);

            // preparing output
            string hash = Convert.ToBase64String(bHash);
            return hash;
        }

        /// <summary>
        /// Verifies if password entered into log in form matches the stored
        /// password in the database. Method converts the formPass into a hashed
        /// password and then appends it to the users salt value. Once completed,
        /// compares the two hashed values to see if they are a match.
        /// </summary>
        /// <param name="dbSalt">String containing the salt value from the database</param>
        /// <param name="dbPass">String containing the password hash from database</param>
        /// <param name="formPass">String containing the password from the log in form</param>
        /// <returns>True or false depending on if the passwords match.</returns>
        public static bool AuthenticatePassword(string dbSalt, string dbPass, string formPass)
        {
            string formHash = HashPass(formPass, dbSalt);

            if (dbPass == formHash)
                return true;
            else
                return false;
        }
    }
}
