package com.darakeon.dfm.utils

import android.os.Looper.getMainLooper
import org.robolectric.Shadows.shadowOf

fun waitTasksFinish() {
	shadowOf(getMainLooper()).idle()
}
