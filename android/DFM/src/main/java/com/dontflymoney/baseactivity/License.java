package com.dontflymoney.baseactivity;

import android.app.ProgressDialog;

import com.dontflymoney.userdata.Unique;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DfmLicenseCheckerCallback;
import com.google.android.vending.licensing.AESObfuscator;
import com.google.android.vending.licensing.LicenseChecker;
import com.google.android.vending.licensing.ServerManagedPolicy;

public class License
{
	private SmartActivity activity;
	private LicenseChecker checker;
	private DfmLicenseCheckerCallback callback;
	private ProgressDialog progress;
		
	public License(SmartActivity activity)
	{
		this.activity = activity;
		progress = activity.getMessage().getWaitDialog();

		callback = new DfmLicenseCheckerCallback(activity, progress);

		byte[] SALT = new byte[]{
				-21, +84, -38, +79, -81, -74, -98, +73, -14, +93,
				-27, -94, -87, -32, +57, +42, +62, -78, -54, -29,
		};
		String appKey = activity.getString(R.string.license_key);

		AESObfuscator obfuscator = new AESObfuscator(SALT, activity.getPackageName(), Unique.GetKey(activity));
		ServerManagedPolicy policy = new ServerManagedPolicy(activity, obfuscator); 
		checker = new LicenseChecker(activity, policy, appKey);
	}
	
	public void Check()
	{
		activity.Reset();
		checker.checkAccess(callback);
		progress.show();
	}
	
	void Destroy()
	{
		checker.onDestroy();

		if (progress.isShowing())
			progress.dismiss();
	}
	
	
}
