package com.darakeon.dfm.lib.api.entities

data class ThemeEnum(
    val code: Int,
) {
	val enum get() = Theme.get(code)!!
}
