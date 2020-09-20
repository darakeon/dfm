package com.darakeon.dfm.error_logs.service

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context.NOTIFICATION_SERVICE
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

	private val manager: NotificationManager
		get() = service.getSystemService(NOTIFICATION_SERVICE) as NotificationManager

	init {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
			val name = "ERROR"
			val descriptionText = "LOGS"
			val channel = NotificationChannel(channelID, name, priorityNewer)
				.apply { description = descriptionText }

			manager.createNotificationChannel(channel)
		}
	}

	fun notify(message: String) {
		val builder = NotificationCompat.Builder(service, channelID)
			.setSmallIcon(R.drawable.notification)
			.setContentTitle(message)
			.setPriority(priorityOlder)

		service.startForeground(27, builder.build())
	}
}
