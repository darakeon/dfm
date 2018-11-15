package com.darakeon.dfm.extensions

import android.app.Activity
import android.app.AlertDialog
import android.graphics.Typeface
import android.util.TypedValue
import android.widget.TextView
import com.darakeon.dfm.R
import com.darakeon.dfm.api.entities.ComboItem
import com.darakeon.dfm.base.BaseActivity

fun TextView.setColorByAttr(attr : Int){
	val typedValue = TypedValue()
	context.theme.resolveAttribute(attr, typedValue, true)
	setTextColor(typedValue.data)
}

fun TextView.applyGlyphicon(ac: BaseActivity<*>) {
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
