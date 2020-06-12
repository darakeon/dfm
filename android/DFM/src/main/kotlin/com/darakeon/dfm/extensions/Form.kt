package com.darakeon.dfm.extensions

import android.annotation.SuppressLint
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

fun TextView.setValueColored(value: Double) {
	text = String.format("%,.2f", value)

	@SuppressLint("SetTextI18n")
	if (value > 0) text = "+$text"

	setColorByAttr(getColor(value))
}

private fun getColor(value: Double): Int {
	if (value < 0)
		return R.attr.negative

	if (value > 0)
		return R.attr.positive

	return R.attr.neutral
}

fun Activity.showChangeList(
	list: Array<ComboItem>,
	titleId: Int,
	setResult: (String, String) -> Unit
) {
	val adapter = list.map { it.text }.toTypedArray()

	val title = getString(titleId)

	AlertDialog.Builder(this).setTitle(title)
		.setItems(adapter) { _, w -> run {
			setResult(
				list[w].text,
				list[w].value
			)
		}}.show()
}
