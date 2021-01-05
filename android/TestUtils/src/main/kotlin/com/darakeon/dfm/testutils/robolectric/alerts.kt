package com.darakeon.dfm.testutils.robolectric

import android.app.AlertDialog
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertTrue
import org.robolectric.Shadows.shadowOf

fun AlertDialog.assertAlertWait() {
	assertAlert("Wait!", "Calling the Pig…")
}

private fun AlertDialog.assertAlert(title: String, message: String) {
	assertTrue(isShowing)

	val shadowAlert = shadowOf(this)
	assertThat(shadowAlert.title.toString(), `is`(title))
	assertThat(shadowAlert.message.toString(), `is`(message))
}
