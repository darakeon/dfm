package com.darakeon.dfm.lib.api.entities.status

data class StatusEnum(
    val code: Int,
) {
	val enum get() = Status.get(code)!!
}
