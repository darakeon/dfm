package com.darakeon.dfm.extensions

import java.lang.reflect.Field

fun Any.getPrivate(vararg fieldNames: String): Any? {
	return getField(fieldNames.asList()) { _, _ -> }
}

fun Any.setPrivate(vararg fieldNames: String, value: () -> Any) {
	getField(fieldNames.asList()) {
		field, parent -> field?.set(parent, value())
	}
}

private fun Any.getField(fieldNames: List<String>, set: (Field?, Any?) -> Unit): Any? {
	var field: Field? = null
	var element: Any? = this
	var parent: Any? = null

	for (fieldName in fieldNames) {
		field = element?.getField(fieldName)
		parent = element
		element = field?.get(element)
	}

	set(field, parent)

	return element
}

private fun Any.getField(fieldName: String): Field? {
	return getField(javaClass, fieldName)
}

private fun Any.getField(`class`: Class<*>, fieldName: String): Field? {
	for (field in `class`.declaredFields) {
		if (field.name != fieldName)
			continue

		field.isAccessible = true
		return field
	}

	val `super` = `class`.superclass ?: return null

	return getField(`super`, fieldName)
}
