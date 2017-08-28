package com.dontflymoney.view;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.entities.Nature;
import com.dontflymoney.entities.Detail;
import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.AfterTextWatcher;
import com.dontflymoney.viewhelper.DetailBox;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Calendar;

public class MovesCreateActivity extends SmartActivity {
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
                for (int c = 0; c < categoryList.length(); c++)
                {
                    JSONObject category = categoryList.getJSONObject(c);
                    String value = category.getString("Value");

                    if (value.equals(move.Category))
                    {
                        String text = category.getString("Text");
                        form.setValue(R.id.category, text);
                        break;
                    }
                }
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

            for(int n = 0; n < accountList.length(); n++)
            {
                JSONObject account = accountList.getJSONObject(n);
                String value = account.getString("Value");

                if (value.equals(move.AccountOut))
                {
                    String text = account.getString("Text");
                    form.setValue(R.id.account_out, text);
                    break;
                }
            }

            for(int n = 0; n < accountList.length(); n++)
            {
                JSONObject account = accountList.getJSONObject(n);
                String value = account.getString("Value");

                if (value.equals(move.AccountIn))
                {
                    String text = account.getString("Text");
                    form.setValue(R.id.account_in, text);
                    break;
                }
            }
			
			
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

            EditText valueView = (EditText) findViewById(R.id.value);
            valueView.setText(Double.toString(move.Value));

		}
	}

	private void populateScreen()
	{
		request = new Request(this, "Moves/Create");
        request.AddParameter("ticket", Authentication.Get());
        request.AddParameter("accounturl", getIntent().getStringExtra("accounturl"));
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

        if (data.has("Move") && !data.get("Move").equals(null))
        {
            JSONObject moveToEdit = data.getJSONObject("Move");
            move.SetData(moveToEdit);
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

	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
		@Override
		public void onDateSet(DatePicker view, int year, int month, int day)
		{
            if (view.isShown())
            {
                move.Date.set(year, month, day);
                form.setValue(R.id.date, move.DateString());
                dialog.dismiss();
            }
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
			setNature(text, value);
		}

		@Override
		public void handleError(JSONException exception)
		{
			message.alertError(R.string.error_convert_result);
		}
	}

	public void setNature(String text, String value)
	{
		form.setValue(R.id.nature, text);
		move.SetNature(value);
		
		int accountOutVisibility = move.Nature != Nature.In ? View.VISIBLE : View.GONE;
        findViewById(R.id.account_out_block).setVisibility(accountOutVisibility);

        int accountInVisibility = move.Nature != Nature.Out ? View.VISIBLE : View.GONE;
        findViewById(R.id.account_in_block).setVisibility(accountInVisibility);
	}



    public void changeAccountOut(View view) throws JSONException
    {
        form.showChangeList(accountList, R.string.account, new DialogAccountOut(accountList));
    }

    public void changeAccountIn(View view) throws JSONException
    {
        form.showChangeList(accountList, R.string.account, new DialogAccountIn(accountList));
    }

    class DialogAccountOut extends DialogSelectClickListener
    {
        public DialogAccountOut(JSONArray list) { super(list); }

        @Override
        public void setResult(String text, String value)
        {
            form.setValue(R.id.account_out, text);
            move.AccountOut = value;
        }

        @Override
        public void handleError(JSONException exception)
        {
            message.alertError(R.string.error_convert_result);
        }
    }

    class DialogAccountIn extends DialogSelectClickListener
    {
        public DialogAccountIn(JSONArray list) { super(list); }

        @Override
        public void setResult(String text, String value)
        {
            form.setValue(R.id.account_in, text);
            move.AccountIn = value;
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
		request = new Request(this, "Moves/Create");
		
		request.AddParameter("ticket", Authentication.Get());
		move.setParameters(request);
		
		request.Post(Step.Recording);
	}
	
	private void backToExtract()
	{
		Intent intent = new Intent(this, ExtractActivity.class);
		intent.putExtra("accounturl", getIntent().getStringExtra("accounturl"));
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
