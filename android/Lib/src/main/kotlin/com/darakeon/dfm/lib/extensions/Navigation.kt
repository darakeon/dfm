package com.darakeon.dfm.lib.extensions

import android.app.Activity

fun Activity.refresh() {
	finish()
	startActivity(intent)
}
