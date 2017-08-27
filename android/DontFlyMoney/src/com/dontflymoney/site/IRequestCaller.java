package com.dontflymoney.site;

import org.json.JSONObject;

public interface IRequestCaller
{
	void DoOnReturn(JSONObject json);

	void Error(String errorMessage);

}
