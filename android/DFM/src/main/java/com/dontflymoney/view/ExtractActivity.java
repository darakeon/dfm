package com.dontflymoney.view;

import android.annotation.SuppressLint;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.ContextMenu;
import android.view.MenuItem;
import android.view.View;
import android.widget.DatePicker;
import android.widget.ListView;
import android.widget.TextView;

import com.dontflymoney.adapters.MoveAdapter;
import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.IYesNoDialogAnswer;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.layout.MoveLine;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Field;
import java.text.SimpleDateFormat;
import java.util.Calendar;

public class ExtractActivity extends SmartActivity implements IYesNoDialogAnswer
{
	ListView main;
	TextView empty;

	String accountUrl;

	static JSONArray moveList;
	static String name;
	static double total;
	static boolean canCheck;

	DatePickerDialog dialog;
	private static int month;
	private static int year;



	protected int contentView() { return R.layout.extract; }
	protected int optionsMenuResource() { return R.menu.extract; }
	protected int contextMenuResource() { return R.menu.move_options; }
	protected int viewWithContext(){ return R.id.main_table; }



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
		main = (ListView)findViewById(R.id.main_table);
		empty = (TextView)findViewById(R.id.empty_list);

		accountUrl = getIntent().getStringExtra("accountUrl");
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
			catch (Exception ignored) { }
		}

		dialog.show();
	}

	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
		@Override
		public void onDateSet(DatePicker view, int year, int month, int day)
		{
			if (view.isShown())
			{
				setDate(month, year);
				getExtract();
				dialog.dismiss();
			}
		}

	}

	private void getExtract()
	{
		request = new Request(this, "Moves/Extract");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accountUrl", accountUrl);
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
				canCheck = data.getBoolean("CanCheck");

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
	
	private void fillMoves() throws JSONException
	{
		form.setValue(R.id.totalTitle, name);
		form.setValueColored(R.id.totalValue, total);

		if (moveList.length() == 0)
		{
			main.setVisibility(View.GONE);
			empty.setVisibility(View.VISIBLE);
		}
		else
		{
			main.setVisibility(View.VISIBLE);
			empty.setVisibility(View.GONE);

			MoveAdapter accountAdapter = new MoveAdapter(this, moveList, canCheck);
			main.setAdapter(accountAdapter);
		}
	}


	public void goToSummary(MenuItem item)
	{
		Intent intent = new Intent(this, SummaryActivity.class);
		
		intent.putExtra("accountUrl", accountUrl);
		intent.putExtra("year", year);
		
		startActivity(intent);
	}

	public void goToMove(MenuItem item)
	{
		goToMove(0);
	}

	private void goToMove(int moveId)
	{
		Intent intent = new Intent(this, MovesCreateActivity.class);

		intent.putExtra("id", moveId);
		intent.putExtra("accountUrl", accountUrl);
		intent.putExtra("year", year);
		intent.putExtra("month", month);

		startActivity(intent);
	}



	@Override
	public boolean onContextItemSelected(MenuItem item)
	{
		switch (item.getItemId()) {
			case R.id.edit_move:
				edit();
				return true;
			case R.id.delete_move:
				askDelete();
				return true;
			case R.id.check_move:
				check();
				return true;
			case R.id.uncheck_move:
				uncheck();
				return true;
			default:
				return super.onContextItemSelected(item);
		}
	}

	private void edit()
	{
		MoveLine moveLine = (MoveLine)clickedView;
		goToMove(moveLine.getId());
	}



	public boolean askDelete()
	{
		String messageText = getString(R.string.sure_to_delete);
		MoveLine moveRow = (MoveLine) clickedView;

		messageText = String.format(messageText, moveRow.getDescription());

		message.alertYesNo(messageText, this);

		return false;
	}

	@Override
	public void YesAction() {
		submitMoveAction("Delete");
	}

	@Override
	public void NoAction() { }



	private void check()
	{
		submitMoveAction("Check");
	}

	private void uncheck()
	{
		submitMoveAction("Uncheck");
	}



	private void submitMoveAction(String action)
	{
		request = new Request(this, "Moves/" + action);

		MoveLine view = (MoveLine) clickedView;

		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accountUrl", accountUrl);
		request.AddParameter("id", view.getId());

		request.Post(Step.Recording);
	}


	@Override
	public void changeContextMenu(View view, ContextMenu menu)
	{
		MoveLine move = (MoveLine) clickedView;

		try
		{
			if (canCheck)
			{
				hideMenuItem(menu, move.isChecked() ? R.id.check_move : R.id.uncheck_move);
				showMenuItem(menu, move.isChecked() ? R.id.uncheck_move : R.id.check_move);
			}
			else
			{
				hideMenuItem(menu, R.id.check_move);
				hideMenuItem(menu, R.id.uncheck_move);
			}
		}
		catch (Exception e)
		{
			e.printStackTrace();
		}

	}

	public void hideMenuItem(ContextMenu menu, int id)
	{
		toggleMenuItem(menu, id, false);
	}

	public void showMenuItem(ContextMenu menu, int id)
	{
		toggleMenuItem(menu, id, true);
	}

	public void toggleMenuItem(ContextMenu menu, int id, boolean show)
	{
		MenuItem buttonToHide = menu.findItem(id);
		buttonToHide.setVisible(show);
	}

}

