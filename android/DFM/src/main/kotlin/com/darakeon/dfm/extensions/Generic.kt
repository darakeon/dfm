package com.darakeon.dfm.extensions

fun Any.getChildOrMe(fieldName: String) =
	getChild(javaClass, fieldName) ?: this

private fun Any.getChild(`class`: Class<*>, fieldName: String): Any? {
	for (field in `class`.declaredFields) {
		if (field.name == fieldName) {
			field.isAccessible = true
			return field.get(this)
		}
	}

	val `super` = `class`.superclass ?: return null

	return getChild(`super`, fieldName)
}
