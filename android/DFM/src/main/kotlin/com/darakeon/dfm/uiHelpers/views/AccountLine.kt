package com.darakeon.dfm.uiHelpers.views

import android.content.Context
import android.content.Intent
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.activities.base.setColorByAttr
import com.darakeon.dfm.uiHelpers.adapters.AccountAdapter
import java.text.DecimalFormat

class AccountLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	private val nameField: TextView get() = findViewById(R.id.name)
	private val totalField: TextView get() = findViewById(R.id.value)

	fun setAccount(account: AccountAdapter.Account, color: Int) {
		setBackgroundColor(color)

		nameField.text = account.name

		val totalColor = if (account.total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (account.total < 0) -account.total else account.total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		totalField.setColorByAttr(totalColor)
		totalField.text = totalStr

		isClickable = true

		setOnClickListener(OnClickListener(context, account.url))
	}

	inner class OnClickListener internal constructor(private val context: Context, private val url: String) : View.OnClickListener {

		override fun onClick(v: View) {
			val intent = Intent(context, ExtractActivity::class.java)
			intent.putExtra("accountUrl", url)
			context.startActivity(intent)
		}
	}
}
