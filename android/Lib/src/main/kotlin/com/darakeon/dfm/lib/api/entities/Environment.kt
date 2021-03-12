package com.darakeon.dfm.lib.api.entities

data class Environment(
	val theme: Theme,
	val language: String
) {
	constructor(language: String) : this(Theme.None, language)
	constructor(theme: Theme) : this(theme, "")
}
