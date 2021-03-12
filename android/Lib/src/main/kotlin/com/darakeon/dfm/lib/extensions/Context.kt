package com.darakeon.dfm.lib.extensions

import android.content.Context
import android.util.TypedValue

fun Context.getColorByAttr(attr : Int): Int {
	val typedValue = TypedValue()
	theme.resolveAttribute(attr, typedValue, true)
	return typedValue.data
}
