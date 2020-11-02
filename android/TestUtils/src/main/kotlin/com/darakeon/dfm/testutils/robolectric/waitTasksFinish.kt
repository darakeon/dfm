package com.darakeon.dfm.testutils.robolectric

import android.os.Build
import android.os.Looper.getMainLooper
import androidx.annotation.RequiresApi
import com.darakeon.dfm.testutils.getPrivate
import org.robolectric.Shadows.shadowOf

@RequiresApi(Build.VERSION_CODES.M)
fun Any.waitTasksFinish() {
	var started: Boolean
	var ended: Boolean

	do {
		shadowOf(getMainLooper()).idle()
		started = this.getPrivate("waitStarted") ?: true
		ended = this.getPrivate("waitEnded") ?: true
	} while(!started || !ended)
}
