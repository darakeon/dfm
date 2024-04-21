package com.darakeon.dfm.lib.extensions

import android.annotation.SuppressLint
import android.app.Activity
import android.app.AlertDialog
import android.graphics.Typeface.createFromAsset
import android.widget.ArrayAdapter
import android.widget.AutoCompleteTextView
import android.widget.TextView
import com.darakeon.dfm.lib.R
import com.darakeon.dfm.lib.api.entities.ComboItem

const val glyphicon = "fonts/glyphicons-halflings-regular.ttf"

fun TextView.setColorByAttr(attr : Int){
	setTextColor(context.getColorByAttr(attr))
}

fun TextView.applyGlyphicon() {
	if (text.length != 1)
		return

	typeface = createFromAsset(context.assets, glyphicon)
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

	return R.attr.zero
}

fun <T: ComboItem> Activity.showChangeList(
	list: Array<T>,
	titleId: Int,
	setResult: (String, String?) -> Unit
) {
	val clear = ComboItem(getString(R.string.clear), null)

	val itemsList = listOf(clear)
		.union(list.asIterable())
		.toTypedArray()

	val adapter = itemsList
		.map { it.text }
		.toTypedArray()

	val title = getString(titleId)

	AlertDialog.Builder(this).setTitle(title)
		.setItems(adapter) { _, w -> run {
			if (w == 0)
				setResult("", null)
			else
				setResult(
					itemsList[w].text,
					itemsList[w].value
				)
		}}.show()
}

fun <T: ComboItem> AutoCompleteTextView.complete(
	list: Array<T>,
	setResult: (String?) -> Unit
) {
	val adapter = ArrayAdapter(
		context,
		android.R.layout.simple_dropdown_item_1line,
		list.map { it.text }
	)

	(context as Activity).runOnUiThread {
		setAdapter(adapter)
	}

	onChange { text ->
		val value = list
			.firstOrNull { it.text == text }
			?.value

		setResult(value)
	}
}
