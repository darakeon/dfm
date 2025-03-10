package com.darakeon.dfm.lib.api.entities

data class Environment(
	val theme: ThemeEnum,
	val language: String,
	val tfaForgottenWarning: Boolean
) {
	constructor(language: String)
		: this(Theme.None, language)

	constructor(theme: Theme)
		: this(theme, "")

	constructor(tfaForgottenWarning: Boolean)
		: this(Theme.None, "", tfaForgottenWarning)

	constructor(theme: Theme, language: String)
		: this(theme, language, false)

	constructor(theme: Theme, language: String, tfaForgottenWarning: Boolean)
		: this(ThemeEnum(theme.code), language, tfaForgottenWarning)
}
