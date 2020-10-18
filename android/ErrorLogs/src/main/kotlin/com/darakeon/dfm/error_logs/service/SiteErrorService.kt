package com.darakeon.dfm.error_logs.service

import android.app.Service
import android.content.Context
import android.content.Intent
import android.os.IBinder
import com.darakeon.dfm.error_logs.R
import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.ApiCaller
import com.darakeon.dfm.lib.auth.Authentication
import com.darakeon.dfm.lib.R as r

class SiteErrorService : Service(), ApiCaller, Timer.Caller {
	override var api: Api<SiteErrorService>? = null
	private lateinit var auth: Authentication
	private lateinit var notification: Notification

	override lateinit var timer: Timer

	override fun onBind(intent: Intent?): IBinder? = null

	override fun onCreate() {
		api = Api(this, null)
		auth = Authentication(this)
		notification = Notification(this)

		val minutes = resources.getInteger(
			R.integer.service_minutes_interval
		)
		timer = Timer(this, minutes * 60) {
			this.callServer()
		}
	}

	override fun onStart(intent: Intent?, startid: Int) {
		timer.start()
	}

	private fun callServer() {
		api?.countErrors { list ->
			notifyErrors(list.count)
		}
	}

	private var message: String = ""

	private fun notifyErrors(count: Int) {
		val message = if (count <= 0)
			getString(R.string.empty)
		else
			getString(R.string.notification_text).format(count)

		if (message != this.message) {
			this.message = message
			notification.notify(message)
		}
	}

	override fun onDestroy() {
		timer.cancel()
		api?.cancel()
		intent = null
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
		error(r.string.uninvited)
		stopSelf()
	}

	override fun logout() {
		error(r.string.uninvited)
		stopSelf()
	}

	companion object {
		fun start(context: Context) {
			if (!running) {
				intent = Intent(context, SiteErrorService::class.java)
				context.startService(intent)
			}
		}

		fun stop(context: Context) {
			context.stopService(intent)
			intent = null
		}

		private var intent: Intent? = null
		val running get() = intent != null
	}
}
