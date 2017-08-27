package com.dontflymoney.api;

public class Site
{
	private static final String publicDomain = "dontflymoney.com";
	public static final String Domain = publicDomain;
	
	public static String GetProtocol()
	{
		return IsLocal() ? "http" : "https";
	}
	
	//TODO: DELETE IT
	public static Boolean IsLocal()
	{
		return Domain != publicDomain;
	}
	
	
}
