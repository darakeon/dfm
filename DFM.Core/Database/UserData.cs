using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Exceptions;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
		private UserData() { }

        public static User SelectByLogin(String email)
        {
            var criteria = CreateSimpleCriteria(u => u.Email == email);

            return criteria.UniqueResult<User>();
        }

        public static User ValidateAndGet(String email, String password)
        {
            var user = SelectByLogin(email);
            password = encrypt(password);

            if (user == null || user.Password != password)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return user;
        }



        public static User SaveOrUpdate(User user)
        {
            return SaveOrUpdate(user, validate, complete);
        }

        private static void validate(User user)
        {
            var regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

            if (!regex.Match(user.Email).Success)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserInvalidEmail);

            if (SelectByLogin(user.Email) != null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserAlreadyExists);
        }

        private static void complete(User user)
        {
            user.Language = "pt-BR";

            user.Password = encrypt(user.Password);
        }



        private static String encrypt(String password)
        {
            var md5 = new MD5CryptoServiceProvider();

            var originalBytes = Encoding.Default.GetBytes(password);
            var encodedBytes = md5.ComputeHash(originalBytes);

            var hexCode = BitConverter
                            .ToString(encodedBytes)
                            .Replace("-", "");

            return hexCode;
        }

    }
}
