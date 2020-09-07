package com.darakeon.dfm.lib.api.entities.settings

data class Settings(
	var useCategories: Boolean,
	var moveCheck: Boolean
) {
	constructor() : this(false, false)
}
