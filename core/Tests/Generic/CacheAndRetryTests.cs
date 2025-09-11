using System;
using System.Threading;
using Keon.Util.Exceptions;
using NUnit.Framework;

namespace DFM.Generic.Tests;

internal class CacheAndRetryTests
{
	[SetUp]
	public void SetCfg()
	{
		Cfg.Init();
	}

	[Test]
	public void Get_NullKey_ReturnDefault()
	{
		var cacheAndRetry = new CacheAndRetry<String>(
			null, "default", (key) => $"new {key}"
		);

		var result = cacheAndRetry.Get();
		Assert.That(result, Is.EqualTo("default"));
	}
	[Test]
	public void Get_EmptyKey_ReturnDefault()
	{
		var cacheAndRetry = new CacheAndRetry<String>(
			"", "default", (key) => $"new {key}"
		);

		var result = cacheAndRetry.Get();
		Assert.That(result, Is.EqualTo("default"));
	}

	[Test]
	public void Get_FromDb()
	{
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_FromDb", "default", (key) => $"new {key}"
		);

		var result = cacheAndRetry.Get();
		Assert.That(result, Is.EqualTo("new Get_FromDb"));
	}

	[Test]
	public void Get_AlreadyCached()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_AlreadyCached", "default", (key) =>
			{
				counter++;
				return $"new {key} {counter}";
			}
		);

		cacheAndRetry.Get();
		var result = cacheAndRetry.Get();
		Assert.That(result, Is.EqualTo("new Get_AlreadyCached 1"));
	}

	[Test]
	public void Get_CachedExpired()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_CachedExpired", "default", (key) =>
			{
				counter++;
				return $"new {key} {counter}";
			}
		);

		cacheAndRetry.Get();
		Thread.Sleep(Cfg.MillisecondsToCache);
		var result = cacheAndRetry.Get();
		Assert.That(result, Is.EqualTo("new Get_CachedExpired 2"));
	}

	[Test]
	public void Get_CleanUp()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_CleanUp", "default", (key) =>
			{
				counter++;
				return $"new {key} {counter}";
			}
		);

		Assert.That(cacheAndRetry.Size(), Is.EqualTo(0));

		var result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("new Get_CleanUp 1"));
		Assert.That(cacheAndRetry.Size(), Is.EqualTo(1));

		Thread.Sleep(Cfg.MillisecondsToCache);

		Assert.That(cacheAndRetry.Size(), Is.EqualTo(1));

		result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("new Get_CleanUp 2"));
		Assert.That(cacheAndRetry.Size(), Is.EqualTo(1));
	}

	[Test]
	public void Get_Retry_OnFail()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_Retry_OnFail", "default", (key) =>
			{
				counter++;
				if (counter == 1) throw new Exception();
				return $"new {key} attempt {counter}";
			}
		);

		var result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("new Get_Retry_OnFail attempt 2"));
	}

	[Test]
	public void Get_Retry_FailAllTimesReturnDefault()
	{
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_Retry_FailAllTimesReturnDefault",
			"default",
			(_) => throw new Exception()
		);

		var result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("default"));
	}

	[Test]
	public void Get_SystemError_ReturnDefault()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_SystemError_ReturnDefault", "default", (key) =>
			{
				counter++;
				if (counter == 1) throw new SystemError("");
				return $"new {key} attempt {counter}";
			}
		);

		var result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("default"));
	}

	[Test]
	public void Get_DKException_ReturnDefault()
	{
		var counter = 0;
		var cacheAndRetry = new CacheAndRetry<String>(
			"Get_DKException_ReturnDefault", "default", (key) =>
			{
				counter++;
				if (counter == 1) throw new DKException("");
				return $"new {key} attempt {counter}";
			}
		);

		var result = cacheAndRetry.Get();

		Assert.That(result, Is.EqualTo("default"));
	}
}
