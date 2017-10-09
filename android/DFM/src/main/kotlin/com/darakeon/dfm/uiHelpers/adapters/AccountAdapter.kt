package com.darakeon.dfm.uiHelpers.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.uiHelpers.views.AccountLine
import com.darakeon.dfm.user.Theme
import org.json.JSONArray
import org.json.JSONObject

class AccountAdapter(context: Context, accountJsonList: JSONArray) : BaseAdapter() {

	private val accountList: MutableList<Account>
	private var inflater: LayoutInflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	init {
		accountList =
			(0..accountJsonList.length() - 1)
				.map { Account(accountJsonList.getJSONObject(it)) }
				.toMutableList()

	}

	inner class Account(jsonObject: JSONObject) {
		var Name: String = jsonObject.getString("Name")
		var Total: Double = jsonObject.getDouble("Total")
		var Url: String = jsonObject.getString("Url")
	}

	override fun getCount(): Int {
		return accountList.size
	}

	override fun getItem(position: Int): Any {
		return position
	}

	override fun getItemId(position: Int): Long {
		return position.toLong()
	}

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.accounts_line, null) as AccountLine

		val color = Theme.getLineColor(position)
		line.setAccount(accountList[position], color)

		return line
	}


}
