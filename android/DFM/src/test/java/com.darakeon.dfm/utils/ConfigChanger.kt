package com.darakeon.dfm.utils

import android.os.Build
import java.lang.reflect.Field
import java.lang.reflect.Modifier

object ConfigChanger {
	fun setAndroidVersion(version: Int) {
		val field = Build.VERSION::class.java.getField("SDK_INT")
		field.isAccessible = true

		val modifiersField: Field = Field::class.java.getDeclaredField("modifiers")
		modifiersField.isAccessible = true
		modifiersField.setInt(field, field.modifiers and Modifier.FINAL.inv())

		field.set(null, version)
	}
}
