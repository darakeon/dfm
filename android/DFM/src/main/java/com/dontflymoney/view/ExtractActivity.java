package com.dontflymoney.view;

import java.lang.reflect.Field;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnLongClickListener;
import android.widget.DatePicker;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.IYesNoDialogAnswer;
import com.dontflymoney.baseactivity.SmartActivity;

public class ExtractActivity extends SmartActivity implements IYesNoDialogAnswer
{
	static JSONArray moveList;
	static String name;
	static double total;
	
	TableLayout main;
	String accounturl;

	DatePickerDialog dialog;
	private static int month;
	private static int year;
	
	public int moveId;

	
	public ExtractActivity()
	{
		init(R.layout.activity_extract, R.menu.extract);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setCurrentInfo();

		if (rotated && succeded)
		{
			setDateFromLast();
			
			try
			{
				fillMoves();
			}
			catch (JSONException e)
			{
				message.alertError(R.string.error_activity_json, e);
			}
		}
		else
		{
			setDateFromCaller();
			getExtract();
		}
	}
	
	private void setCurrentInfo()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		accounturl = getIntent().getStringExtra("accounturl");
	}
	
	private void setDateFromLast()
	{
		setDate(month, year);
	}
	
	private void setDateFromCaller()
	{
		Calendar today = Calendar.getInstance();
		int startMonth = getIntent().getIntExtra("month", today.get(Calendar.MONTH));
		int startYear = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		setDate(startMonth, startYear);
	}

	@SuppressLint("SimpleDateFormat")
	private void setDate(int month, int year)
	{
		ExtractActivity.month = month;
		ExtractActivity.year = year;
		
		Calendar date = Calendar.getInstance();
	    date.set(Calendar.MONTH, month);
	    date.set(Calendar.YEAR, year);
	    
	    SimpleDateFormat formatter = new SimpleDateFormat("MMM/yyyy");
	    String dateInFull = formatter.format(date.getTime());
		
	    form.setValue(R.id.reportDate, dateInFull);
	}
	
    public void changeDate(View v)
    {
		if(dialog == null)
		{
			dialog = new DatePickerDialog(this, new PickDate(), year, month, 1);

			try
			{
		        Field pickerField = dialog.getClass().getDeclaredField("mDatePicker");
		        pickerField.setAccessible(true);
		        DatePicker datePicker = (DatePicker) pickerField.get(dialog);
	            
	            Field field = datePicker.getClass().getDeclaredField("mDaySpinner");
	            field.setAccessible(true);
	            Object dayPicker = field.get(datePicker);
	            ((View) dayPicker).setVisibility(View.GONE);
		    } 
		    catch (Exception e) { }
		}
		
		dialog.show();
    }
	
	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
	    @Override
	    public void onDateSet(DatePicker view, int year, int month, int day)
	    {
	        setDate(month, year); 
			getExtract();
	        dialog.hide();
	    }
	    
	}
	
	private void getExtract()
	{
		main.removeAllViews();
		
		request = new Request(this, "Moves/Extract");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", year * 100 + month + 1);
		
		request.Post(Step.Populate);
	}
	
	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		switch (step)
		{
			case Populate: {
				moveList = data.getJSONArray("MoveList");
				name = data.getString("Name");
				total = data.getDouble("Total");
				
				fillMoves();
				break;
			}
			case Recording: {
				refresh();
				break;
			}
			default: {
				message.alertError(R.string.this_is_not_happening);
				break;
			}
		}
	}
	
	private void fillMoves()
		throws JSONException
	{
		form.setValue(R.id.totalTitle, name);
		form.setValueColored(R.id.totalValue, total);
		
		if (moveList.length() == 0)
		{
			View empty = form.createText(getString(R.string.no_extract), Gravity.CENTER);
			main.addView(empty);
		}
		else
		{
			for(int a = 0; a < moveList.length(); a++)
			{
				int color = a % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
				
				getMove(moveList.getJSONObject(a), color);
			}
		}
	}

	private void getMove(JSONObject move, int color)
			throws JSONException
	{
		TableRow row = new TableRow(this);
		row.setBackgroundColor(color);
		
		String description = move.getString("Description");
		row.addView(form.createText(description, Gravity.LEFT));
		
		if (getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE)
		{
			JSONObject date = move.getJSONObject("Date");
			
			Calendar calendar = Calendar.getInstance();
            calendar.set(Calendar.YEAR, date.getInt("Year"));
			calendar.set(Calendar.MONTH, date.getInt("Month") - 1);
            calendar.set(Calendar.DAY_OF_MONTH, date.getInt("Day"));
            
            DateFormat format = DateFormat.getDateInstance(DateFormat.SHORT);
			
            TextView cell = form.createText(format.format(calendar.getTime()), Gravity.CENTER);
            cell.setPadding(20, 20, 200, 20);
            
			row.addView(cell);
		}

		double total = move.getDouble("Total");
		row.addView(form.createText(total, Gravity.RIGHT));
		
		int id = move.getInt("ID");

		setRowClick(row, description, id);

		main.addView(row);
	}
	
	
	
	
	private void setRowClick(TableRow row, final String moveDescription, final int moveId)
	{
		row.setClickable(true);
		
		final ExtractActivity activity = this;
		
		row.setOnLongClickListener(new OnLongClickListener(){

			@Override
			public boolean onLongClick(View v) {
				
				String messageText = getString(R.string.sure_to_delete);
				
				messageText = String.format(messageText, moveDescription);
				
				activity.moveId = moveId; 
				
				message.alertYesNo(messageText, activity);

				return false;
			}
			
		});
		
		row.setOnClickListener(new OnClickListener()
		{
		    public void onClick(View row)
		    {
		        //TableRow tablerow = (TableRow)row;
		        //TextView sample = (TextView) tablerow.getChildAt(0);
		        //alertError(sample.getText().toString());
		        
		        //redirect(MovesActivity.class);
		    }
		});
	}
	
	
	
	public void goToSummary(MenuItem item)
	{
		Intent intent = new Intent(this, SummaryActivity.class);
		
		intent.putExtra("accounturl", accounturl);
		intent.putExtra("year", year);
		
		startActivity(intent);
	}
	
	public void goToMove(MenuItem item)
	{
		Intent intent = new Intent(this, MovesCreateActivity.class);
		
		intent.putExtra("accounturl", accounturl);
		intent.putExtra("year", year);
		intent.putExtra("month", month);
		
		startActivity(intent);
	}



	

	@Override
	public void YesAction() { 		
		request = new Request(this, "Moves/Delete");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", moveId);
		
		request.Post(Step.Recording);
	}

	@Override
	public void NoAction() { }
	
}
