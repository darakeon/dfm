package com.darakeon.dfm.api

import android.content.DialogInterface
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
	override fun onResponse(call: Call<Body<T>>?, response: Response<Body<T>>?) {
		val url = getUrl(call)

		if (response == null) {
			onError(url, Exception("Null response"))
			return
		}

		val body = response.body()

		if (body?.environment != null) {
			activity.setEnvironment(body.environment)
		}

		when {
			body == null ->
				onError(url, Exception("Null body"))

			body.data == null ->
				assemblyResponse(body.code, body.error)

			else -> onSuccess(body.data)
		}

		uiHandler.endUIWait()
	}

	override fun onFailure(call: Call<Body<T>>?, throwable: Throwable?) {
		onError(
			getUrl(call),
			throwable ?: Exception("Null error")
		)
		uiHandler.endUIWait()
	}

	private fun getUrl(call: Call<Body<T>>?): String {
		return call?.request()?.url()?.encodedPath() ?: "No url info"
	}

	private fun onError(url: String?, error: Throwable) {
		if (error is SocketTimeoutException
				|| error is ConnectException) {
			activity.alertError(R.string.internet_too_slow)
			return
		}

		if (BuildConfig.DEBUG) throw error

		val sendReport = DialogInterface.OnClickListener(function = { dialog, _ ->
			dialog.dismiss()
			activity.composeErrorEmail(url ?: "", error)
		})

		activity.alertError(R.string.error_contact_url, sendReport)
	}

	private fun assemblyResponse(
		code: Int?,
		error: String?
	) {
		val tfa = activity.resources.getInteger(R.integer.TFA)
		val uninvited = activity.resources.getInteger(R.integer.uninvited)

		if (code == tfa) {
			activity.redirect<TFAActivity>()
		} else {
			activity.alertError(error ?: "")

			if (code == uninvited) {
				activity.logoutLocal()
			}
		}
	}
}
