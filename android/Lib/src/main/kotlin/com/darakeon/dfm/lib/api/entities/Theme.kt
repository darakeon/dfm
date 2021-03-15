package com.darakeon.dfm.lib.api.entities

import com.darakeon.dfm.lib.R

@Suppress("unused")
enum class Theme(
	val style: Int,
) {
	None(R.style.None),
	DarkMagic(R.style.DarkMagic),
	DarkSober(R.style.DarkSober),
	DarkNature(R.style.DarkNature),
	LightMagic(R.style.LightMagic),
	LightSober(R.style.LightSober),
	LightNature(R.style.LightNature);
}
