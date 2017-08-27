package com.dontflymoney.api;

public class Site
{
	public static final String Domain = "beta.dontflymoney.com";
	
	//TODO: DELETE IT
	public static Boolean IsLocal()
	{
		return Domain != "beta.dontflymoney.com";
	}
}
