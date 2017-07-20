using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFM.BusinessLogic;
using DFM.Multilanguage;
using DFM.Repositories;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    [Binding]
    public class MainStep : BaseStep
    {
        [Then(@"I will receive this error")]
        public void ThenIWillReceiveThisError(Table table)
        {
            var error = table.Rows[0]["Error"];

            Assert.IsNotNull(Error);

            Assert.AreEqual(error, Error.Type.ToString());
        }

        [Then(@"I will receive no error")]
        public void ThenIWillReceiveNoError()
        {
            Assert.IsNull(Error);
        }

    }
}
