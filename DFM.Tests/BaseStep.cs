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
        protected ServiceAccess SA;

        protected BaseStep()
        {
            NHManager.Start();

            SA = new ServiceAccess(new Connector());

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
                return SA.Safe.ValidateAndGet(userEmail, userPassword);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidUser)
                    throw;

                SA.Safe.SaveUserAndSendVerify(userEmail, userPassword);
                return SA.Safe.ValidateAndGet(userEmail, userPassword);
            }
        }

        protected Account GetOrCreateAccount(String accountName)
        {
            try
            {
                return SA.Admin.SelectAccountByName(accountName, User);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidAccount)
                    throw;

                SA.Admin.SaveOrUpdateAccount(new Account { Name = accountName, User = User });
                return SA.Admin.SelectAccountByName(accountName, User);
            }
        }

        protected Category GetOrCreateCategory(String categoryName)
        {
            try
            {
                return SA.Admin.SelectCategoryByName(categoryName, User);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidCategory)
                    throw;

                SA.Admin.SaveOrUpdateCategory(new Category { Name = categoryName, User = User });
                return SA.Admin.SelectCategoryByName(categoryName, User);
            }
        }
        #endregion



        protected static DFMCoreException Error
        {
            get { return Get<DFMCoreException>("error"); }
            set { Set("error", value); }
        }

        protected static User User
        {
            get { return Get<User>("user"); }
            set { Set("user", value); }
        }

        protected static Account Account
        {
            get { return Get<Account>("Account"); }
            set { Set("Account", value); }
        }

        protected static Category Category
        {
            get { return Get<Category>("Category"); }
            set { Set("Category", value); }
        }


        protected static Account AccountOut
        {
            get { return Get<Account>("AccountOut"); }
            set { Set("AccountOut", value); }
        }

        protected static Double AccountOutTotal
        {
            get { return Get<Double>("AccountOutTotal"); }
            set { Set("AccountOutTotal", value); }
        }

        protected static Double YearCategoryAccountOutTotal
        {
            get { return Get<Double>("YearCategoryAccountOutTotal"); }
            set { Set("YearCategoryAccountOutTotal", value); }
        }

        protected static Double MonthCategoryAccountOutTotal
        {
            get { return Get<Double>("MonthCategoryAccountOutTotal"); }
            set { Set("MonthCategoryAccountOutTotal", value); }
        }


        protected static Account AccountIn
        {
            get { return Get<Account>("AccountIn"); }
            set { Set("AccountIn", value); }
        }

        protected static Double AccountInTotal
        {
            get { return Get<Double>("AccountInTotal"); }
            set { Set("AccountInTotal", value); }
        }

        protected static Double YearCategoryAccountInTotal
        {
            get { return Get<Double>("YearCategoryAccountInTotal"); }
            set { Set("YearCategoryAccountInTotal", value); }
        }

        protected static Double MonthCategoryAccountInTotal
        {
            get { return Get<Double>("MonthCategoryAccountInTotal"); }
            set { Set("MonthCategoryAccountInTotal", value); }
        }

        
        protected static Category MoveCategory
        {
            get { return Get<Category>("MoveCategory"); }
            set { Set("MoveCategory", value); }
        }

        
        protected static BaseMove Move
        {
            get { return Get<BaseMove>("BaseMove"); }
            set { Set("BaseMove", value); }
        }

        


        protected const String UserEmail = "test@dontflymoney.com";
        protected static String UserPassword = "password";
        protected const String AccountName = "first account";
        protected const String CategoryName = "first category";

        protected const String AccountOutName = "account out";
        protected const String AccountInName = "account in";

    }
}
