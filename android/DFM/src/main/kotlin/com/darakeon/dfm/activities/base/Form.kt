package com.darakeon.dfm.activities.base

import android.app.Activity
import android.app.AlertDialog
import android.graphics.Typeface
import android.util.TypedValue
import android.widget.EditText
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.uiHelpers.dialogs.DialogSelectClickListener
import org.json.JSONArray

fun TextView.setColorByAttr(attr : Int){
	val typedValue = TypedValue()
	context.theme.resolveAttribute(attr, typedValue, true)
	setTextColor(typedValue.data)
}

fun TextView.applyGlyphicon(ac: SmartActivity<*>) {
	if (text.length != 1)
		return

	typeface = Typeface.createFromAsset(ac.assets, "fonts/glyphicons-halflings-regular.ttf")
}

fun Any.getChildOrMe(fieldName: String): Any {
	for (field in javaClass.declaredFields) {
		if (field.name == fieldName)
		{
			field.isAccessible = true
			return field.get(this)
		}
	}

	return this
}

fun Activity.getValue(id: Int): String {
	val field = findViewById<EditText>(id)

	return field.text.toString()
}

fun Activity.setValueColored(id: Int, value: Double) {
	val field = findViewById<TextView>(id)

	field.text = String.format("%1$,.2f", value)

	val color = if (value < 0) R.attr.negative else R.attr.positive
	field.setColorByAttr(color)
}

fun Activity.setValue(id: Int, text: Any?) {
	setValue(id, text.toString())
}

fun Activity.setValue(id: Int, text: String?) {
	val field = findViewById<TextView>(id)
	field.text = text
}


fun Activity.showChangeList(list: JSONArray?, titleId: Int, selectList: DialogSelectClickListener) {
	if (list != null) {
		val adapter = arrayOfNulls<CharSequence>(list.length())

		for (c in 0..list.length() - 1) {
			val item = list.getJSONObject(c)
			adapter[c] = item.getString("Text")
		}

		val title = getString(titleId)

		AlertDialog.Builder(this).setTitle(title)
				.setItems(adapter, selectList).show()
	}
}
