package com.darakeon.dfm.extract

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.Move
import com.darakeon.dfm.api.old.getCalendar
import com.darakeon.dfm.auth.getThemeLineColor
import org.json.JSONArray
import org.json.JSONObject
import java.util.*

class MoveAdapter(private val activity: ExtractActivity, moveJsonList: Array<Move>, private val canCheck: Boolean) : BaseAdapter() {

	private val moveList: MutableList<Move> = moveJsonList.toMutableList()
	private var inflater: LayoutInflater = activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	override fun getCount(): Int = moveList.size
	override fun getItem(position: Int): Any = position
	override fun getItemId(position: Int): Long = position.toLong()

	override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
		val line = inflater.inflate(R.layout.extract_line, null) as MoveLine

		val color = getThemeLineColor(position)
		line.setMove(activity, moveList[position], color, canCheck)

		return line
	}

}
