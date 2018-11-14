package com.darakeon.dfm.api

import android.app.Activity
import android.content.DialogInterface
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Body
import com.darakeon.dfm.dialogs.alertError
import com.darakeon.dfm.extensions.composeErrorEmail
import com.darakeon.dfm.extensions.logoutLocal
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.tfa.TFAActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

internal class Handler<T>(
	private val context: Activity,
	private val onSuccess: (T) -> Unit
)  : Callback<Body<T>> {
	override fun onResponse(call: Call<Body<T>>?, response: Response<Body<T>>?) {
		val url = getUrl(call)

		if (response == null) {
			onError(url, Exception("Null response"))
			return
		}

		val body = response.body()

		when {
			body == null ->
				onError(url, Exception("Null body"))

			body.data == null ->
				assemblyResponse(body.code, body.error)

			else -> onSuccess(body.data)
		}

		context.endUIWait()
	}

	override fun onFailure(call: Call<Body<T>>?, throwable: Throwable?) {
		val url = getUrl(call)

		if (throwable == null) {
			onError(url, Exception("Null error"))
		} else {
			onError(url, throwable)
		}
	}

	private fun getUrl(call: Call<Body<T>>?): String {
		return call?.request()?.url()?.encodedPath() ?: "No url info"
	}

	private fun onError(url: String?, error: Throwable) {
		val sendReport = DialogInterface.OnClickListener(function = { dialog, _ ->
			dialog.dismiss()
			context.composeErrorEmail(url ?: "", error)
		})

		context.alertError(context.getString(R.string.error_contact_url), sendReport)
	}

	private fun assemblyResponse(
		code: Int?,
		error: String?
	) {
		val tfa = context.resources.getInteger(R.integer.TFA)
		val uninvited = context.resources.getInteger(R.integer.uninvited)

		if (code == tfa) {
			context.redirect<TFAActivity>()
		} else {
			context.alertError(error ?: "")

			if (code == uninvited) {
				context.logoutLocal()
			}
		}
	}
}
