package com.dontflymoney.viewhelper;

import android.app.ProgressDialog;
import android.content.Intent;
import android.net.Uri;

import com.dontflymoney.api.Site;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.view.R;
import com.google.android.vending.licensing.LicenseCheckerCallback;
import com.google.android.vending.licensing.Policy;

public class DfmLicenseCheckerCallback implements LicenseCheckerCallback
{
	SmartActivity activity;
	ProgressDialog progress;
	
	public DfmLicenseCheckerCallback(SmartActivity activity, ProgressDialog progress)
	{
		this.activity = activity;
		this.progress = progress;
	}
	
    public void allow(int reason)
    {
        progress.dismiss();

        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        if (reason == Policy.RETRY)
        {
        	activity.getMessage().alertRetryLicense();
        }
        else
        {
            enableScreen();
        }
    }
    
    private void enableScreen()
    {
    	activity.runOnUiThread(new Runnable()
        {
		    @Override
	        public void run()
	        {
		        activity.EnableScreen();
	        }
        });
    }
    
    public void dontAllow(int reason)
    {
        progress.dismiss();

        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        if (reason == Policy.RETRY)
        {
        	activity.getMessage().alertRetryLicense();
        }
        else if (Site.IsLocal())
        {
        	enableScreen();
        }
        else
        {
        	Intent intent = new Intent(Intent.ACTION_VIEW);
        	intent.setData(Uri.parse("market://details?id=" + activity.getPackageName()));
        	activity.startActivity(intent);
        }
    }

	@Override
	public void applicationError(int errorCode)
	{
        progress.dismiss();

        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        String genericMessage = activity.getString(R.string.license_error);
        String specificMessage = String.format(genericMessage, errorCode);
        
		activity.getMessage().alertError(specificMessage);
	}
	
}
