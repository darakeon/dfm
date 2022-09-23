package com.darakeon.dfm.testutils.robolectric

import android.app.Activity
import android.os.Build
import androidx.annotation.RequiresApi
import com.darakeon.dfm.testutils.getPrivate
import org.robolectric.Shadows.shadowOf

@RequiresApi(Build.VERSION_CODES.M)
fun Activity.waitTasks() {
	this.waitTasksWithCount(null)
}

@RequiresApi(Build.VERSION_CODES.M)
fun Activity.waitTasks(waitable: Waitable) {
	this.waitTasksWithCount(waitable.count)
}

@RequiresApi(Build.VERSION_CODES.M)
private fun Activity.waitTasksWithCount(count: Int?) {
	do {
		shadowOf(mainLooper).idle()
	} while (!started(count) || !ended(count))
}

private fun Activity.started(count: Int?) =
	this.getPrivate("Started", count)

private fun Activity.ended(count: Int?) =
	this.getPrivate("Ended", count)

private fun Any.getPrivate(name: String, count: Int?) =
	if (count == null) {
		this.getPrivate("wait$name") ?: true
	} else {
		val prop = name.lowercase()
		this.getPrivate<Int>("ui", prop) >= count
	}

interface Waitable {
	val count: Int
}
