package com.darakeon.dfm.lib.ui

import android.app.Activity
import android.view.View
import android.widget.TextView

class MockAdapter(
	activity: Activity,
	list: List<String>
): Adapter<Activity, String, View>(activity, list)
{
	override val lineLayoutId = android.R.layout.simple_list_item_1
	override fun populateView(view: View, position: Int) {
		view.findViewById<TextView>(
			android.R.id.text1
		).text = list[position]
	}
}
