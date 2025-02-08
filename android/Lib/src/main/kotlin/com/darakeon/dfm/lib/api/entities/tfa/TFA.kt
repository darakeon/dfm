package com.darakeon.dfm.lib.api.entities.tfa

data class TFA (
	var operation: OperationEnum,
	val code: String? = null,
	val password: String? = null,
) {
	var operationEnum
		get() = operation.enum
		set(value) { operation = OperationEnum(value.value) }

	companion object {
		fun forValidate(code: String): TFA {
			val operation = OperationEnum(Operation.Validate.value)
			return TFA(operation, code, null)
		}

		fun forRemove(password: String): TFA {
			val operation = OperationEnum(Operation.AskRemove.value)
			return TFA(operation, null, password)
		}
	}
}
