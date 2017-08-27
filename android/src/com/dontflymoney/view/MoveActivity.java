package com.dontflymoney.view;

import java.util.Calendar;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.LinearLayout;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.entities.Constants;
import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.AfterTextWatcher;
import com.dontflymoney.viewhelper.DetailBox;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

public class MoveActivity extends SmartActivity {
	DatePickerDialog dialog;

	Move move;

	static boolean useCategories;
	static JSONArray categoryList;
	static JSONArray natureList;
	static JSONArray accountList;


	
	public MoveActivity() {
		init(R.layout.activity_move, R.menu.move);
		move = new Move();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
		if (rotated)
		{
			try
			{
				populateCategoryAndNature();
			}
			catch (JSONException e)
			{
				message.alertError(R.string.error_activity_json, e);
			}
		}
		else
		{
			populateScreen();
		}
	}

	private void populateScreen()
	{
		Request request = new Request(this, "Moves/Create");
		request.AddParameter("ticket", Authentication.Get());
		request.Get(Step.Populate);

		Calendar today = Calendar.getInstance();
		int day = getIntent().getIntExtra("day", today.get(Calendar.DAY_OF_MONTH));
		int month = getIntent().getIntExtra("month", today.get(Calendar.MONTH));
		int year = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		
		move.Date.set(year, month, day);
		form.setValue(R.id.date, move.DateString());
		move.PrimaryAccount = getIntent().getStringExtra("accounturl");
		
		setDescriptionListener();
		setValueListener();
	}

	
	
	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		switch (step) {
			case Populate: {
				populateScreen(data);
				break;
			}
			case Recording: {
				backToExtract();
				break;
			}
			default: {
				message.alertError(R.string.this_is_not_happening);
				break;
			}
		}
	}

	private void populateScreen(JSONObject data) throws JSONException
	{
		useCategories = data.getBoolean("UseCategories");
		
		if (useCategories)
			categoryList = data.getJSONArray("CategoryList");
		
		natureList = data.getJSONArray("NatureList");
		accountList = data.getJSONArray("AccountList");
		
		populateCategoryAndNature();
	}
	
	private void populateCategoryAndNature()
		throws JSONException
	{
		if (useCategories)
		{
			findViewById(R.id.category_box).setVisibility(View.VISIBLE);
		}
		else
		{
			findViewById(R.id.category_box).setVisibility(View.GONE);
		}
		
		JSONObject firstNature = natureList.getJSONObject(0);
		
		form.setValue(R.id.nature, firstNature.getString("Text"));
		move.Nature = firstNature.getInt("Value");
	}

	
	
	private void setDescriptionListener()
	{
		EditText textMessage = (EditText) findViewById(R.id.description);
	    textMessage.addTextChangedListener(new DescriptionWatcher());
	}
	
	private class DescriptionWatcher extends AfterTextWatcher
	{
		@Override
		public void textChanged(String text)
		{
			move.Description = text;
		}
	}
	
	
	
	public void showDatePicker(View view)
	{
		dialog = 
			new DatePickerDialog(
				this,
				new PickDate(),
				move.getYear(),
				move.getMonth(),
				move.getDay()
			);
		dialog.show();
	}

	private class PickDate implements DatePickerDialog.OnDateSetListener {
		@Override
		public void onDateSet(DatePicker view, int year, int month, int day)
		{
			move.Date.set(year, month, day);

			form.setValue(R.id.date, move.DateString());
			dialog.hide();
		}
	}

	
	
	public void changeCategory(View view) throws JSONException
	{
		form.showChangeList(categoryList, R.string.category, new DialogCategory(categoryList));
	}

	class DialogCategory extends DialogSelectClickListener
	{
		public DialogCategory(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			form.setValue(R.id.category, text);
			move.Category = value;
		}

		@Override
		public void handleError(JSONException exception)
		{
			message.alertError(R.string.error_convert_result);
		}
	}

	
	
	public void changeNature(View view) throws JSONException
	{
		form.showChangeList(natureList, R.string.nature, new DialogNature(natureList));
	}

	class DialogNature extends DialogSelectClickListener
	{
		public DialogNature(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			form.setValue(R.id.nature, text);
			move.Nature = Integer.parseInt(value);
			
			int accountVisibility =
				move.Nature == Constants.MoveNatureTransfer
					? View.VISIBLE : View.GONE;
			
			findViewById(R.id.account).setVisibility(accountVisibility);
			findViewById(R.id.account_label).setVisibility(accountVisibility);
				
		}

		@Override
		public void handleError(JSONException exception)
		{
			message.alertError(R.string.error_convert_result);
		}
	}

	
	
	public void changeAccount(View view) throws JSONException
	{
		form.showChangeList(accountList, R.string.account, new DialogAccount(accountList));
	}

	class DialogAccount extends DialogSelectClickListener
	{
		public DialogAccount(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			form.setValue(R.id.account, text);
			move.OtherAccount = value;
		}

		@Override
		public void handleError(JSONException exception)
		{
			message.alertError(R.string.error_convert_result);
		}
	}

	
	
	private void setValueListener()
	{
		EditText textMessage = (EditText) findViewById(R.id.value);
	    textMessage.addTextChangedListener(new ValueWatcher());
	}
	
	private class ValueWatcher extends AfterTextWatcher
	{
		@Override
		public void textChanged(String text)
		{
			try { move.Value = Double.parseDouble(text); }
			catch (NumberFormatException e) { }
		}
	}
	
	
	
	public void useDetailed(View view) {
		findViewById(R.id.simple_value).setVisibility(View.GONE);
		findViewById(R.id.detailed_value).setVisibility(View.VISIBLE);
	}

	public void useSimple(View view) {
		findViewById(R.id.simple_value).setVisibility(View.VISIBLE);
		findViewById(R.id.detailed_value).setVisibility(View.GONE);
	}

	public void addDetail(View view) {
		String description = form.getValue(R.id.detail_description);
		String amountStr = form.getValue(R.id.detail_amount);
		String valueStr = form.getValue(R.id.detail_value);

		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty()) {
			message.alertError(R.string.fill_all);
			return;
		}
		
		int amountDefault = getResources().getInteger(R.integer.amount_default);
		
		form.setValue(R.id.detail_description, "");
		form.setValue(R.id.detail_amount, amountDefault);
		form.setValue(R.id.detail_value, "");

		int amount = Integer.parseInt(amountStr);
		double value = Double.parseDouble(valueStr);

		move.Add(description, amount, value);

		DetailBox row = new DetailBox(this, move, description, amount, value);
		LinearLayout list = (LinearLayout) findViewById(R.id.details);
		list.addView(row);
	}
	
	
	
	public void save(View view)
	{
		Request request = new Request(this, "Moves/Create");
		request.AddParameter("ticket", Authentication.Get());
		move.setParameters(request);
		request.Post(Step.Recording);
	}
	
	private void backToExtract()
	{
		Intent intent = new Intent(this, ExtractActivity.class);
		intent.putExtra("accounturl", move.PrimaryAccount);
		intent.putExtra("month", move.getMonth());
		intent.putExtra("year", move.getYear());

		startActivity(intent);
	}

	

}
