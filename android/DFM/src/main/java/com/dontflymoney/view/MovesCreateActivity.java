package com.dontflymoney.view;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;

import com.dontflymoney.api.InternalRequest;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.dialogs.DialogAccountIn;
import com.dontflymoney.dialogs.DialogAccountOut;
import com.dontflymoney.entities.Detail;
import com.dontflymoney.entities.Move;
import com.dontflymoney.entities.Nature;
import com.dontflymoney.listeners.DialogCategory;
import com.dontflymoney.listeners.DialogNature;
import com.dontflymoney.listeners.IDatePickerActivity;
import com.dontflymoney.listeners.PickDate;
import com.dontflymoney.viewhelper.DetailBox;
import com.dontflymoney.watchers.DescriptionWatcher;
import com.dontflymoney.watchers.ValueWatcher;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Calendar;

public class MovesCreateActivity extends SmartActivity implements IDatePickerActivity
{
	DatePickerDialog dialog;
	ScrollView window;

	static Move move;
	static boolean useCategories;
	static JSONArray categoryList;
	static JSONArray natureList;
	static JSONArray accountList;



	protected int contentView() { return R.layout.moves_create; }



	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		window = (ScrollView) findViewById(R.id.window);

		if (rotated && succeded)
		{
			try
			{
				populateCategoryAndNature();
				setControls();
				populateOldData(false);
			}
			catch (JSONException e)
			{
				message.alertError(R.string.error_activity_json, e);
			}
		}
		else
		{
			move = new Move();
			populateScreen();
		}
	}

	private void populateOldData(boolean populateAll) throws NumberFormatException, JSONException
	{
		if (move != null)
		{
			if (useCategories)
			{
				setDataFromList(categoryList, move.Category, R.id.category);
			}

			form.setValue(R.id.date, move.DateString());

			for(int n = 0; n < natureList.length(); n++)
			{
				JSONObject nature = natureList.getJSONObject(n);
				int comparValue = nature.getInt("Value");

				if (comparValue == move.Nature.GetNumber())
				{
					String text = nature.getString("Text");
					String value = nature.getString("Value");
					setNature(text, value);
					break;
				}
			}

			setDataFromList(accountList, move.AccountOut, R.id.account_out);
			setDataFromList(accountList, move.AccountIn, R.id.account_in);

			if (move.isDetailed)
			{
				for(int d = 0; d < move.Details.size(); d++)
				{
					Detail detail = move.Details.get(d);

					addViewDetail(move, detail.Description, detail.Amount, detail.Value);
				}

				useDetailed();
			}

			if (!populateAll)
				return;

			EditText descriptionView = (EditText) findViewById(R.id.description);
			descriptionView.setText(move.Description);

			if (move.Value != 0)
			{
				EditText valueView = (EditText) findViewById(R.id.value);
				valueView.setText(String.format("%1$,.2f", move.Value));
			}

		}
	}

	private void setDataFromList(JSONArray list, String dataSaved, int resourceId) throws JSONException
	{
		for (int n = 0; n < list.length(); n++)
		{
			JSONObject object = list.getJSONObject(n);
			String value = object.getString("Value");

			if (value.equals(dataSaved))
			{
				String text = object.getString("Text");
				form.setValue(resourceId, text);
				break;
			}
		}
	}

	private void populateScreen()
	{
		request = new InternalRequest(this, "Moves/Create");
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accountUrl", getIntent().getStringExtra("accountUrl"));
		request.AddParameter("id", getIntent().getIntExtra("id", 0));
		request.Get(Step.Populate);

		setCurrentDate();
		setControls();
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

		if (data.has("Move") && !data.isNull("Move"))
		{
			JSONObject moveToEdit = data.getJSONObject("Move");
			move.SetData(moveToEdit, getIntent().getStringExtra("accountUrl"));
			populateOldData(true);
		}
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

		if (move.Nature == null)
		{
			JSONObject firstNature = natureList.getJSONObject(0);
			form.setValue(R.id.nature, firstNature.getString("Text"));
			move.SetNature(firstNature.getInt("Value"));
		}
	}



	private void setCurrentDate()
	{
		Calendar today = Calendar.getInstance();
		int day = getIntent().getIntExtra("day", today.get(Calendar.DAY_OF_MONTH));
		int month = getIntent().getIntExtra("month", today.get(Calendar.MONTH));
		int year = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		move.Date.set(year, month, day);
	}

	private void setControls()
	{
		form.setValue(R.id.date, move.DateString());

		setDescriptionListener();
		setValueListener();
	}

	private void setDescriptionListener()
	{
		EditText textMessage = (EditText) findViewById(R.id.description);
		textMessage.addTextChangedListener(new DescriptionWatcher(move));
	}

	private void setValueListener()
	{
		EditText textMessage = (EditText) findViewById(R.id.value);
		textMessage.addTextChangedListener(new ValueWatcher(move));
	}



	public void showDatePicker(View view)
	{
		dialog =
			new DatePickerDialog(this
				, new PickDate(this)
				, move.getYear(), move.getMonth(), move.getDay()
			);

		dialog.show();
	}

	@Override
	public void setResult(int year, int month, int day)
	{
		move.Date.set(year, month, day);
		form.setValue(R.id.date, move.DateString());
	}

	@Override
	public DatePickerDialog getDialog()
	{
		return dialog;
	}



	public void changeCategory(View view) throws JSONException
	{
		form.showChangeList(categoryList, R.string.category, new DialogCategory(categoryList, form, message, move));
	}

	public void changeNature(View view) throws JSONException
	{
		form.showChangeList(natureList, R.string.nature, new DialogNature(natureList, this));
	}


	public void setNature(String text, String value)
	{
		form.setValue(R.id.nature, text);
		move.SetNature(value);

		int accountOutVisibility = move.Nature != Nature.In ? View.VISIBLE : View.GONE;
		findViewById(R.id.account_out_block).setVisibility(accountOutVisibility);

		int accountInVisibility = move.Nature != Nature.Out ? View.VISIBLE : View.GONE;
		findViewById(R.id.account_in_block).setVisibility(accountInVisibility);

		if (move.Nature == Nature.Out)
		{
			move.AccountIn = null;
			form.setValue(R.id.account_in, getString(R.string.pick));
		}

		if (move.Nature == Nature.In)
		{
			move.AccountOut = null;
			form.setValue(R.id.account_out, getString(R.string.pick));
		}
	}



	public void changeAccountOut(View view) throws JSONException
	{
		form.showChangeList(accountList, R.string.account, new DialogAccountOut(accountList, form, message, move));
	}

	public void changeAccountIn(View view) throws JSONException
	{
		form.showChangeList(accountList, R.string.account, new DialogAccountIn(accountList, form, message, move));
	}



	public void useDetailed() {
		useDetailed(null);
	}

	public void useDetailed(View view)
	{
		move.isDetailed = true;

		findViewById(R.id.simple_value).setVisibility(View.GONE);
		findViewById(R.id.detailed_value).setVisibility(View.VISIBLE);

		scrollToTheEnd();
	}

	public void useSimple(View view)
	{
		move.isDetailed = false;

		findViewById(R.id.simple_value).setVisibility(View.VISIBLE);
		findViewById(R.id.detailed_value).setVisibility(View.GONE);

		scrollToTheEnd();
	}

	public void addDetail(View view)
	{
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

		addViewDetail(move, description, amount, value);

		scrollToTheEnd();
	}

	private void addViewDetail(Move move, String description, int amount, double value)
	{
		DetailBox row = new DetailBox(this, move, description, amount, value);
		LinearLayout list = (LinearLayout) findViewById(R.id.details);
		list.addView(row);
	}


	public void save(View view)
	{
		request = new InternalRequest(this, "Moves/Create");

		request.AddParameter("ticket", Authentication.Get());
		move.setParameters(request);

		request.Post(Step.Recording);
	}

	private void backToExtract()
	{
		Intent intent = new Intent(this, ExtractActivity.class);
		intent.putExtra("accountUrl", getIntent().getStringExtra("accountUrl"));
		intent.putExtra("month", move.getMonth());
		intent.putExtra("year", move.getYear());

		startActivity(intent);
	}



	private void scrollToTheEnd()
	{
		window.postDelayed(new Runnable() {
			@Override
			public void run() {
				window.fullScroll(ScrollView.FOCUS_DOWN);
			}
		}, 100);
	}


}

