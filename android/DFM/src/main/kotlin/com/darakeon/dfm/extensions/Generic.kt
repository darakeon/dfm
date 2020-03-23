package com.darakeon.dfm.extensions

fun Any.getChild(vararg fieldNames: String): Any? {
	var result: Any? = this
	for (fieldName in fieldNames) {
		result = result?.getChild(result.javaClass, fieldName)
	}
	return result
}

private fun Any.getChild(`class`: Class<*>, fieldName: String): Any? {
	for (field in `class`.declaredFields) {
		if (field.name != fieldName)
			continue

		field.isAccessible = true
		return field.get(this)
	}

	val `super` = `class`.superclass ?: return null

	return getChild(`super`, fieldName)
}
