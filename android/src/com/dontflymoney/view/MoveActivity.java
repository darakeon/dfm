package com.dontflymoney.view;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.DatePickerDialog;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.DatePicker;
import android.widget.LinearLayout;
import android.widget.Spinner;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.LinearLayoutDetail;

public class MoveActivity extends SmartActivity
{
	DatePickerDialog dialog;
	int day;
	int month;
	int year;
	
	Move move;

	public MoveActivity()
	{
		init(this, R.layout.activity_move, R.menu.move);
		move = new Move();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		populateScreen();
	}
	
	private void populateScreen()
	{
		Request request = new Request(this, "Moves/Create");
		request.AddParameter("ticket", Authentication.Get());
		request.Get(Step.Populate);
		
		Calendar today = Calendar.getInstance();
		day = getIntent().getIntExtra("month", today.get(Calendar.DAY_OF_MONTH));
		month = getIntent().getIntExtra("month", today.get(Calendar.MONTH));
		year = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
	}

	@Override
	public void HandlePost(Request request, Step step)
	{
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			try
			{
				switch (step)
				{
					case Populate:
					{
						populateScreen(result);
						break;
					}
					case Recording:
					{
						backToExtract();
						break;
					}
					default:
					{
						alertError(getString(R.string.this_is_not_happening));
						break;
					}
				}
			}
			catch (JSONException e)
			{
				alertError(getString(R.string.error_activity_json) + ": " + e.getMessage());
			}
		}
		else
		{
			alertError(request.GetError());
		}
	}
	
	private void populateScreen(JSONObject result)
		throws JSONException
	{
		JSONObject data = result.getJSONObject("data");
		
		JSONArray categoryList = data.getJSONArray("CategoryList");
		Spinner categorySpinner = (Spinner)findViewById(R.id.category);
		for(int c = 0; c < categoryList.length(); c++)
		{
			JSONObject category = categoryList.getJSONObject(c);
		}
		
		JSONArray accountList = data.getJSONArray("AccountList"); 
		Spinner accountSpinner = (Spinner)findViewById(R.id.account);
		
		for(int a = 0; a < accountList.length(); a++)
		{
			JSONObject account = accountList.getJSONObject(a);
		}
		
		JSONArray natureList = data.getJSONArray("NatureList");
		Spinner natureSpinner = (Spinner)findViewById(R.id.nature);
		
		for(int a = 0; a < natureList.length(); a++)
		{
			JSONObject nature = natureList.getJSONObject(a);
		}
	}
	
	private void backToExtract()
	{
		
	}
	

	
	public void showDatePicker(View view)
	{
		dialog = new DatePickerDialog(this, new PickDate(), year, month, day);
		dialog.show();
	}
	
	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
	    @Override
	    public void onDateSet(DatePicker view, int year, int month, int day)
	    {
	    	Calendar date = Calendar.getInstance();
	    	date.set(year, month, day);
		    SimpleDateFormat formatter = new SimpleDateFormat("DD/MM/yyyy", Locale.getDefault());
		    String dateInFull = formatter.format(date.getTime());

		    setValue(R.id.date, dateInFull);
	        dialog.hide();
	    }
	}
	


	public void useDetailed(View view)
	{
		findViewById(R.id.simple_value).setVisibility(View.GONE);
		findViewById(R.id.detailed_value).setVisibility(View.VISIBLE);
	}
	
	public void useSimple(View view)
	{
		findViewById(R.id.simple_value).setVisibility(View.VISIBLE);
		findViewById(R.id.detailed_value).setVisibility(View.GONE);
	}
	
	public void addDetail(View view)
	{
		String description = getValue(R.id.detail_description);
		String amountStr = getValue(R.id.detail_amount);
		String valueStr = getValue(R.id.detail_value);
		
		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty())
		{
			alertError(getString(R.string.fill_all));
			return;
		}
		
		int amount = Integer.parseInt(amountStr);
		double value = Double.parseDouble(valueStr);
		
		move.Add(description, amount, value);
		
   	    LinearLayoutDetail row = new LinearLayoutDetail(this, move, description, amount, value);
		LinearLayout list = (LinearLayout)view.getParent().getParent();
   	    list.addView(row);
	}
	
	
	public void refresh(MenuItem menuItem)
	{
		populateScreen();
	}
	
	
}
