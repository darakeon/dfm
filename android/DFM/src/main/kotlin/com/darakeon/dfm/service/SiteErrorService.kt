package com.darakeon.dfm.service

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.IBinder
import androidx.annotation.RequiresApi
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import com.darakeon.dfm.R
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
		initChannel()
	}

	override fun onStart(intent: Intent?, startid: Int) {
		api.listErrors { list ->
			list.forEach {
				notify(it.id(), it.title(), it.message())
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

	private var channelID = "0"
	private val priorityOlder = NotificationCompat.PRIORITY_DEFAULT
	@RequiresApi(Build.VERSION_CODES.O)
	private val priorityNewer = NotificationManager.IMPORTANCE_DEFAULT

	private fun notify(error: String) {
		val title = getString(R.string.error_title)
		notify(0, title, error)
	}

	private fun notify(id: Int, title: String, text: String) {
		val builder = NotificationCompat.Builder(this, channelID)
			.setSmallIcon(R.drawable.notification)
			.setContentTitle(title)
			.setContentText(text)
			.setPriority(priorityOlder)

		with(NotificationManagerCompat.from(this)) {
			notify(id, builder.build())
		}
	}

	private fun initChannel() {
		if (Build.VERSION.SDK_INT < Build.VERSION_CODES.O) return

		val name = "ERROR"
		val descriptionText = "LOGS"
		val channel = NotificationChannel(channelID, name, priorityNewer)
			.apply { description = descriptionText }

		val notificationManager: NotificationManager =
			getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

		notificationManager.createNotificationChannel(channel)
	}

}
