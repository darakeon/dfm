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

        protected const String CentralUserEmail = "test@dontflymoney.com";
        protected static String CentralUserPassword = "password";

    }
}
