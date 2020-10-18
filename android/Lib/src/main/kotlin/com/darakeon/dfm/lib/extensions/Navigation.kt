package com.darakeon.dfm.lib.extensions

import android.app.Activity
import android.content.Context
import android.content.Intent

fun Activity.refresh() {
	finish()
	startActivity(intent)
}

inline fun <reified T : Activity> Context.redirect(
	function: (Intent) -> Unit
) {
	val intent = Intent(this, T::class.java)
	function(intent)
	startActivity(intent)
}

inline fun <reified T : Activity> Context.redirect() {
	redirect<T> {}
}
