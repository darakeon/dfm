package com.darakeon.dfm.base

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.lib.auth.getThemeLineColor

abstract class Adapter<Act, Ent, Lin>(
	private val activity: Act,
	protected val list: List<Ent>
) : BaseAdapter()
	where Ent : Any, Act : Context, Lin: View
{
	private var inflater: LayoutInflater =
		activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	override fun getCount(): Int = list.size
	override fun getItem(position: Int): Any = list[position]
	override fun getItemId(position: Int): Long = position.toLong()

	abstract val id: Int
	abstract fun populateView(view: Lin, position: Int)

	override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
		val view = convertView ?: inflater.inflate(id, parent, false)

		val color = activity.getThemeLineColor(position)
		view.setBackgroundColor(color)

		@Suppress("UNCHECKED_CAST")
		populateView(view as Lin, position)

		return view
	}
}
