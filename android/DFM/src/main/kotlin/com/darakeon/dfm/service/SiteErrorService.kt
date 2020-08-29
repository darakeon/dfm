package com.darakeon.dfm.service

import android.app.Service
import android.content.Intent
import android.os.IBinder
import android.widget.Toast
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.api.Caller
import com.darakeon.dfm.auth.Authentication

class SiteErrorService : Service(), Caller {
	private lateinit var api: Api<SiteErrorService>
	private lateinit var auth: Authentication

	override fun onBind(intent: Intent?): IBinder? {
		return null
	}

	override fun onCreate() {
		api = Api(this, null)
		auth = Authentication(this)
	}

	override fun onStart(intent: Intent?, startid: Int) {
		api.listErrors { list ->
			list.forEach {
				notify("${it.id}: ${it.error().message}")
			}
		}
	}

	override fun onDestroy() {
		api.cancel()
	}

	override val ticket: String
		get() = auth.ticket

	override fun logout() { }
	override fun error(text: String) = notify(text)
	override fun error(resId: Int) = notify(toText(resId))
	override fun error(resId: Int, action: () -> Unit) = notify(toText(resId))
	override fun error(url: String, error: Throwable) = notify("$url > $error")

	private fun toText(resId: Int) =
		applicationContext.getString(resId)

	private fun notify(text: String) {
		Toast.makeText(this, text, Toast.LENGTH_LONG).show()
	}
}
