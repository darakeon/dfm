package com.darakeon.dfm.error_logs

import android.content.Context
import android.util.AttributeSet
import android.widget.GridLayout
import com.darakeon.dfm.error_logs.databinding.ErrorLineBinding
import com.darakeon.dfm.lib.api.entities.status.ErrorLog

class ErrorLine(
	context: Context, attributeSet: AttributeSet
) : GridLayout(context, attributeSet) {
	private lateinit var binding: ErrorLineBinding

	fun set(log: ErrorLog, archive: () -> Unit) {
		binding = ErrorLineBinding.bind(this)

		binding.lastDate.text = log.date()
		binding.message.text = log.message()
		binding.count.text = log.count.toString()

		isClickable = true

		setOnLongClickListener {
			archive()
			true
		}
	}
}
