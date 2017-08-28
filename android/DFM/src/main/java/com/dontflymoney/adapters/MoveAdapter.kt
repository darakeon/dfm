package com.dontflymoney.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import com.dontflymoney.layout.MoveLine
import com.dontflymoney.view.ExtractActivity
import com.dontflymoney.view.R
import com.dontflymoney.viewhelper.DateTime
import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject
import java.util.*

class MoveAdapter @Throws(JSONException::class)
constructor(private val activity: ExtractActivity, moveJsonList: JSONArray?, private val canCheck: Boolean) : BaseAdapter() {
    private val moveList: MutableList<Move>

    init {

        moveList = ArrayList<Move>()

        if (moveJsonList != null) {
            for (a in 0..moveJsonList.length() - 1) {
                val move = Move(moveJsonList.getJSONObject(a))
                moveList.add(move)
            }
        }

        inflater = activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
    }

    inner class Move @Throws(JSONException::class)

    internal constructor(jsonObject: JSONObject) {
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
