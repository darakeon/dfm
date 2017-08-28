package com.dontflymoney.entities;

import com.dontflymoney.api.InternalRequest;
import com.dontflymoney.viewhelper.DateTime;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

public class Move
{
	public Move()
	{
		Details = new ArrayList<>();
		Date = Calendar.getInstance();
	}

	private int Id;
	public String Description;
	public Calendar Date;

	public Nature Nature;

	public String Category;
	public String AccountOut;
	public String AccountIn;

	public boolean isDetailed;
	public double Value;
	public List<Detail> Details;


	private Nature getNature(int number)
	{
		return com.dontflymoney.entities.Nature.GetNature(number);
	}


	public void SetNature(String number)
	{
		SetNature(Integer.parseInt(number));
	}

	public void SetNature(int number)
	{
		Nature = getNature(number);
	}

	public void Add(String description, int amount, double value)
	{
		Detail detail = new Detail();

		detail.Description = description;
		detail.Amount = amount;
		detail.Value = value;

		Details.add(detail);
	}

	public void Remove(String description, int amount, double value)
	{
		for (Detail detail : Details)
		{
			if (detail.Equals(description, amount, value))
			{
				Details.remove(detail);
				return;
			}
		}
	}



	public int getDay()
	{
		return Date.get(Calendar.DAY_OF_MONTH);
	}

	public int getMonth()
	{
		return Date.get(Calendar.MONTH);
	}

	public int getYear()
	{
		return Date.get(Calendar.YEAR);
	}

	public void setParameters(InternalRequest request)
	{
		request.AddParameter("ID", Id);
		request.AddParameter("Description", Description);

		request.AddParameter("Date.Year", Date.get(Calendar.YEAR));
		request.AddParameter("Date.Month", Date.get(Calendar.MONTH) + 1);
		request.AddParameter("Date.Day", Date.get(Calendar.DAY_OF_MONTH));

		request.AddParameter("Nature", Nature);

		request.AddParameter("Category", Category);
		request.AddParameter("AccountOutUrl", AccountOut);
		request.AddParameter("AccountInUrl", AccountIn);

		if (isDetailed)
		{
			for (Detail detail : Details)
			{
				int position = Details.lastIndexOf(detail);

				request.AddParameter("DetailList[" + position + "].Description", detail.Description);
				request.AddParameter("DetailList[" + position + "].Amount", detail.Amount);
				request.AddParameter("DetailList[" + position + "].Value", detail.Value);
			}
		}
		else
		{
			request.AddParameter("Value", Value);
		}
	}

	public String DateString() {
		SimpleDateFormat formatter = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());

		return formatter.format(Date.getTime());
	}



	public void SetData(JSONObject move, String activityAccountUrl) throws JSONException
	{
		Id = move.getInt("ID");

		if (Id == 0)
		{
			AccountOut = activityAccountUrl;
		}
		else
		{
			setEditMoveData(move);
		}
	}

	private void setEditMoveData(JSONObject move) throws JSONException
	{
		Description = move.getString("Description");
		Date = DateTime.getCalendar(move.getJSONObject("Date"));
		Category = move.getString("Category");
		AccountOut = move.getString("AccountOutUrl");
		AccountIn = move.getString("AccountInUrl");
		Nature = getNature(move.getInt("Nature"));

		if (move.has("Value") && !move.isNull("Value"))
		{
			Value = move.getDouble("Value");
		}

		if (move.has("DetailList"))
		{
			JSONArray detailList = move.getJSONArray("DetailList");

			isDetailed = detailList.length() > 0;

			for (int d = 0; d < detailList.length(); d++)
			{
				JSONObject detail = detailList.getJSONObject(d);
				Add(detail.getString("Description"), detail.getInt("Amount"), detail.getDouble("Value"));
			}
		}
	}


}
