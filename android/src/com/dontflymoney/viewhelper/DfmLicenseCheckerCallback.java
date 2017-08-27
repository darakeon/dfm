package com.dontflymoney.viewhelper;

import android.content.Intent;
import android.net.Uri;

import com.dontflymoney.baseactivity.Message;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.view.R;
import com.google.android.vending.licensing.LicenseCheckerCallback;
import com.google.android.vending.licensing.Policy;

public class DfmLicenseCheckerCallback implements LicenseCheckerCallback
{
	SmartActivity activity;
	Message message;
	
	public DfmLicenseCheckerCallback(SmartActivity activity, Message message)
	{
		this.activity = activity;
		this.message = message;
	}
	
    public void allow(int reason)
    {
        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        if (reason == Policy.RETRY)
        {
        	message.alertRetryLicense();
        }
        else
        {
            message.alertError("Allow");
        }
    }

    public void dontAllow(int reason)
    {
        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        message.alertError("dont_allow");
        
        if (reason == Policy.RETRY)
        {
        	message.alertRetryLicense();
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
        if (activity.isFinishing())
        {
            // Don't update UI if Activity is finishing.
            return;
        }

        String genericMessage = activity.getString(R.string.license_error);
        String specificMessage = String.format(genericMessage, errorCode);
        
        message.alertError(specificMessage);
	}
	
}
