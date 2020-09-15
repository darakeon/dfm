package com.darakeon.dfm.error_logs

import android.content.Context
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.lib.api.entities.status.ErrorLog
import kotlinx.android.synthetic.main.error_line.view.message
import kotlinx.android.synthetic.main.error_line.view.title

class ErrorLine(
	context: Context, attributeSet: AttributeSet
) : LinearLayout(context, attributeSet) {
	fun set(log: ErrorLog, archive: (id: String) -> Unit) {
		title.text = log.title()
		message.text = log.message()

		isClickable = true

		setOnLongClickListener {
			archive(log.id)
			true
		}
	}
}

