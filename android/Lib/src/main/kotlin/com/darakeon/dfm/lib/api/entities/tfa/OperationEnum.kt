package com.darakeon.dfm.lib.api.entities.tfa

data class OperationEnum(
	val code: Int,
) {
	val enum get() = Operation.get(code)!!
}
