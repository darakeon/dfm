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
import org.json.JSONException
import java.text.DecimalFormat



class AccountLine(context: Context, attributeSet: AttributeSet) : LinearLayout(context, attributeSet) {

	override fun onFinishInflate() {
		super.onFinishInflate()
	}

	val NameField: TextView get() = findViewById(R.id.name) as TextView
	val TotalField: TextView get() = findViewById(R.id.value) as TextView

	@Throws(JSONException::class)
	fun setAccount(account: AccountAdapter.Account, color: Int) {
		setBackgroundColor(color)

		NameField.text = account.Name

		val totalColor = if (account.Total < 0) R.attr.negative else R.attr.positive
		val totalToShow = if (account.Total < 0) -account.Total else account.Total
		val totalStr = DecimalFormat("#,##0.00").format(totalToShow)

		TotalField.setColorByAttr(totalColor)
		TotalField.text = totalStr

		isClickable = true

		setOnClickListener(onClickListener(context, account.Url))
	}

	inner class onClickListener internal constructor(private val context: Context, private val url: String) : OnClickListener {

		override fun onClick(v: View) {
			val intent = Intent(context, ExtractActivity::class.java)
			intent.putExtra("accountUrl", url)
			context.startActivity(intent)
		}
	}

}
