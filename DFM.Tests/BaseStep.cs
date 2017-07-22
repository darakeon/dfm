using System;
using System.IO;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
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
            NHManager.Close();
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

        protected const String CentralUserEmail = "test@dontflymoney.com";
        protected static String CentralUserPassword = "password";
        protected const String CentralAccountName = "first account";
        protected const String CentralCategoryName = "first category";

    }
}
