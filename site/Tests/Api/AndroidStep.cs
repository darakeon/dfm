using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.MVC.Areas.Api.Models;
using DFM.Tests.Util;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Api.Tests
{
    [Binding]
    public class AndroidStep : ContextHelper
    {
	    private static readonly String androidJsonPath = Path.Combine(
			"..", "..", "..", "..", "..", "..",
			"android", "DFM", "src",
			"test", "resources", "responses"
		);

	    private String json
	    {
		    get => get<String>("json");
		    set => set("json", value);
	    }

	    private Type type
	    {
		    get => get<Type>("type");
		    set => set("type", value);
		}

	    private object newObject
	    {
		    get => get<object>("newObject");
		    set => set("newObject", value);
	    }

	    private String newJson
	    {
		    get => get<String>("newJson");
		    set => set("newJson", value);
	    }

	    private IList<String> allJsons
	    {
		    get => get<IList<String>>("allJsons");
		    set => set("allJsons", value);
	    }

	    private IList<String> featureJsons
	    {
		    get => get<IList<String>>("featureJsons");
		    set => set("featureJsons", value);
	    }

		private readonly IDictionary<Type, Func<object>> defaults =
			new Dictionary<Type, Func<object>>
			{
				{ typeof(AccountsListModel), () => new AccountsListModel() },
				{ typeof(UserConfigModel), () => new UserConfigModel() },
				{ typeof(MovesExtractModel), () => new MovesExtractModel("", 0) },
				{ typeof(AuthModel), () => new AuthModel("") },
				{ typeof(MovesCreateModel), () => new MovesCreateModel() },
				{ typeof(MovesSummaryModel), () => new MovesSummaryModel("", 0) },
				{ typeof(object), () => new object() },
			};

		[Given(@"android json (.*)")]
        public void GivenAndroidJson(String jsonName)
        {
	        var path = Path.Combine(androidJsonPath, $"{jsonName}.json");
			json = File.ReadAllText(path);
        }
        
        [Given(@"\.NET object (.*)")]
        public void Given_NETObject(String netClass)
        {
	        if (netClass == "object")
	        {
		        type = typeof(object);
		        return;
	        }

	        var bl = typeof(BaseApiModel);

			var typePath = $"{bl.Namespace}.{netClass}";

			type = bl.Assembly.DefinedTypes
				.Single(t => t.FullName == typePath);
        }
        
        [Given(@"I have a list of all jsons")]
        public void GivenIHaveAListOfAllJsons()
        {
	        allJsons = Directory.GetFiles(
		        androidJsonPath
	        ).Select(
		        f => f.Remove(0, androidJsonPath.Length + 1)
			        .Replace(".json", "")
	        ).ToList();
        }
        
        [When(@"I deserialize json to \.NET")]
        public void WhenIDeserializeJsonToNet()
        {
	        newObject = JsonConvert.DeserializeObject(json, type);
        }
        
        [When(@"I serialize \.NET to json")]
        public void WhenISerializeNetToJson()
        {
	        newJson = JsonConvert.SerializeObject(defaults[type]());
        }
        
        [When(@"I check against this file list")]
        public void WhenICheckAgainstThisFileList()
        {
	        var regex = new Regex(@"\t\| (\w+) +\| \w+ +\|");

	        featureJsons = File.ReadAllLines("../../../Android.feature")
		        .Where(l => regex.IsMatch(l))
		        .Select(l => regex.Match(l).Groups[1].Value)
		        .ToList();
        }
        
        [Then(@"the new content will be same as default")]
        public void ThenTheNewContentWillBeSameAsDefault()
        {
            Assert.AreEqual(newObject, defaults[type]());
        }
        
        [Then(@"the new json will be same as original json")]
        public void ThenTheNewJsonWillBeSameAsOriginalJson()
        {
	        Assert.AreEqual(newJson, json);
        }
        
        [Then(@"I verify that all jsons are included")]
        public void ThenIVerifyThatAllJsonsAreIncluded()
        {
	        var shouldAppear = 2;

	        var missing = allJsons.Where(j =>
		        featureJsons.Count(fj => fj == j) < shouldAppear
		    );

			Assert.IsEmpty(missing, $"missing files: {missing}");
        }
    }
}
