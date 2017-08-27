package baseactivity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;

import com.dontflymoney.view.R;

public class Message
{
	Activity activity;
	
	Message(Activity activity)
	{
		this.activity = activity;
	}
	
	
	public void alertError(Object message)
	{
		alertError(message.toString());
	}
	
	protected void alertError(int resourceId, Exception e)
	{
		alertError(activity.getString(resourceId)+ ": " + e.getLocalizedMessage());
	}
	
	protected void alertError(int resourceId)
	{
		alertError(activity.getString(resourceId));
	}
	
	protected void alertError(String message)
	{
		new AlertDialog.Builder(activity)
			.setTitle(R.string.alert_title)
			.setMessage(message)
			.setPositiveButton(R.string.alert_button, new OnClickListener(){
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.cancel();
				}
	    	})
    		.show();
	}
}
