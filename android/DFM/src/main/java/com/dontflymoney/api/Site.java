package com.dontflymoney.api;

public class Site
{
	private static final String publicDomain = "dontflymoney.com";
	static final String Domain = publicDomain;
	
	static String GetProtocol()
	{
		return IsLocal() ? "http" : "https";
	}
	
	public static Boolean IsLocal()
	{
		return !Domain.equals(publicDomain);
	}
	
	
}
