package com.darakeon.dfm.api

import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.auth.setEnvironment
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.composeErrorEmail
import com.darakeon.dfm.extensions.logoutLocal
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.tfa.TFAActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.net.ConnectException
import java.net.SocketTimeoutException

internal class ResponseHandler<T>(
	private val activity: BaseActivity,
	private val uiHandler: UIHandler,
	private val onSuccess: (T) -> Unit
)  : Callback<Body<T>> {
	override fun onResponse(call: Call<Body<T>>, response: Response<Body<T>>?) {
		uiHandler.endUIWait()

		if (response == null) {
			onError(call, ApiException("Null response"))
			return
		}

		val body = response.body()

		if (body?.environment != null) {
			activity.setEnvironment(body.environment)
		}

		when {
			body == null ->
				activity.alertError(R.string.body_null)

			body.data == null || body.code != null ->
				assemblyResponse(body.code, body.error)

			else -> onSuccess(body.data)
		}
	}

	override fun onFailure(call: Call<Body<T>>, throwable: Throwable) {
		uiHandler.endUIWait()

		when (throwable) {
			is SocketTimeoutException, is ConnectException ->
				activity.alertError(R.string.internet_too_slow)
			else -> {
				onError(call, throwable)
			}
		}
	}

	private fun onError(call: Call<Body<T>>, error: Throwable) {
		if (BuildConfig.DEBUG) throw error

		val url = call.request().url().encodedPath()

		activity.alertError(R.string.error_contact_url) {
			activity.composeErrorEmail(url, error)
		}
	}

	private fun assemblyResponse(code: Int?, error: String?) {
		val tfa = activity.resources.getInteger(R.integer.TFA)
		val uninvited = activity.resources.getInteger(R.integer.uninvited)

		when (code) {
			tfa -> activity.redirect<TFAActivity>()
			uninvited -> activity.logoutLocal()
			else -> activity.alertError(getError(error))
		}
	}

	private fun getError(error: String?) =
		error ?: activity.getString(R.string.error_not_identified)
}
