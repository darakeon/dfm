using System;
using System.IO;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Multilanguage;
using DFM.Repositories;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    public abstract class BaseStep
    {
        protected ServiceAccess Access;

        protected BaseStep()
        {
            NHManager.Start();

            Access = new ServiceAccess(new Connector());

            var path = Directory.GetCurrentDirectory();
            PlainText.Initialize(path);
        }


        public void Dispose()
        {
            NHManager.End();
        }


        protected static T Get<T>(String key)
        {
            return ScenarioContext.Current.ContainsKey(key)
                ? (T)ScenarioContext.Current[key]
                : default(T);
        }

        protected static void Set(String key, object value)
        {
            ScenarioContext.Current[key] = value;
        }



        protected Int32? GetInt(String str)
        {
            return String.IsNullOrEmpty(str)
                ? (Int32?) null
                : Int32.Parse(str);
        }



        #region Get or Create
        protected User GetOrCreateUser(String userEmail, String userPassword)
        {
            try
            {
                return Access.Safe.ValidateAndGet(userEmail, userPassword);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidUser)
                    throw;

                Access.Safe.SaveUserAndSendVerify(userEmail, userPassword);
                return Access.Safe.ValidateAndGet(userEmail, userPassword);
            }
        }

        protected Account GetOrCreateAccount(String accountName)
        {
            try
            {
                return Access.Admin.SelectAccountByName(accountName, User);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidAccount)
                    throw;

                Access.Admin.SaveOrUpdateAccount(new Account { Name = accountName, User = User });
                return Access.Admin.SelectAccountByName(accountName, User);
            }
        }

        protected Category GetOrCreateCategory(String categoryName)
        {
            try
            {
                return Access.Admin.SelectCategoryByName(categoryName, User);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidCategory)
                    throw;

                Access.Admin.SaveOrUpdateCategory(new Category { Name = categoryName, User = User });
                return Access.Admin.SelectCategoryByName(categoryName, User);
            }
        }
        #endregion



        protected DFMCoreException Error
        {
            get { return Get<DFMCoreException>("error"); }
            set { Set("error", value); }
        }

        protected User User
        {
            get { return Get<User>("user"); }
            set { Set("user", value); }
        }

        protected Account Account
        {
            get { return Get<Account>("Account"); }
            set { Set("Account", value); }
        }

        protected Category Category
        {
            get { return Get<Category>("Category"); }
            set { Set("Category", value); }
        }

        protected static Account AccountOut
        {
            get { return Get<Account>("AccountOut"); }
            set { Set("AccountOut", value); }
        }

        protected static Account AccountIn
        {
            get { return Get<Account>("AccountIn"); }
            set { Set("AccountIn", value); }
        }

        protected static BaseMove Move
        {
            get { return Get<BaseMove>("BaseMove"); }
            set { Set("BaseMove", value); }
        }

        


        protected const String CentralUserEmail = "test@dontflymoney.com";
        protected static String CentralUserPassword = "password";
        protected const String CentralAccountName = "first account";
        protected const String CentralCategoryName = "first category";

        protected const String AccountOutName = "account out";
        protected const String AccountInName = "account in";

    }
}
