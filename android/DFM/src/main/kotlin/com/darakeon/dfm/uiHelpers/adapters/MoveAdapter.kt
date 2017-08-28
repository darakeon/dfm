package com.darakeon.dfm.uiHelpers.adapters

import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.ExtractActivity
import com.darakeon.dfm.api.DateTime
import com.darakeon.dfm.uiHelpers.views.MoveLine
import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject
import java.util.*

class MoveAdapter(private val activity: ExtractActivity, moveJsonList: JSONArray, private val canCheck: Boolean) : BaseAdapter() {

    private val moveList: MutableList<Move>
    private var inflater: LayoutInflater = activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

    init {
        moveList =
            (0..moveJsonList.length() - 1)
                .map { Move(moveJsonList.getJSONObject(it)) }
                .toMutableList()
    }

    inner class Move(jsonObject: JSONObject) {
        var Description: String = jsonObject.getString("Description")
        var Date: Calendar = DateTime.getCalendar(jsonObject.getJSONObject("Date"))
        var Total: Double = jsonObject.getDouble("Total")
        var Checked: Boolean = jsonObject.getBoolean("Checked")
        var ID: Int = jsonObject.getInt("ID")
    }


    override fun getCount(): Int {
        return moveList.size
    }

    override fun getItem(position: Int): Any {
        return position
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, view: View?, viewGroup: ViewGroup): View? {
        val line = inflater.inflate(R.layout.extract_line, null) as MoveLine

        try {
            val color = if (position % 2 == 0) Color.TRANSPARENT else Color.LTGRAY
            line.setMove(activity, moveList[position], color, canCheck)
        } catch (e: JSONException) {
            e.printStackTrace()
        }

        return line
    }

}
