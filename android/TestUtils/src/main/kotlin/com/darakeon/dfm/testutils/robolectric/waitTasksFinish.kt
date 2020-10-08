package com.darakeon.dfm.testutils.robolectric

import android.os.Build
import android.os.Looper.getMainLooper
import androidx.annotation.RequiresApi
import org.robolectric.Robolectric.flushForegroundThreadScheduler
import org.robolectric.Shadows.shadowOf

@RequiresApi(Build.VERSION_CODES.M)
fun waitTasksFinish() {
	shadowOf(getMainLooper()).idle()
	flushForegroundThreadScheduler()
	shadowOf(getMainLooper()).idle()
}
