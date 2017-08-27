package com.dontflymoney.api;

public class Site
{
	private static final String publicDomain = "https://dontflymoney.com";
	public static final String Domain = publicDomain;
	
	//TODO: DELETE IT
	public static Boolean IsLocal()
	{
		return Domain != publicDomain;
	}
}
