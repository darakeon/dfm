package com.darakeon.dfm.error_logs

import com.darakeon.dfm.lib.api.Api
import com.darakeon.dfm.lib.api.entities.errors.ErrorLog
import com.darakeon.dfm.lib.ui.Adapter

class ErrorAdapter(
	private val activity: ListActivity,
	private val api: Api<*>,
	private val logs: MutableList<ErrorLog>
): Adapter<ListActivity, ErrorLog, ErrorLine>(
	activity, logs
) {
	override val lineLayoutId: Int
		get() = R.layout.error_line

	override fun populateView(view: ErrorLine, position: Int) =
		view.set(logs[position]) {
			archiveAll(position)
		}

	private fun archiveAll(position: Int) {
		api.archiveErrors(logs[position].id) {
			logs.removeAt(position)
			notifyDataSetChanged()
		}
	}
}
