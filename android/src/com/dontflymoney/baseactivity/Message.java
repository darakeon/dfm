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
	
	public void alertError(int resourceId, Exception e)
	{
		alertError(activity.getString(resourceId)+ ": " + e.getLocalizedMessage());
	}
	
	private void alertError(String message)
	{
		alertError(message, new OnClickListener(){
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.cancel();
			}
    	});
	}
	
	private void alertError(String message, OnClickListener clickListener)
	{
		new AlertDialog.Builder(activity)
			.setTitle(R.string.error_title)
			.setMessage(message)
			.setPositiveButton(R.string.ok_button, clickListener)
    		.show();
	}
	
	public void alertRetryLicense()
	{
		alertError(activity.getString(R.string.license_retry),
			new OnClickListener(){
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.cancel();
					activity.refresh();
				}
	    	}
		);
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
