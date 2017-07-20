using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class UserService : BaseService<User>
    {
        internal UserService(IRepository<User> repository) : base(repository) { }

        private const string emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
        
        
        
        internal User SelectByEmail(String email)
        {
            return SingleOrDefault(u => u.Email == email);
        }

        internal User ValidateAndGet(String email, String password)
        {
            var user = SelectByEmail(email);
            password = encrypt(password);

            if (user == null || user.Password != password)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return user;
        }




        internal User Update(User user)
        {
            if (user.ID == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return SaveOrUpdate(user);
        }



        internal void Activate(User user)
        {
            user.Active = true;
            Update(user);
        }


        internal User SaveOrUpdate(User user)
        {
            return SaveOrUpdate(user, complete, validate);
        }

        private void validate(User user)
        {
            var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

            if (!regex.Match(user.Email).Success)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserInvalidEmail);

            var userByEmail = SelectByEmail(user.Email);

            if (userByEmail != null && userByEmail.ID != user.ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserAlreadyExists);
        }

        private void complete(User user)
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
                    && user.HasPendentActivation())
                {
                    user.Active = false;
                }
            }

        }



        internal User ChangePassword(User user)
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



        internal void ValidateSecurity(Security security)
        {
            var currentUser = SelectById(security.User.ID);

            if (currentUser == null || currentUser.Email != security.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);
        }

    }
}
