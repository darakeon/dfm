package com.dontflymoney.api

import android.app.ProgressDialog
import android.content.pm.ActivityInfo
import android.content.res.Configuration
import android.view.Surface
import android.view.WindowManager

import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.VolleyError
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.view.R

import org.json.JSONException
import org.json.JSONObject

import java.util.HashMap

class InternalRequest(var activity: SmartActivity, private val url: String) {
    private val parameters: HashMap<String, Any>

    private var jsonRequest: JsonObjectRequest? = null

    private var progress: ProgressDialog? = null


    init {
        this.parameters = HashMap<String, Any>()
    }


    fun AddParameter(key: String, value: Any) {
        parameters.put(key, value)
    }


    fun Post() {
        Post(Step.NoSteps)
    }

    fun Post(step: Step): Boolean {
        makeRequest(step, Request.Method.POST)
        return true
    }

    fun Get(step: Step) {
        makeRequest(step, Request.Method.GET)
    }

    private fun makeRequest(step: Step, method: Int) {
        if (Internet.isOffline(activity)) {
            val error = activity.getString(R.string.u_r_offline)
            activity.HandlePostError(error, step)
            return
        }

        val parameters = getParameters(step) ?: return

        val completeUrl = getUrl()

        jsonRequest = JsonObjectRequest(method, completeUrl, parameters, Response.Listener<JSONObject> { response -> handleResponse(response, step) }, Response.ErrorListener { error -> handleError(error, step) })

        Volley.newRequestQueue(activity).add(jsonRequest!!)

        startUIWait()
    }


    private fun getParameters(step: Step): JSONObject? {
        val jsonParameters = JSONObject()

        for ((key, rawValue) in parameters) {

            if (rawValue != null) {
                val value = rawValue.toString()

                try {
                    jsonParameters.put(key, value)
                } catch (e: JSONException) {
                    handleError(e, step)
                    return null
                }

            }
        }

        return jsonParameters
    }

    private fun getUrl(): String {
        var completeUrl = Site.GetProtocol() + "://" + Site.Domain + "/Api"

        if (parameters.containsKey("ticket")) {
            completeUrl += "-" + parameters["ticket"]
            parameters.remove("ticket")
        }

        if (parameters.containsKey("accountUrl")) {
            completeUrl += "/Account-" + parameters["accountUrl"]
            parameters.remove("accountUrl")
        }

        completeUrl += "/" + url

        if (parameters.containsKey("id")) {
            completeUrl += "/" + parameters["id"]
            parameters.remove("id")
        }

        return completeUrl
    }


    private fun handleResponse(response: JSONObject, step: Step) {
        val internalResponse = InternalResponse(response)

        endUIWait()

        if (step == Step.Logout)
            return

        handleResponse(internalResponse, step)
    }

    private fun handleError(e: JSONException, step: Step) {
        val response = InternalResponse(
                activity.getString(R.string.error_convert_result)
                        + ": [json] " + e.message
        )

        handleResponse(response, step)
    }

    private fun handleError(e: Exception, step: Step) {
        val response = InternalResponse(
                activity.getString(R.string.error_contact_url)
                        + ": " + this.url
                        + "\r\n " + e.message
        )

        handleResponse(response, step)
    }

    private fun handleResponse(internalResponse: InternalResponse, step: Step) {
        if (internalResponse.IsSuccess())
            activity.HandlePostResult(internalResponse.GetResult(), step)
        else
            activity.HandlePostError(internalResponse.GetError(), step)
    }


    fun Cancel() {
        endUIWait()
        jsonRequest!!.cancel()
    }


    private fun startUIWait() {
        openProgressBar()
        disableSleep()
        disableRotation()
    }

    private fun openProgressBar() {
        progress = activity.message.showWaitDialog()
    }

    private fun disableSleep() {
        activity.window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
    }

    private fun disableRotation() {
        val rotation = activity.windowManager.defaultDisplay.rotation
        val orientation = activity.resources.configuration.orientation

        when (orientation) {
            Configuration.ORIENTATION_PORTRAIT -> handlePortrait(rotation)

            Configuration.ORIENTATION_LANDSCAPE -> handleLandscape(rotation)
        }
    }

    private fun handlePortrait(rotation: Int) {
        when (rotation) {
            Surface.ROTATION_0, Surface.ROTATION_270 -> activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
            else -> activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
        }
    }

    private fun handleLandscape(rotation: Int) {
        when (rotation) {
            Surface.ROTATION_0, Surface.ROTATION_90 -> activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
            else -> activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
        }
    }


    private fun endUIWait() {
        closeProgressBar()
        enableSleep()
        enableRotation()
    }

    private fun closeProgressBar() {
        if (progress == null)
            return

        // This try is a fix for user turn the screen
        // it recharges the activity and fucks the dialog
        try {
            progress!!.dismiss()
            progress = null
        } catch (ignored: IllegalArgumentException) {
        }

    }

    private fun enableSleep() {
        activity.window.clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
    }

    private fun enableRotation() {
        activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_SENSOR
    }


}