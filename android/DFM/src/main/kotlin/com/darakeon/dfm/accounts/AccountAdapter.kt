package com.darakeon.dfm.accounts

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.auth.getThemeLineColor

class AccountAdapter(
	context: Context,
	private val accountList: List<com.darakeon.dfm.api.Account>
) : BaseAdapter() {
	private var inflater: LayoutInflater =
		context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	override fun getCount(): Int = accountList.size
	override fun getItem(position: Int): Any = position
	override fun getItemId(position: Int): Long = position.toLong()

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = createOrGet(view, viewGroup) as AccountLine

		val color = getThemeLineColor(position)
		line.setAccount(accountList[position], color)

		return line
	}

	private fun createOrGet(view: View?, viewGroup: ViewGroup) =
		(view ?: inflater.inflate(R.layout.accounts_line, null))
}
