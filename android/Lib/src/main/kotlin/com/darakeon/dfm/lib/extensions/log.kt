package com.darakeon.dfm.lib.extensions

import android.util.Log

fun log(text: Any) {
	try {
		Log.e("DFM", text.toString())
	} catch(e: Exception) {
		println(text)
	}
}
