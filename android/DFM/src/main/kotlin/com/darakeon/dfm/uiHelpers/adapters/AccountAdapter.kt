package com.darakeon.dfm.uiHelpers.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.uiHelpers.views.AccountLine
import com.darakeon.dfm.user.getThemeLineColor
import org.json.JSONArray
import org.json.JSONObject

class AccountAdapter(context: Context, accountJsonList: JSONArray) : BaseAdapter() {

	private val accountList: MutableList<Account>
	private var inflater: LayoutInflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	init {
		accountList =
			(0 until accountJsonList.length())
				.map { Account(accountJsonList.getJSONObject(it)) }
				.toMutableList()

	}

	inner class Account(jsonObject: JSONObject) {
		var name: String = jsonObject.getString("Name")
		var total: Double = jsonObject.getDouble("Total")
		var url: String = jsonObject.getString("Url")
	}

	override fun getCount(): Int = accountList.size
	override fun getItem(position: Int): Any = position
	override fun getItemId(position: Int): Long = position.toLong()

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.accounts_line, null) as AccountLine

		val color = getThemeLineColor(position)
		line.setAccount(accountList[position], color)

		return line
	}


}
