package com.darakeon.dfm.error_logs.service

import android.app.Service
import android.content.Intent
import android.os.IBinder
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.auth.Authentication

class SiteErrorService : Service(), ApiCaller, Timer.Caller {
	override var api: Api<SiteErrorService>? = null
	private lateinit var auth: Authentication

	private val notification = Notification(this)

	override var timer = Timer(this, 30 * 60) { this.callServer() }

	override fun onBind(intent: Intent?): IBinder? {
		return null
	}

	override fun onCreate() {
		api = Api(this, null)
		auth = Authentication(this)
		notification.init()
	}

	override fun onStart(intent: Intent?, startid: Int) {
		timer.start()
	}

	private fun callServer() {
		api?.listErrors { list ->
			val count = list.logs.size
			if (count == 1) {
				val note = list.logs[0]
				notification.notify(note.id(), note.title(), note.message())
			} else if (count > 0) {
				notification.notify("Errors count: $count")
			}
		}
	}

	override fun onDestroy() {
		timer.cancel()
		api?.cancel()
	}

	override val ticket: String
		get() = auth.ticket

	override fun error(text: String) {
		notification.notify(text)
	}

	override fun error(resId: Int) = error(getString(resId))
	override fun error(resId: Int, action: () -> Unit) = error(resId)
	override fun error(url: String, error: Throwable) = error("$url > $error")

	override fun checkTFA() {
		error(R.string.uninvited)
		stopSelf()
	}

	override fun logout() {
		error(R.string.uninvited)
		stopSelf()
	}

	override fun startWait() { }
	override fun endWait() { }
}
