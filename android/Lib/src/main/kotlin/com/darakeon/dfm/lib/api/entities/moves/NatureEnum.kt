package com.darakeon.dfm.lib.api.entities.moves

data class NatureEnum(
	val code: Int,
) {
	val enum get() = Nature.get(code)!!
}
