package com.darakeon.dfm.lib.extensions

import android.view.View
import android.widget.GridLayout

fun View.changeColSpan(size: Int) {
	val layoutParams = layoutParams as GridLayout.LayoutParams
	layoutParams.columnSpec = GridLayout.spec(GridLayout.UNDEFINED, size, size.toFloat())
}
