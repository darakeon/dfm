package com.dontflymoney.adapters;

import android.annotation.SuppressLint;
import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;

import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.layout.MoveLine;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DateTime;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

public class MoveAdapter extends BaseAdapter
{
	private static LayoutInflater inflater = null;
	private SmartActivity activity;
	private final boolean canCheck;
	private List<Move> moveList;

	public MoveAdapter(SmartActivity activity, JSONArray moveJsonList, boolean canCheck) throws JSONException
	{
		this.activity = activity;
		this.canCheck = canCheck;

		moveList = new ArrayList<>();

		for (int a = 0; a < moveJsonList.length(); a++)
		{
			Move move = new Move(moveJsonList.getJSONObject(a));
			moveList.add(move);
		}

		inflater = (LayoutInflater) activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}

	public class Move
	{
		public String Description;
		public Calendar Date;
		public double Total;
		public Boolean Checked;
		public int ID;

		Move(JSONObject jsonObject) throws JSONException
		{
			Description = jsonObject.getString("Description");
			Date = DateTime.getCalendar(jsonObject.getJSONObject("Date"));
			Total = jsonObject.getDouble("Total");
			Checked = jsonObject.getBoolean("Checked");
			ID = jsonObject.getInt("ID");
		}
	}



	@Override
	public int getCount() { return moveList.size(); }

	@Override
	public Object getItem(int position) { return position; }

	@Override
	public long getItemId(int position) { return position; }

	@Override
	public View getView(int position, View view, ViewGroup viewGroup)
	{
		@SuppressLint({"ViewHolder", "InflateParams"})
		MoveLine line = (MoveLine)inflater.inflate(R.layout.extract_line, null);

		try
		{
			int color = position % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
			line.setMove(activity, moveList.get(position), color, canCheck);
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}

		return line;
	}
}
