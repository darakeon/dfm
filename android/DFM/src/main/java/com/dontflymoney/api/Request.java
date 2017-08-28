package com.dontflymoney.api;

import android.app.ProgressDialog;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;
import android.view.Surface;
import android.view.WindowManager;

import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.stati.Internet;
import com.dontflymoney.view.R;

import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Request
{
    private String site;

    public SmartActivity activity;
    private String url;
    private HashMap<String, Object> parameters;

    private ProgressDialog progress;
    private SiteConnector connector;


    public Request(SmartActivity activity, String url)
    {
        this.activity = activity;
        this.url = url;
        this.parameters = new HashMap<String, Object>();

        setMainUrl();
    }

    private void setMainUrl()
    {
        site = Site.GetProtocol() + "://" + Site.Domain + "/Api";
    }


    public void AddParameter(String key, Object value)
    {
        parameters.put(key, value);
    }


    public void Post()
    {
        Post(Step.NoSteps);
    }

    public boolean Post(Step step)
    {
        if (isOffline())
        {
            String error = activity.getString(R.string.u_r_offline);
            activity.HandlePostError(error, step);
            return false;
        }

        String completeUrl = getUrl();
        HttpPost post = new HttpPost(completeUrl);
        List<NameValuePair> nameValuePairs = getParameters();

        try
        {
            post.setEntity(new UrlEncodedFormEntity(nameValuePairs, "UTF-8"));
        } catch (UnsupportedEncodingException e)
        {
            String error = activity.getString(R.string.error_set_parameters) + e.getMessage();
            activity.HandlePostError(error, step);
            return false;
        }

        connector = new SiteConnector(post, this, step);
        connector.execute();

        startUIWait();

        return true;
    }


    public void Get()
    {
        Get(Step.NoSteps);
    }

    public void Get(Step step)
    {
        if (isOffline())
        {
            String error = activity.getString(R.string.u_r_offline);
            activity.HandlePostError(error, step);
            return;
        }

        String completeUrl = getUrl();
        HttpGet get = new HttpGet(completeUrl);

        connector = new SiteConnector(get, this, step);

        connector.execute();

        startUIWait();
    }


    private boolean isOffline()
    {
        return Internet.isOffline(activity);
    }

    private String getUrl()
    {
        String completeUrl = site;

        if (parameters.containsKey("ticket"))
        {
            completeUrl += "-" + parameters.get("ticket");
            parameters.remove("ticket");
        }

        if (parameters.containsKey("accounturl"))
        {
            completeUrl += "/Account-" + parameters.get("accounturl");
            parameters.remove("accounturl");
        }

        completeUrl += "/" + url;

        if (parameters.containsKey("id"))
        {
            completeUrl += "/" + parameters.get("id");
            parameters.remove("id");
        }

        return completeUrl;
    }

    private List<NameValuePair> getParameters()
    {
        List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>();

        for (Map.Entry<String, Object> parameter : parameters.entrySet())
        {
            Object rawValue = parameter.getValue();

            if (rawValue != null)
            {
                String key = parameter.getKey();
                String value = rawValue.toString();

                BasicNameValuePair pair = new BasicNameValuePair(key, value);

                nameValuePairs.add(pair);
            }
        }

        return nameValuePairs;
    }


    void HandleResponse(String json, String errorMessage, Step step)
    {
        Response response;

        if (errorMessage != null)
        {
            response = new Response(errorMessage);
        }
        else
        {
            while (json.startsWith("\n") || json.startsWith("\r"))
            {
                json = json.substring(1);
            }

            if (json.startsWith("<"))
            {
                response = new Response(activity.getString(R.string.error_contact_url) + " " + this.url);
            } else
            {
                try
                {
                    response = new Response(new JSONObject(json));
                } catch (JSONException e)
                {
                    response = new Response(activity.getString(R.string.error_convert_result) + ": [json] " + e.getMessage());
                }
            }
        }

        endUIWait();

        if (step == Step.Logout)
            return;

        if (response.IsSuccess())
            activity.HandlePostResult(response.GetResult(), step);
        else
            activity.HandlePostError(response.GetError(), step);
    }


    public void Cancel()
    {
        endUIWait();
        connector.cancel(true);
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