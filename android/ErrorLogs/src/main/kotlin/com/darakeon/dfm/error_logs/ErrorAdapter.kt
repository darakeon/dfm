package com.darakeon.dfm.error_logs

import com.darakeon.dfm.lib.ui.Adapter

class ErrorAdapter(
	private val activity: ListActivity,
	private val logs: MutableList<ErrorGroup>
): Adapter<ListActivity, ErrorGroup, ErrorLine>(
	activity, logs
) {
	override val lineLayoutId: Int
		get() = R.layout.error_line

	override fun populateView(view: ErrorLine, position: Int) =
		view.set(logs[position]) {
			archiveAll(position)
		}

	private fun archiveAll(position: Int) {
		val log = logs[position]
		val id = log.ids[0]

		activity.api.archiveError(id) {
			log.remove(id)
			if (log.count == 0) {
				logs.removeAt(position)
			} else {
				archiveAll(position)
			}
			notifyDataSetChanged()
		}
	}
}
