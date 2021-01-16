package com.darakeon.dfm.error_logs

import android.content.Context
import android.util.AttributeSet
import android.widget.GridLayout
import com.darakeon.dfm.lib.api.entities.status.ErrorLog
import kotlinx.android.synthetic.main.error_line.view.count
import kotlinx.android.synthetic.main.error_line.view.last_date
import kotlinx.android.synthetic.main.error_line.view.message

class ErrorLine(
	context: Context, attributeSet: AttributeSet
) : GridLayout(context, attributeSet) {
	fun set(log: ErrorLog, archive: () -> Unit) {
		last_date.text = log.date()
		message.text = log.message()
		count.text = log.count.toString()

		isClickable = true

		setOnLongClickListener {
			archive()
			true
		}
	}
}
