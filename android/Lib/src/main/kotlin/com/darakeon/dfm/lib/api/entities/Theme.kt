package com.darakeon.dfm.lib.api.entities

import com.darakeon.dfm.lib.R

@Suppress("unused")
enum class Theme(
	val code: Int,
	val style: Int,
) {
	None(0, R.style.Test),
	DarkMagic(1, R.style.DarkMagic),
	DarkSober(2, R.style.DarkSober),
	DarkNature(3, R.style.DarkNature),
	DarkMono(4, R.style.DarkMono),
	LightMagic(-1, R.style.LightMagic),
	LightSober(-2, R.style.LightSober),
	LightNature(-3, R.style.LightNature),
	LightMono(-4, R.style.LightMono);

	companion object {
		fun get(value: Int?) =
			entries.singleOrNull {
				it.code == value
			}
	}
}
