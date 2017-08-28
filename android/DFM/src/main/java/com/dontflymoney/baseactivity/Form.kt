package com.dontflymoney.baseactivity

import android.app.Activity
import android.app.AlertDialog
import android.graphics.Color
import android.view.View
import android.widget.EditText
import android.widget.TextView
import com.dontflymoney.viewhelper.DialogSelectClickListener
import org.json.JSONArray
import org.json.JSONException

class Form internal constructor(private val activity: Activity) {


    fun getValue(id: Int): String {
        val field = getField<EditText>(id)

        return field.text.toString()
    }


    fun setValueColored(id: Int, value: Double) {
        val field = getField<TextView>(id)

        field.text = String.format("%1$,.2f", value)

        val color = if (value < 0) Color.RED else Color.BLUE
        field.setTextColor(color)
    }

    fun setValue(id: Int, text: Any?) {
        setValue(id, text.toString())
    }

    fun setValue(id: Int, text: String?) {
        val field = getField<TextView>(id)

        field.text = text
    }

    private fun <T : View> getField(id: Int): T {
        @Suppress("UNCHECKED_CAST")
        return activity.findViewById(id) as T
    }


    @Throws(JSONException::class)
    fun showChangeList(list: JSONArray?, titleId: Int, selectList: DialogSelectClickListener) {
        if (list != null) {
            val adapter = arrayOfNulls<CharSequence>(list.length())

            for (c in 0..list.length() - 1) {
                val item = list.getJSONObject(c)
                adapter[c] = item.getString("Text")
            }

            val title = activity.getString(titleId)

            AlertDialog.Builder(activity).setTitle(title)
                    .setItems(adapter, selectList).show()
        }
    }

}
