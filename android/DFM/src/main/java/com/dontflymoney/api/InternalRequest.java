package com.dontflymoney.api;

import android.app.ProgressDialog;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;
import android.view.Surface;
import android.view.WindowManager;

import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class InternalRequest
{
	public SmartActivity activity;
	private String url;
	private HashMap<String, Object> parameters;

	private JsonObjectRequest jsonRequest;

	private ProgressDialog progress;



	public InternalRequest(SmartActivity activity, String url)
	{
		this.activity = activity;
		this.url = url;
		this.parameters = new HashMap<>();
	}


	public void AddParameter(String key, Object value)
	{
		parameters.put(key, value);
	}



	public void Post()
	{
		Post(Step.NoSteps);
	}

	public boolean Post(final Step step)
	{
		makeRequest(step, Request.Method.POST);
		return true;
	}

	public void Get(final Step step)
	{
		makeRequest(step, Request.Method.GET);
	}

	private void makeRequest(final Step step, int method)
	{
		if (Internet.isOffline(activity))
		{
			String error = activity.getString(R.string.u_r_offline);
			activity.HandlePostError(error, step);
			return;
		}

		JSONObject parameters = getParameters(step);

		if (parameters == null)
			return;

		String completeUrl = getUrl();

		jsonRequest = new JsonObjectRequest(method, completeUrl, parameters, new Response.Listener<JSONObject>()
		{
			@Override
			public void onResponse(JSONObject response)
			{
				handleResponse(response, step);
			}
		}, new Response.ErrorListener()
		{
			@Override
			public void onErrorResponse(VolleyError error)
			{
				handleError(error, step);
			}
		});

		Volley.newRequestQueue(activity).add(jsonRequest);

		startUIWait();
	}



	private JSONObject getParameters(Step step)
	{
		JSONObject jsonParameters = new JSONObject();

		for (Map.Entry<String, Object> parameter : parameters.entrySet())
		{
			Object rawValue = parameter.getValue();

			if (rawValue != null)
			{
				String key = parameter.getKey();
				String value = rawValue.toString();

				try
				{
					jsonParameters.put(key, value);
				}
				catch (JSONException e)
				{
					handleError(e, step);
					return null;
				}
			}
		}

		return jsonParameters;
	}

	private String getUrl()
	{
		String completeUrl = Site.GetProtocol() + "://" + Site.Domain + "/Api";

		if (parameters.containsKey("ticket"))
		{
			completeUrl += "-" + parameters.get("ticket");
			parameters.remove("ticket");
		}

		if (parameters.containsKey("accountUrl"))
		{
			completeUrl += "/Account-" + parameters.get("accountUrl");
			parameters.remove("accountUrl");
		}

		completeUrl += "/" + url;

		if (parameters.containsKey("id"))
		{
			completeUrl += "/" + parameters.get("id");
			parameters.remove("id");
		}

		return completeUrl;
	}



	private void handleResponse(JSONObject response, Step step)
	{
		InternalResponse internalResponse = new InternalResponse(response);

		endUIWait();

		if (step == Step.Logout)
			return;

		handleResponse(internalResponse, step);
	}

	private void handleError(JSONException e, Step step)
	{
		InternalResponse response = new InternalResponse(
				activity.getString(R.string.error_convert_result)
						+ ": [json] " + e.getMessage()
		);

		handleResponse(response, step);
	}

	private void handleError(Exception e, Step step)
	{
		InternalResponse response = new InternalResponse(
				activity.getString(R.string.error_contact_url)
						+ ": " + this.url
						+ "\r\n " + e.getMessage()
		);

		handleResponse(response, step);
	}

	private void handleResponse(InternalResponse internalResponse, Step step)
	{
		if (internalResponse.IsSuccess())
			activity.HandlePostResult(internalResponse.GetResult(), step);
		else
			activity.HandlePostError(internalResponse.GetError(), step);
	}



	public void Cancel()
	{
		endUIWait();
		jsonRequest.cancel();
	}




	private void startUIWait()
	{
		openProgressBar();
		disableSleep();
		disableRotation();
	}

	private void openProgressBar()
	{
		progress = activity.getMessage().showWaitDialog();
	}

	private void disableSleep()
	{
		activity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
	}

	private void disableRotation()
	{
		int rotation = activity.getWindowManager().getDefaultDisplay().getRotation();
		int orientation = activity.getResources().getConfiguration().orientation;

		switch (orientation)
		{
			case Configuration.ORIENTATION_PORTRAIT:
				handlePortrait(rotation);
				break;

			case Configuration.ORIENTATION_LANDSCAPE:
				handleLandscape(rotation);
				break;
		}
	}

	private void handlePortrait(int rotation)
	{
		switch (rotation)
		{
			case Surface.ROTATION_0:
			case Surface.ROTATION_270:
				activity.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
				break;
			default:
				activity.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT);
				break;
		}
	}

	private void handleLandscape(int rotation)
	{
		switch (rotation)
		{
			case Surface.ROTATION_0:
			case Surface.ROTATION_90:
				activity.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
				break;
			default:
				activity.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE);
				break;
		}
	}


	private void endUIWait()
	{
		closeProgressBar();
		enableSleep();
		enableRotation();
	}

	private void closeProgressBar()
	{
		if (progress == null)
			return;

		// This try is a fix for user turn the screen
		// it recharges the activity and fucks the dialog
		try
		{
			progress.dismiss();
			progress = null;
		} catch (IllegalArgumentException ignored) { }
	}

	private void enableSleep()
	{
		activity.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
	}

	private void enableRotation()
	{
		activity.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_SENSOR);
	}


}