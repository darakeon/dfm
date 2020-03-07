package com.darakeon.dfm.utils

import android.app.AlertDialog
import org.hamcrest.CoreMatchers.`is`
import org.junit.Assert.assertNotNull
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.robolectric.Shadows.shadowOf

fun assertAlertWait(alert: AlertDialog?) {
	assertAlert(alert, "Wait!", "Calling the Pig…")
}

private fun assertAlert(alert: AlertDialog?, title: String, message: String) {
	assertNotNull(alert)
	if (alert == null) return

	assertTrue(alert.isShowing)

	val shadowAlert = shadowOf(alert)
	assertThat(shadowAlert.title.toString(), `is`(title))
	assertThat(shadowAlert.message.toString(), `is`(message))
}
