package com.dontflymoney.view;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.widget.DatePicker;
import android.widget.TableLayout;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.IYesNoDialogAnswer;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.viewhelper.MoveRow;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Field;
import java.text.SimpleDateFormat;
import java.util.Calendar;

public class ExtractActivity extends SmartActivity implements IYesNoDialogAnswer
{
	static JSONArray moveList;
	static String name;
	static double total;
	
	TableLayout table;
	String accounturl;

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
		table = (TableLayout)findViewById(R.id.main_table);
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
		table.removeAllViews();
		
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
			table.addView(empty);
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

    private void getMove(final JSONObject move, int color)
            throws JSONException
    {
        MoveRow row = new MoveRow(this);

        row.setBackgroundColor(color);

        row.setDescription(move.getString("Description"));
        row.setDate(move.getJSONObject("Date"));
        row.setTotal(move.getDouble("Total"));

        row.setChecked(move.getBoolean("Checked"));

        row.ID = move.getInt("ID");

        row.setClickable(true);

        final Activity thisParent = this;

        row.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                try
                {
                    int idButtonToHide = move.getBoolean("Checked") ? R.id.check_move : R.id.uncheck_move;
                    View buttonToHide = thisParent.findViewById(idButtonToHide);
                    buttonToHide.setVisibility(View.GONE);

                    int idButtonToShow = move.getBoolean("Checked") ? R.id.uncheck_move : R.id.check_move;
                    View buttonToShow = thisParent.findViewById(idButtonToShow);
                    buttonToShow.setVisibility(View.VISIBLE);
                }
                catch (JSONException e)
                {
                    e.printStackTrace();
                }
            }
        });

        table.addView(row);
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
        goToMove(0);
	}

    private void goToMove(int moveId)
    {
        Intent intent = new Intent(this, MovesCreateActivity.class);

        intent.putExtra("id", moveId);
        intent.putExtra("accounturl", accounturl);
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
        MoveRow moveRow = (MoveRow)clickedView;
        goToMove(moveRow.ID);
    }



    public boolean askDelete()
    {
        String messageText = getString(R.string.sure_to_delete);
        MoveRow moveRow = (MoveRow)clickedView;

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

        MoveRow view = (MoveRow)clickedView;

        request.AddParameter("ticket", Authentication.Get());
        request.AddParameter("accounturl", accounturl);
        request.AddParameter("id", view.ID);

        request.Post(Step.Recording);
    }

}

