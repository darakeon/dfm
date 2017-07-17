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

        private const string emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
        
        
        
        public static User SelectByEmail(String email)
        {
            var criteria = CreateSimpleCriteria(u => u.Email == email);

            return criteria.UniqueResult<User>();
        }

        public static User ValidateAndGet(String email, String password)
        {
            var user = SelectByEmail(email);
            password = encrypt(password);

            if (user == null || user.Password != password)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return user;
        }



        public static User SaveAndSendVerify(User user, String subject, String layout)
        {
            user = saveOrUpdate(user);

            SecurityData.SendUserVerify(user, subject, layout);

            return user;
        }

        public static User Update(User user)
        {
            if (user.ID == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return saveOrUpdate(user);
        }



        internal static void Activate(User user)
        {
            user.Active = true;
            Update(user);
        }


        
        private static User saveOrUpdate(User user)
        {
            return SaveOrUpdate(user, complete, validate);
        }

        private static void validate(User user)
        {
            var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

            if (!regex.Match(user.Email).Success)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserInvalidEmail);

            var userByEmail = SelectByEmail(user.Email);

            if (userByEmail != null && userByEmail.ID != user.ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserAlreadyExists);
        }

        private static void complete(User user)
        {
            var oldUser = SelectById(user.ID);
            
            var userIsNew = oldUser == null;

            if (userIsNew)
            {
                user.Active = false;
                user.Language = "pt-BR";
                user.Creation = DateTime.Now;
                user.Password = encrypt(user.Password);
            }
            else
            {
                if (user.Active
                    && SecurityData.GetUserActivation(user) == null)
                {
                    user.Active = false;
                }
            }

        }



        internal static User ChangePassword(User user)
        {
            user.Password = encrypt(user.Password);

            return Update(user);
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
