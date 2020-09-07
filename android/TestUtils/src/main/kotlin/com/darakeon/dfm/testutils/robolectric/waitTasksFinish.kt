package com.darakeon.dfm.testutils.robolectric

import android.os.Looper.getMainLooper
import org.robolectric.Shadows.shadowOf

fun waitTasksFinish() {
	shadowOf(getMainLooper()).idle()
}
