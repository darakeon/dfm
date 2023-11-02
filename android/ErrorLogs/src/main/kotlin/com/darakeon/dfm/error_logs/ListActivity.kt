package com.darakeon.dfm.error_logs

import android.os.Bundle
import android.os.Handler
import android.view.View
import com.darakeon.dfm.error_logs.databinding.ListBinding
import com.darakeon.dfm.error_logs.service.SiteErrorService
import com.darakeon.dfm.lib.Log
import com.darakeon.dfm.lib.api.entities.status.ErrorList
import com.darakeon.dfm.lib.api.entities.status.ErrorLog
import com.darakeon.dfm.lib.extensions.refresh

class ListActivity : BaseActivity<ListBinding>() {
	private var logs: MutableList<ErrorLog> = mutableListOf()

	override fun inflateBinding(): ListBinding {
		return ListBinding.inflate(layoutInflater)
	}

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.list)

		Handler(mainLooper).postDelayed({
			api.listErrors(this::fillList)
		}, 5000)

		toggleButtons()

		binding.main.setOnRefreshListener { refresh() }
		binding.main.listChild = binding.list
	}

	private fun fillList(errors: ErrorList) {
		if (errors.count == 0) {
			binding.message.setText(R.string.empty)
			logs.clear()
		} else {
			binding.message.text = ""
			errors.logs.forEach(logs::add)
		}

		binding.list.adapter = ErrorAdapter(this, api, logs)
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
		Log.record(SiteErrorService.running)
		val running = SiteErrorService.running
		toggle(binding.stop, running)
		toggle(binding.start, !running)
	}

	private fun toggle(view: View, visible: Boolean) {
		view.visibility = if (visible) View.VISIBLE else View.GONE
	}
}
