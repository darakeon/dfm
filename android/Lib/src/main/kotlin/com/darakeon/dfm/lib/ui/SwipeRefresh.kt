package com.darakeon.dfm.lib.ui

import android.content.Context
import android.util.AttributeSet
import android.widget.ListView
import android.widget.ScrollView
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout

class SwipeRefresh(
	context: Context,
	attributes: AttributeSet
) : SwipeRefreshLayout(context, attributes) {
	var scrollChild: ScrollView? = null
	var listChild: ListView? = null

	override fun canChildScrollUp(): Boolean {
		if (scrollChild != null) {
			return scrollChild?.canScrollVertically(-1) ?: true
		}

		if (listChild != null) {
			return listChild?.firstVisiblePosition != 0
		}

		return true
	}
}
