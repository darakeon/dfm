package com.darakeon.dfm.api

import android.app.Activity
import android.app.Dialog
import android.content.DialogInterface
import android.content.pm.ActivityInfo
import android.content.res.Configuration
import android.view.Surface
import android.view.WindowManager
import com.android.volley.DefaultRetryPolicy
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.TimeoutError
import com.android.volley.VolleyError
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import com.darakeon.dfm.R
import com.darakeon.dfm.auth.languageChangeAndSave
import com.darakeon.dfm.auth.themeChangeAndSave
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.base.SmartStatic
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.dialogs.createWaitDialog
import com.darakeon.dfm.extensions.composeEmail
import com.darakeon.dfm.extensions.info
import com.darakeon.dfm.extensions.isProd
import com.darakeon.dfm.extensions.logout
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.stackTraceText
import com.darakeon.dfm.tfa.TFAActivity
import org.json.JSONObject
import java.util.HashMap

class InternalRequest<T : SmartStatic>(
	private var activity: BaseActivity<T>,
	private val url: String,
	private var handleSuccess: ((JSONObject) -> Unit)?
) {

	constructor(activity: BaseActivity<T>, url: String)
		: this(activity, url, null)

	private val parameters: HashMap<String, Any?> = HashMap()

	private var jsonRequest: JsonObjectRequest? = null

	private var progress: Dialog? = null


	fun addParameter(key: String, value: Any?) {
		parameters[key] = value
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
			activity.static.succeeded = false
			return
		}

		val parameters = getParameters() ?: return

		val completeUrl = getUrl()

		val jsonRequest = JsonObjectRequest(method, completeUrl, parameters,
			Response.Listener<JSONObject> { response -> handleResponse(response) },
			Response.ErrorListener { e -> handleError(completeUrl, e) })

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
		val main = getSite("Api")
		val auth = getParameterUrl(parameters, "ticket", "-")
		val account = getParameterUrl(parameters, "accountUrl", "/Account-")
		val path =  "/$url"
		val id = getParameterUrl(parameters, "id", "/")

		return main + auth + account + path + id
	}

	private fun getParameterUrl(parameters: HashMap<String, Any?>, key: String, prefix: String): String {
		if (parameters.containsKey(key)) {
			val result = prefix + parameters[key]
			parameters.remove(key)
			if (result != prefix) {
				return result
			}
		}

		return ""
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

	private fun handleError(url: String, error: VolleyError) {
		if (error is TimeoutError) {
			handleTimeout()
		} else {
			handleOtherErrors(url, error)
		}
	}

	private fun handleTimeout() {
		val response = InternalResponse(activity.getString(R.string.timeout))
		assemblyResponse(response)
	}

	private fun handleOtherErrors(url: String, error: VolleyError) {
		val response = InternalResponse(
			activity.getString(R.string.error_contact_url)
		)

		val sendReport = DialogInterface.OnClickListener(function = { dialog, _ ->
			dialog.dismiss()

			val subject = activity.getString(R.string.error_mail_title)

			val reportUrl =
				Regex("Api-\\w{32}")
					.replace(url, "Api-{ticket}")

			val body = reportUrl + "\n\n" +
					error.info + "\n\n" +
					error.stackTraceText

			val emails = activity.getString(R.string.error_mail_address)

			activity.composeEmail(subject, body, emails)
		})

		assemblyResponse(response, sendReport)
	}

	private fun assemblyResponse(internalResponse: InternalResponse, sendEmailReport: DialogInterface.OnClickListener? = null) {
		activity.endUIWait()

		activity.static.succeeded = internalResponse.isSuccess()

		if (!activity.static.succeeded) {
			activity.alertError(internalResponse.getError(), sendEmailReport)
			return
		}

		val result = internalResponse.getResult()

		if (result.has("error")) {
			val error = result.getString("error")

			val code =
				if (result.has("code"))
					result.getInt("code")
				else
					0

			val tfa = activity.resources.getInteger(R.integer.TFA)
			val uninvited = activity.resources.getInteger(R.integer.uninvited)

			if (code == tfa) {
				activity.redirect<TFAActivity>()
			} else {
				activity.alertError(error)

				if (code == uninvited) {
					activity.logout()
				}
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
