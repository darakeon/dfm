package com.darakeon.dfm.api

import android.app.Activity
import android.app.Dialog
import android.content.pm.ActivityInfo
import android.content.res.Configuration
import android.view.Surface
import android.view.WindowManager
import com.android.volley.DefaultRetryPolicy
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.alertError
import com.darakeon.dfm.activities.base.logout
import com.darakeon.dfm.activities.base.createWaitDialog
import com.darakeon.dfm.activities.objects.SmartStatic
import com.darakeon.dfm.user.languageChangeAndSave
import com.darakeon.dfm.user.themeChangeAndSave
import org.json.JSONObject
import java.util.*

class InternalRequest<T : SmartStatic>(
	private var activity: SmartActivity<T>,
	private val url: String,
	private var handleSuccess: ((JSONObject) -> Unit)?
) {

	constructor(activity: SmartActivity<T>, url: String)
		: this(activity, url, null)

	private val parameters: HashMap<String, Any?> = HashMap()

	private var jsonRequest: JsonObjectRequest? = null

	private var progress: Dialog? = null


	fun addParameter(key: String, value: Any?) {
		parameters.put(key, value)
	}


	fun post(): Boolean {
		makeRequest(Request.Method.POST)
		return true
	}

	fun get() {
		makeRequest(Request.Method.GET)
	}

	private fun makeRequest(method: Int) {
		activity.request = this

		if (Internet.isOffline(activity)) {
			val error = activity.getString(R.string.u_r_offline)
			activity.alertError(error)
			activity.succeeded = false
			return
		}

		val parameters = getParameters() ?: return

		val completeUrl = getUrl()

		val jsonRequest = JsonObjectRequest(method, completeUrl, parameters,
			Response.Listener<JSONObject> { response -> handleResponse(response) },
			Response.ErrorListener { handleError() })

		jsonRequest.retryPolicy = getDefaultRetryPolicy()
		Volley.newRequestQueue(activity).add(jsonRequest)

		this.jsonRequest = jsonRequest

		activity.startUIWait()
	}

	private fun getParameters(): JSONObject? {
		val jsonParameters = JSONObject()

		for ((key, rawValue) in parameters) {
			if (rawValue != null) {
				val value = rawValue.toString()
				jsonParameters.put(key, value)
			}
		}

		return jsonParameters
	}

	private fun getDefaultRetryPolicy(): DefaultRetryPolicy {
		val timeoutMilliseconds = 30 * 1000
		val maxRetries = 0
		val backoffMulti = DefaultRetryPolicy.DEFAULT_BACKOFF_MULT

		return DefaultRetryPolicy(timeoutMilliseconds, maxRetries, backoffMulti)
	}

	private fun getUrl(): String {
		var completeUrl = getSite("Api")

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

	private fun getSite(path: String) : String {
		val publicDomain = "dontflymoney.com"

		val domain =
			if (activity.isProd)
				publicDomain
			else
				activity.getString(R.string.local_address)

		val protocol = if (activity.isProd) "https" else "http"

		return "$protocol://$domain/$path"
	}

	private fun handleResponse(response: JSONObject) {
		val internalResponse = InternalResponse(response)

		if (handleSuccess == null) {
			activity.endUIWait()
			return
		}

		assemblyResponse(internalResponse)
	}

	private fun handleError() {
		val response = InternalResponse(
			activity.getString(R.string.error_contact_url)
		)

		assemblyResponse(response)
	}

	private fun assemblyResponse(internalResponse: InternalResponse) {
		activity.endUIWait()

		activity.succeeded = internalResponse.isSuccess()

		if (!activity.succeeded) {
			activity.alertError(internalResponse.getError())
			return
		}

		val result = internalResponse.getResult()

		if (result.has("error")) {
			val error = result.getString("error")

			activity.alertError(error)

			if (error.contains("uninvited")) {
				activity.logout()
			}
		} else {
			val data = result.getJSONObject("data")

			if (data.has("Language"))
				activity.languageChangeAndSave(data.getString("Language"))

			if (data.has("Theme"))
				activity.themeChangeAndSave(data.getString("Theme"))

			handleSuccess?.invoke(data)
		}

	}


	fun cancel() {
		activity.endUIWait()
		jsonRequest?.cancel()
	}


	private fun Activity.startUIWait() {
		openProgressBar()
		disableSleep()
		disableRotation()
	}

	private fun Activity.openProgressBar() {
		progress = createWaitDialog()
	}

	private fun Activity.disableSleep() {
		window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
	}

	private fun Activity.disableRotation() {
		val rotation = windowManager.defaultDisplay.rotation
		val orientation = resources.configuration.orientation

		requestedOrientation = when (orientation) {
			Configuration.ORIENTATION_PORTRAIT -> handlePortrait(rotation)
			Configuration.ORIENTATION_LANDSCAPE -> handleLandscape(rotation)
			else -> 0
		}
	}

	private fun handlePortrait(rotation: Int): Int =
		when (rotation) {
			Surface.ROTATION_0, Surface.ROTATION_270 ->
				ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
			else ->
				ActivityInfo.SCREEN_ORIENTATION_REVERSE_PORTRAIT
		}

	private fun handleLandscape(rotation: Int): Int =
		when (rotation) {
			Surface.ROTATION_0, Surface.ROTATION_90 ->
				ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
			else ->
				ActivityInfo.SCREEN_ORIENTATION_REVERSE_LANDSCAPE
		}

	private fun Activity.endUIWait() {
		closeProgressBar()
		enableSleep()
		enableRotation()
	}

	private fun closeProgressBar() {
		progress?.dismiss()
		progress = null
	}

	private fun Activity.enableSleep() {
		window.clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
	}

	private fun Activity.enableRotation() {
		requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_SENSOR
	}

}