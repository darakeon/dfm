package com.dontflymoney.baseactivity;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;

import com.dontflymoney.view.R;

public class Message
{
	SmartActivity activity;
	
	Message(SmartActivity activity)
	{
		this.activity = activity;
	}
	
	
	public void alertError(Object message)
	{
		alertError(message.toString());
	}
	
	public void alertError(int resMessage, Exception e)
	{
		alertError(activity.getString(resMessage)+ ": " + e.getLocalizedMessage());
	}
	
	private void alertError(String message)
	{
		alertError(message, R.string.ok_button, false, new OnClickListener(){
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.cancel();
			}
    	});
	}
	
	private void alertError(String message, int resOkButton, boolean hasCancelButton, OnClickListener clickListener)
	{
		AlertDialog.Builder builder = 
			new AlertDialog.Builder(activity)
				.setTitle(R.string.error_title)
				.setMessage(message);
		
		if (resOkButton != 0)
			builder.setPositiveButton(resOkButton, clickListener);
		
		if (hasCancelButton)
			builder.setNegativeButton(R.string.cancel_button, clickListener);
			
		builder.show();
	}
	
	public void alertRetryLicense()
	{
		String message = activity.getString(R.string.license_retry);
		
		alertError(message, R.string.try_again, true, new OnClickListener(){
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.cancel();
				activity.refresh();
			}
    	});
	}
	
	


	public ProgressDialog getWaitDialog()
	{
		ProgressDialog progress = new ProgressDialog(activity);
		progress.setTitle(activity.getString(R.string.wait_title));
		progress.setMessage(activity.getString(R.string.wait_text));
		
		return progress;
	}
	
	public ProgressDialog showWaitDialog()
	{
		ProgressDialog progress = getWaitDialog();
		progress.show();
		
		return progress;
	}
	
	
	
}
