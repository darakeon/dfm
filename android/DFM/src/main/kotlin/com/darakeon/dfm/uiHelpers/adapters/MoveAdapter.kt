package com.darakeon.dfm.uiHelpers.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.api.getCalendar
import com.darakeon.dfm.uiHelpers.views.MoveLine
import com.darakeon.dfm.user.getThemeLineColor
import org.json.JSONArray
import org.json.JSONObject
import java.util.*

class MoveAdapter(private val activity: ExtractActivity, moveJsonList: JSONArray, private val canCheck: Boolean) : BaseAdapter() {

	private val moveList: MutableList<Move>
	private var inflater: LayoutInflater = activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

	init {
		moveList =
			(0 until moveJsonList.length())
				.map { Move(moveJsonList.getJSONObject(it)) }
				.toMutableList()
	}

	inner class Move(jsonObject: JSONObject) {
		var description: String = jsonObject.getString("Description")
		var date: Calendar = jsonObject.getCalendar("Date")
		var total: Double = jsonObject.getDouble("Total")
		var checked: Boolean = jsonObject.getBoolean("Checked")
		var id: Int = jsonObject.getInt("ID")
	}


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
