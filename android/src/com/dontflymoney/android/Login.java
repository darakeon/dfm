package com.dontflymoney.android;

import java.util.ArrayList;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.dontflymoney.helpers.Request;

public class Login extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		super.onCreate(savedInstanceState);
		
		setContentView(R.layout.activity_login);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.login, menu);
		
		return true;
	}
	
	
	
	public void login(View view)
	{
		View form = (View)view.getParent();
		
		TextView errorField = (TextView)form.findViewById(R.id.error_message);

		EditText emailField = (EditText)form.findViewById(R.id.email);
		EditText passwordField = (EditText)form.findViewById(R.id.password);
		
		List<NameValuePair> parameters = new ArrayList<NameValuePair>();
		
		parameters.add(new BasicNameValuePair("email", emailField.getText().toString()));
		parameters.add(new BasicNameValuePair("password", passwordField.getText().toString()));

		Request task = new Request(errorField, parameters);
		
		task.execute();
		
		
		
	}
	
	
	
	

}
