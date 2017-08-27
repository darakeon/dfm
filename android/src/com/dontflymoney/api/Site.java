package com.dontflymoney.api;

public class Site
{
	public static final String Domain = "192.168.1.31";
	
	//TODO: DELETE IT
	public static Boolean IsLocal()
	{
		return Domain != "beta.dontflymoney.com";
	}
}
