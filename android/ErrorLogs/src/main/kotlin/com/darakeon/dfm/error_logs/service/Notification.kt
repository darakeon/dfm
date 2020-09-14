package com.darakeon.dfm.error_logs.service

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context
import android.os.Build
import androidx.annotation.RequiresApi
import androidx.core.app.NotificationCompat
import com.darakeon.dfm.lib.R

class Notification(private val service: Service) {
	private var channelID = "dfm"

	private val priorityOlder =
		NotificationCompat.PRIORITY_DEFAULT

	@RequiresApi(Build.VERSION_CODES.O)
	private val priorityNewer =
		NotificationManager.IMPORTANCE_DEFAULT

	private var n = 0

	fun init() {
		if (Build.VERSION.SDK_INT < Build.VERSION_CODES.O) return

		val name = "ERROR"
		val descriptionText = "LOGS"
		val channel = NotificationChannel(channelID, name, priorityNewer)
			.apply { description = descriptionText }

		val notificationManager: NotificationManager =
			service.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

		notificationManager.createNotificationChannel(channel)
	}

	fun notify(error: String) {
		val title = service.getString(R.string.error_title)
		notify(n++, title, error)
	}

	fun notify(id: Int, title: String, text: String) {
		val builder = NotificationCompat.Builder(service, channelID)
			.setSmallIcon(R.drawable.notification)
			.setContentTitle(title)
			.setContentText(text)
			.setPriority(priorityOlder)

		service.startForeground(id, builder.build())
	}
}
