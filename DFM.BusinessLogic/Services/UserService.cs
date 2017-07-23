using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Extensions;

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

            if (!user.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledUser);

            return user;
        }



        internal User Save(User user)
        {
            if (user.ID != 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return saveOrUpdate(user);
        }



        internal void Activate(User user)
        {
            user.Active = true;

            update(user, true);
        }


        internal User ChangePassword(User user)
        {
            user.Password = encrypt(user.Password);

            return update(user);
        }


        private User update(User user, Boolean keepActive = false)
        {
            if (user.ID == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            if (!keepActive && user.Active
                    && user.HasPendentActivation())
                user.Active = false;

            return saveOrUpdate(user);
        }




        private User saveOrUpdate(User user)
        {
            return SaveOrUpdate(user, complete, validate);
        }

        private void validate(User user)
        {
            if (String.IsNullOrEmpty(user.Password))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

            var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

            if (!regex.Match(user.Email).Success)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserEmailInvalid);

            var userByEmail = SelectByEmail(user.Email);

            if (userByEmail != null && userByEmail.ID != user.ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserAlreadyExists);
        }

        private void complete(User user)
        {
            var oldUser = SelectByEmail(user.Email);
            
            var userIsNew = oldUser == null;

            if (!userIsNew) return;

            user.Active = false;
            // TODO: update this to set the person browser language
            user.Language = "pt-BR";
            user.Creation = DateTime.Now;
            user.Password = encrypt(user.Password);
        }



        private static String encrypt(String password)
        {
            if (String.IsNullOrEmpty(password))
                return null;

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
            var currentUser = SelectByEmail(security.User.Email);

            if (currentUser == null || currentUser.Email != security.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);
        }



    }
}
