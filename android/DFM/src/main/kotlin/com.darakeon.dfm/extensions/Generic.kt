package com.darakeon.dfm.extensions

fun Any.getChildOrMe(fieldName: String): Any? {
	for (field in javaClass.declaredFields) {
		if (field.name == fieldName)
		{
			field.isAccessible = true
			return field.get(this)
		}
	}

	return this
}
