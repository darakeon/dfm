package com.dontflymoney.view;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.DatePicker;
import android.widget.LinearLayout;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.entities.Constants;
import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.DialogSelectClickListener;
import com.dontflymoney.viewhelper.LinearLayoutDetail;

public class MoveActivity extends SmartActivity {
	DatePickerDialog dialog;

	Move move;

	JSONArray categoryList;
	JSONArray accountList;
	JSONArray natureList;


	
	public MoveActivity() {
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
		move.setDay(getIntent().getIntExtra("day", today.get(Calendar.DAY_OF_MONTH)));
		move.setMonth(getIntent().getIntExtra("month", today.get(Calendar.MONTH)));
		move.setYear(getIntent().getIntExtra("year", today.get(Calendar.YEAR)));
		move.OtherAccount = getIntent().getStringExtra("accounturl");
	}

	@Override
	protected void HandleSuccess(JSONObject result, Step step) throws JSONException
	{
		switch (step) {
			case Populate: {
				populateScreen(result);
				break;
			}
			case Recording: {
				backToExtract();
				break;
			}
			default: {
				alertError(getString(R.string.this_is_not_happening));
				break;
			}
		}
	}

	private void populateScreen(JSONObject result) throws JSONException
	{
		JSONObject data = result.getJSONObject("data");

		categoryList = data.getJSONArray("CategoryList");
		accountList = data.getJSONArray("AccountList");
		natureList = data.getJSONArray("NatureList");
	}

	
	
	private void backToExtract()
	{
		Intent intent = new Intent(this, ExtractActivity.class);
		intent.putExtra("accounturl", move.OtherAccount);
		intent.putExtra("month", move.getMonth());
		intent.putExtra("year", move.getYear());

		startActivity(intent);
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
		public void onDateSet(DatePicker view, int year, int month, int day) {
			Calendar date = Calendar.getInstance();
			date.set(year, month, day);
			SimpleDateFormat formatter = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
			String dateInFull = formatter.format(date.getTime());
			
			MoveActivity.this.move.setYear(year);
			MoveActivity.this.move.setMonth(month);
			MoveActivity.this.move.setDay(day);

			setValue(R.id.date, dateInFull);
			dialog.hide();
		}
	}

	
	
	public void changeCategory(View view) throws JSONException
	{
		showChangeList(categoryList, R.string.category, new DialogCategory(categoryList));
	}

	class DialogCategory extends DialogSelectClickListener
	{
		public DialogCategory(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			setValue(R.id.category, text);
			move.Category = value;
		}

		@Override
		public void handleError(JSONException exception)
		{
			alertError(R.string.error_convert_result);
		}
	}

	
	
	public void changeNature(View view) throws JSONException
	{
		showChangeList(natureList, R.string.nature, new DialogNature(natureList));
	}

	class DialogNature extends DialogSelectClickListener
	{
		public DialogNature(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			setValue(R.id.nature, text);
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
			alertError(R.string.error_convert_result);
		}
	}

	
	
	public void changeAccount(View view) throws JSONException
	{
		showChangeList(accountList, R.string.account, new DialogAccount(accountList));
	}

	class DialogAccount extends DialogSelectClickListener
	{
		public DialogAccount(JSONArray list) { super(list); }

		@Override
		public void setResult(String text, String value)
		{
			setValue(R.id.account, text);
			move.OtherAccount = value;
		}

		@Override
		public void handleError(JSONException exception)
		{
			alertError(R.string.error_convert_result);
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
		String description = getValue(R.id.detail_description);
		String amountStr = getValue(R.id.detail_amount);
		String valueStr = getValue(R.id.detail_value);

		if (description.isEmpty() || amountStr.isEmpty() || valueStr.isEmpty()) {
			alertError(getString(R.string.fill_all));
			return;
		}
		
		setValue(R.id.detail_description, "");
		setValue(R.id.detail_amount, "");
		setValue(R.id.detail_value, "");

		int amount = Integer.parseInt(amountStr);
		double value = Double.parseDouble(valueStr);

		move.Add(description, amount, value);

		LinearLayoutDetail row = new LinearLayoutDetail(this, move, description, amount, value);
		LinearLayout list = (LinearLayout) findViewById(R.id.details);
		list.addView(row);
	}
	
	
	
	public void refresh(MenuItem menuItem) {
		populateScreen();
	}

}
