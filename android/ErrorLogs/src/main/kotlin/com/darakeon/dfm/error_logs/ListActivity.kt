package com.darakeon.dfm.error_logs

import android.os.Bundle
import android.os.Handler
import android.widget.ArrayAdapter
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.status.ErrorLog
import kotlinx.android.synthetic.main.activity_list.list

class ListActivity : BaseActivity() {
	private var logs: MutableList<ErrorLog> = mutableListOf()

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.activity_list)

		Handler().postDelayed({
			api.listErrors(this::fillList)
		}, 5000)
	}

	private fun fillList(errors: ErrorList) {
		if (errors.count == 0) {
			list.adapter = ArrayAdapter(
				this,
				android.R.layout.simple_list_item_1,
				arrayOf(getString(R.string.empty))
			)
		} else {
			logs = errors.logs.toMutableList()
			list.adapter = ErrorAdapter(this, logs)
		}
	}
}
