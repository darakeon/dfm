package com.darakeon.dfm.tests

import android.os.Build
import java.lang.reflect.Field
import java.lang.reflect.Modifier

fun setAndroidVersion(version: Int) {
	val field = Build.VERSION::class.java.getField("SDK_INT")
	field.isAccessible = true

	val modifiersField = Field::class.java
		.declaredFields.firstOrNull {
			it.name == "modifiers"
		}

	if (modifiersField != null) {
		modifiersField.isAccessible = true
		modifiersField.setInt(field, field.modifiers and Modifier.FINAL.inv())
	}

	field.set(null, version)
}
