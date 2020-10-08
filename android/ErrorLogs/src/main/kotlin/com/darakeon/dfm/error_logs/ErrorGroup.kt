package com.darakeon.dfm.error_logs

import com.darakeon.dfm.lib.api.entities.status.ErrorLog

class ErrorGroup(
	log: ErrorLog,
) {
	val ids = mutableListOf(log.id)

	var lastDate = log.date()
		private set

	val message = log.message()

	var count = 1
		private set

	fun add(log: ErrorLog) {
		if (log.date() > lastDate)
			lastDate = log.date()

		ids.add(log.id)
		count++
	}

	fun remove(id: String) {
		ids.remove(id)
		count--
	}
}
