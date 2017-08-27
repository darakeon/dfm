package com.dontflymoney.baseactivity;

import android.app.ProgressDialog;

import com.dontflymoney.auth.Unique;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DfmLicenseCheckerCallback;
import com.google.android.vending.licensing.AESObfuscator;
import com.google.android.vending.licensing.LicenseChecker;
import com.google.android.vending.licensing.ServerManagedPolicy;

public class License
{
	final byte[] SALT = new byte[] {
			-21, +84, -38, +79, -81, -74, -98, +73, -14, +93,
			-27, -94, -87, -29, +57, +42, +62, -78, -54, -29, 
		};
	
	SmartActivity activity;
	LicenseChecker checker;
	DfmLicenseCheckerCallback callback;
	ProgressDialog progress;
		
	public License(SmartActivity activity)
	{
		this.activity = activity;
		progress = activity.getMessage().getWaitDialog();
		
		AESObfuscator obfuscator = new AESObfuscator(SALT, activity.getPackageName(), Unique.GetKey());
		ServerManagedPolicy policy = new ServerManagedPolicy(activity, obfuscator); 
		String appKey = activity.getString(R.string.license_key);
		
		checker = new LicenseChecker(activity, policy, appKey);
		callback = new DfmLicenseCheckerCallback(activity, progress);			
	}
	
	public void Check()
	{
		checker.checkAccess(callback);
		progress.show();
	}
	
	public void Destroy()
	{
		checker.onDestroy();

		if (progress.isShowing())
			progress.dismiss();
	}
	
	
}
