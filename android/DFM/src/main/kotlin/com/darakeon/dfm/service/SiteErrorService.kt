package com.darakeon.dfm.service

import android.app.Service
import android.content.Intent
import android.os.IBinder
import android.widget.Toast
import com.darakeon.dfm.api.Api
import com.darakeon.dfm.api.Caller
import com.darakeon.dfm.auth.Authentication

class SiteErrorService : Service() {
	override fun onBind(intent: Intent?): IBinder? {
		return null
	}

	override fun onCreate() {
	}

	override fun onStart(intent: Intent?, startid: Int) {
	}

	override fun onDestroy() {
	}
}
