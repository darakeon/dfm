package com.darakeon.dfm.testutils

import java.lang.reflect.Field

fun <T> Any.getPrivate(vararg fieldNames: String): T {
	@Suppress("UNCHECKED_CAST")
	return getField(fieldNames.asList()) as T
}

fun Any.setPrivate(vararg fieldNames: String, value: () -> Any) {
	getField(fieldNames.asList()) {
		field, parent -> field?.set(parent, value())
	}
}

private fun Any.getField(
	fieldNames: List<String>,
	set: ((Field?, Any?) -> Unit)? = null
): Any? {
	var field: Field? = null
	var element: Any? = this
	var parent: Any? = null

	for (fieldName in fieldNames) {
		field = element?.getField(fieldName)
		parent = element
		element = field?.get(element)
	}

	set?.invoke(field, parent)

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
