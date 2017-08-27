using System;
using System.Collections.Generic;
using System.IO;
using Ak.MVC.Cookies;
using Ak.NHibernate;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Multilanguage;
using DFM.Repositories.Mappings;
using DFM.Tests.BusinessLogic.Helpers;
using DFM.Tests.Helpers;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic
{
    public abstract class BaseStep : ContextHelper
    {
        protected static ServiceAccess SA;
        protected static Current Current { get { return SA.Current; } }

        protected BaseStep()
        {
            if (SA != null)
                return;

            NHManager.Start<UserMap, User>();

            SA = new ServiceAccess();

            var path = Directory.GetCurrentDirectory();
            PlainText.Initialize(path);
        }


        public void Dispose()
        {
            NHManager.End();
        }





        protected Int32? GetInt(String str)
        {
            return String.IsNullOrEmpty(str)
                ? (Int32?) null
                : Int32.Parse(str);
        }



        protected static String MakeUrlFromName(String name)
        {
            var regex = new Regex("[^A-Za-z0-9_]");
            var url = regex.Replace(name, "");

            return url;
        }



        #region Get or Create
        protected void CreateUserIfNotExists(String userEmail, String userPassword, Boolean shouldActivateUser = false)
        {
            var userError = verifyUser(userEmail, userPassword);

            if (userError == null)
                return;

            switch (userError.Type)
            {
                case ExceptionPossibilities.InvalidUser:

                    SA.Safe.SaveUserAndSendVerify(userEmail, userPassword, Defaults.ConfigLanguage);

                    if (shouldActivateUser)
                    {
                        var token = DBHelper.GetLastTokenForUser(userEmail, userPassword, SecurityAction.UserVerification);

                        SA.Safe.ActivateUser(token);
                    }

                    return;

                case ExceptionPossibilities.DisabledUser:
                    return;

                default:
                    throw userError;
            }
        }

        private DFMCoreException verifyUser(string userEmail, string userPassword)
        {
            try
            {
                SA.Safe.ValidateUserAndCreateTicket(userEmail, userPassword, MyCookie.Get());
                return null;
            }
            catch (DFMCoreException e)
            {
                return e;
            }
        }


        protected Account GetOrCreateAccount(String accountUrl)
        {
            try
            {
                return SA.Admin.GetAccountByUrl(accountUrl);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidAccount)
                    throw;

                SA.Admin.CreateAccount(
                    new Account
                        {
                            Name = accountUrl, 
                            Url = accountUrl, 
                            User = User
                        });

                return SA.Admin.GetAccountByUrl(accountUrl);
            }
        }

        protected Category GetOrCreateCategory(String categoryName)
        {
            try
            {
                return SA.Admin.GetCategoryByName(categoryName);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidCategory)
                    throw;

                SA.Admin.CreateCategory(new Category { Name = categoryName, User = User });
                return SA.Admin.GetCategoryByName(categoryName);
            }
        }

        protected User GetSavedUser(String email, String password)
        {
            var key = SA.Safe.ValidateUserAndCreateTicket(email, password, MyCookie.Get());

            return SA.Safe.GetUserByTicket(key);
        }
        #endregion



        #region Context
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

        protected static Double YearAccountOutTotal
        {
            get { return Get<Double>("YearAccountOutTotal"); }
            set { Set("YearAccountOutTotal", value); }
        }

        protected static Double MonthAccountOutTotal
        {
            get { return Get<Double>("MonthAccountOutTotal"); }
            set { Set("MonthAccountOutTotal", value); }
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

        protected static Double YearAccountInTotal
        {
            get { return Get<Double>("YearAccountInTotal"); }
            set { Set("YearAccountInTotal", value); }
        }

        protected static Double MonthAccountInTotal
        {
            get { return Get<Double>("MonthAccountInTotal"); }
            set { Set("MonthAccountInTotal", value); }
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


        protected static String AccountUrl
        {
            get { return Get<String>("AccountUrl"); }
            set { Set("AccountUrl", value); }
        }

        protected static String CategoryName
        {
            get { return Get<String>("CategoryName"); }
            set { Set("CategoryName", value); }
        }



        protected static EmailStatus? CurrentEmailStatus
        {
            get { return Get<EmailStatus?>("CurrentEmailStatus"); }
            set { Set("CurrentEmailStatus", value); }
        }

        protected static Move Move
        {
            get { return Get<Move>("Move"); }
            set { Set("Move", value); }
        }

        protected static Schedule Schedule
        {
            get { return Get<Schedule>("Schedule"); }
            set { Set("Schedule", value); }
        }

        protected static DateTime Date
        {
            get
            {
                return Move == null 
                    ? Schedule == null 
                        ? DateTime.MinValue 
                        : Schedule.Date 
                    : Move.Date;
            }
            set
            {
                Set("Date", value);
            }
        }

        protected static IList<Detail> DetailList
        {
            get
            {
                return Move == null
                    ? Schedule == null 
                        ? new List<Detail>() 
                        : Schedule.DetailList 
                    : Move.DetailList;
            }
            set
            {
                Set("DetailList", value);
            }
        }
        #endregion



        protected const String UserEmail = "test@dontflymoney.com";
        protected static String UserPassword = "password";
        protected const String MainAccountUrl = "first_account";
        protected const String MainCategoryName = "first category";

        protected const String AccountOutName = "Account Out";
        protected const String AccountInName = "Account In";
        protected static String AccountOutUrl = MakeUrlFromName(AccountOutName);
        protected static String AccountInUrl = MakeUrlFromName(AccountInName);





        #region Helpers
        protected Detail GetDetailFromTable(TableRow detailData)
        {
            var newDetail = new Detail { Description = detailData["Description"] };

            if (!String.IsNullOrEmpty(detailData["Value"]))
                newDetail.Value = Double.Parse(detailData["Value"]);

            if (!String.IsNullOrEmpty(detailData["Amount"]))
                newDetail.Amount = Int16.Parse(detailData["Amount"]);
            return newDetail;
        }
        #endregion


    }
}
