package com.darakeon.dfm.testutils.robolectric

import android.app.Activity
import android.os.Build
import androidx.annotation.RequiresApi
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.testutils.getPrivate
import org.robolectric.Shadows.shadowOf
import java.util.Locale

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
	var started: Boolean
	var ended: Boolean

	do {
		shadowOf(mainLooper).idle()

		started = this.getPrivate("Started", count)
		ended = this.getPrivate("Ended", count)
	} while(!started || !ended)
}

private fun Any.getPrivate(name: String, count: Int?) =
	if (count == null) {
		this.getPrivate("wait$name") ?: true
	} else {
		val prop = name.toLowerCase(Locale.getDefault())
		this.getPrivate<Int>("ui", prop) >= count
	}

interface Waitable {
	val count: Int
}
