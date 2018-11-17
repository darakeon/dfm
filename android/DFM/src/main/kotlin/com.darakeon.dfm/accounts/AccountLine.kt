package com.darakeon.dfm.accounts

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.LinearLayout
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.accounts.Account
import com.darakeon.dfm.extensions.redirect
import com.darakeon.dfm.extensions.setColorByAttr
import com.darakeon.dfm.extract.ExtractActivity
import kotlinx.android.synthetic.main.accounts_line.view.name
import kotlinx.android.synthetic.main.accounts_line.view.value
import java.text.DecimalFormat

class AccountLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	fun setAccount(account: Account, color: Int) {
		setBackgroundColor(color)

		name.text = account.name

		val totalColor = if (account.total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (account.total < 0) -account.total else account.total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		value.setColorByAttr(totalColor)
		value.text = totalStr

		isClickable = true

		setOnClickListener(OnClickListener(context, account.url))
	}

	inner class OnClickListener internal constructor(
		private val context: Context,
		private val url: String
	) : View.OnClickListener {
		override fun onClick(v: View) {
			context.redirect<ExtractActivity> {
				it.putExtra("accountUrl", url)
			}
		}
	}
}
