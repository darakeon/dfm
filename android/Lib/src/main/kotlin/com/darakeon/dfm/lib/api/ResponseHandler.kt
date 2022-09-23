package com.darakeon.dfm.lib.api

import android.app.Activity
import android.content.Context
import com.darakeon.dfm.lib.BuildConfig
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.auth.setEnvironment
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.net.ConnectException
import java.net.SocketTimeoutException

class ResponseHandler<C, A>(
	private val caller: C,
	private val onSuccess: (A) -> Unit,
) : Callback<Body<A>>
	where C: Context, C: ApiCaller
{
	init { logDebug("INIT") }

	override fun onResponse(call: Call<Body<A>>, response: Response<Body<A>>) {
		logDebug("RESPONSE ${call.request().url()}")

		val body = response.body()

		logDebug("STATUS ${response.code()}")

		if (body?.environment != null && caller is Activity) {
			caller.setEnvironment(body.environment)
		}

		when {
			body == null ->
				caller.error(R.string.body_null)

			body.data == null || body.code != null ->
				assemblyResponse(body.code, body.error)

			else -> onSuccess(body.data)
		}

		caller.endWait()
	}

	override fun onFailure(call: Call<Body<A>>, throwable: Throwable) {
		logDebug("FAIL ${call.request().url()}")

		caller.endWait()

		when (throwable) {
			is SocketTimeoutException, is ConnectException ->
				caller.offline()
			else ->
				onError(call, throwable)
		}
	}

	private fun onError(call: Call<Body<A>>, error: Throwable) {
		val url = call.request().url().encodedPath()

		if (BuildConfig.DEBUG) {
			caller.error(url, error)
			throw error
		}

		caller.error(R.string.error_contact_url, R.string.send_report_button) {
			caller.error(url, error)
		}
	}

	private fun assemblyResponse(code: Int?, error: String?) {
		val tfa = caller.resources.getInteger(R.integer.TFA)
		val uninvited = caller.resources.getInteger(R.integer.uninvited)

		when (code) {
			tfa -> caller.checkTFA()
			uninvited -> caller.logout()
			else -> caller.error(getError(error))
		}
	}

	private fun getError(error: String?) =
		error ?: caller.getString(R.string.error_not_identified)

	private fun logDebug(stage: Any?) {
		Log.record("$stage $caller")
	}
}
