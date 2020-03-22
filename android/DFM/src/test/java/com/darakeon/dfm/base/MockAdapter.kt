package com.darakeon.dfm.base

import android.app.Activity
import android.view.View
import android.widget.TextView

class MockAdapter(
	activity: Activity,
	list: Array<String>
): Adapter<Activity, String, View>(activity, list)
{
	override val id = android.R.layout.simple_list_item_1
	override fun populateView(view: View, position: Int) {
		view.findViewById<TextView>(
			android.R.id.text1
		).text = list[position]
	}
}
