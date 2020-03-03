package com.darakeon.dfm.extensions

import android.app.Activity
import android.app.AlertDialog
import android.graphics.Typeface
import android.util.TypedValue
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.ComboItem

fun TextView.setColorByAttr(attr : Int){
	val typedValue = TypedValue()
	context.theme.resolveAttribute(attr, typedValue, true)
	setTextColor(typedValue.data)
}

fun TextView.applyGlyphicon() {
	if (text.length != 1)
		return

	typeface = Typeface.createFromAsset(context.assets, "fonts/glyphicons-halflings-regular.ttf")
}

fun setValueColored(field: TextView, value: Double) {
	field.text = String.format("%1$,.2f", value)

	val color = if (value < 0) R.attr.negative else R.attr.positive
	field.setColorByAttr(color)
}

fun Activity.showChangeList(list: Array<ComboItem>, titleId: Int, setResult: (String, String) -> Unit) {
	val adapter = list.map { it.text as CharSequence }.toTypedArray()

	val title = getString(titleId)

	AlertDialog.Builder(this).setTitle(title)
		.setItems(adapter) { _, w -> run {
			setResult(
				list[w].text,
				list[w].value
			)
		}}.show()
}
