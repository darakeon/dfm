package com.darakeon.dfm.error_logs

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import com.darakeon.dfm.error_logs.service.SiteErrorService

class StartActivity : Activity() {
	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.activity_start)

		val intent = Intent(this, SiteErrorService::class.java)
		startService(intent)
	}
}
