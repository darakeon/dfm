package com.darakeon.dfm.accounts

import android.content.Context
import android.util.AttributeSet
import android.widget.LinearLayout
import com.darakeon.dfm.extract.ExtractActivity
import com.darakeon.dfm.lib.api.entities.accounts.Account
import com.darakeon.dfm.lib.extensions.redirect
import com.darakeon.dfm.lib.extensions.setValueColored
import kotlinx.android.synthetic.main.accounts_line.view.name
import kotlinx.android.synthetic.main.accounts_line.view.value

class AccountLine(
	context: Context, attributeSet: AttributeSet
) : LinearLayout(context, attributeSet) {
	fun setAccount(account: Account) {
		name.text = account.name

		value.setValueColored(account.total)

		isClickable = true

		setOnClickListener {
			context.redirect<ExtractActivity> {
				it.putExtra("accountUrl", account.url)
			}
		}
	}
}
