package com.darakeon.dfm.error_logs

import android.os.Bundle
import android.os.Handler
import android.view.View
import com.darakeon.dfm.error_logs.service.SiteErrorService
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.status.ErrorLog
import com.darakeon.dfm.lib.extensions.log
import com.darakeon.dfm.lib.extensions.refresh
import kotlinx.android.synthetic.main.list.list
import kotlinx.android.synthetic.main.list.main
import kotlinx.android.synthetic.main.list.message
import kotlinx.android.synthetic.main.list.start
import kotlinx.android.synthetic.main.list.stop

class ListActivity : BaseActivity() {
	private var logs: MutableList<ErrorGroup> = mutableListOf()

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.list)

		Handler().postDelayed({
			api.listErrors(this::fillList)
		}, 5000)

		toggleButtons()

		main.setOnRefreshListener { refresh() }
	}

	private fun fillList(errors: ErrorList) {
		if (errors.count == 0) {
			message.setText(R.string.empty)
			logs.clear()
		} else {
			message.text = ""
			errors.logs.forEach(this::addLog)
		}

		list.adapter = ErrorAdapter(this, logs)
	}

	private fun addLog(log: ErrorLog) {
		val group = logs.singleOrNull {
			it.message == log.message()
		}

		if (group == null) {
			logs.add(ErrorGroup(log))
		} else {
			group.add(log)
		}
	}

	@Suppress("UNUSED_PARAMETER")
	fun stopService(view: View) {
		SiteErrorService.stop(this)
		toggleButtons()
	}

	@Suppress("UNUSED_PARAMETER")
	fun startService(view: View) {
		SiteErrorService.start(this)
		toggleButtons()
	}

	private fun toggleButtons() {
		log(SiteErrorService.running)
		stop.isEnabled = SiteErrorService.running
		start.isEnabled = !SiteErrorService.running
	}
}
