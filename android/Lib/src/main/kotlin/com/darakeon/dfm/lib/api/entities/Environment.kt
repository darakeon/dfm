package com.darakeon.dfm.lib.api.entities

data class Environment(
	val theme: ThemeEnum,
	val language: String
) {
	constructor(language: String) : this(ThemeEnum(Theme.None.code), language)
	constructor(theme: Theme) : this(ThemeEnum(theme.code), "")
}
