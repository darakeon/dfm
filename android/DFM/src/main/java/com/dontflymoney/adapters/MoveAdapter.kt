package com.dontflymoney.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter

import com.dontflymoney.baseactivity.SmartActivity
import com.dontflymoney.layout.MoveLine
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DateTime

import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject

import java.util.ArrayList
import java.util.Calendar

class MoveAdapter @Throws(JSONException::class)
constructor(private val activity: SmartActivity, moveJsonList: JSONArray, private val canCheck: Boolean) : BaseAdapter() {
    private val moveList: MutableList<Move>

    init {

        moveList = ArrayList<Move>()

        for (a in 0..moveJsonList.length() - 1) {
            val move = Move(moveJsonList.getJSONObject(a))
            moveList.add(move)
        }

        inflater = activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
    }

    inner class Move @Throws(JSONException::class)
    internal constructor(jsonObject: JSONObject) {
        var Description: String
        var Date: Calendar
        var Total: Double = 0.toDouble()
        var Checked: Boolean? = null
        var ID: Int = 0

        init {
            Description = jsonObject.getString("Description")
            Date = DateTime.getCalendar(jsonObject.getJSONObject("Date"))
            Total = jsonObject.getDouble("Total")
            Checked = jsonObject.getBoolean("Checked")
            ID = jsonObject.getInt("ID")
        }
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

    override fun getView(position: Int, view: View, viewGroup: ViewGroup): View {
        @SuppressLint("ViewHolder", "InflateParams")
        val line = inflater!!.inflate(R.layout.extract_line, null) as MoveLine

        try {
            val color = if (position % 2 == 0) Color.TRANSPARENT else Color.LTGRAY
            line.setMove(activity, moveList[position], color, canCheck)
        } catch (e: JSONException) {
            e.printStackTrace()
        }

        return line
    }

    companion object {
        private var inflater: LayoutInflater? = null
    }
}
