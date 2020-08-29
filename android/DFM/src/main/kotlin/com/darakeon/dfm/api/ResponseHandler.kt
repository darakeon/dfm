package com.darakeon.dfm.api

import android.app.Activity
import android.content.Context
import com.darakeon.dfm.BuildConfig
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.auth.setEnvironment
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.tfa.TFAActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.net.ConnectException
import java.net.SocketTimeoutException

internal class ResponseHandler<C, A>(
	private val caller: C,
	private val ui: UIHandler?,
	private val onSuccess: (A) -> Unit,
) : Callback<Body<A>>
	where C: Context, C: Caller
{
	override fun onResponse(call: Call<Body<A>>, response: Response<Body<A>>?) {
		ui?.endUIWait()

		if (response == null) {
			onError(call, ApiException("Null response"))
			return
		}

		val body = response.body()

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
	}

	override fun onFailure(call: Call<Body<A>>, throwable: Throwable) {
		ui?.endUIWait()

		when (throwable) {
			is SocketTimeoutException, is ConnectException ->
				caller.error(R.string.internet_too_slow)
			else ->
				onError(call, throwable)
		}
	}

	private fun onError(call: Call<Body<A>>, error: Throwable) {
		if (BuildConfig.DEBUG) throw error

		val url = call.request().url().encodedPath()

		caller.error(R.string.error_contact_url) {
			caller.error(url, error)
		}
	}

	private fun assemblyResponse(code: Int?, error: String?) {
		val tfa = caller.resources.getInteger(R.integer.TFA)
		val uninvited = caller.resources.getInteger(R.integer.uninvited)

		when (code) {
			tfa -> caller.redirect<TFAActivity>()
			uninvited -> caller.logout()
			else -> caller.error(getError(error))
		}
	}

	private fun getError(error: String?) =
		error ?: caller.getString(R.string.error_not_identified)
}
