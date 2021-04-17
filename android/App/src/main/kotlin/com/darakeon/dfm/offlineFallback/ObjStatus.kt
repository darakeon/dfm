package com.darakeon.dfm.offlineFallback

data class ObjStatus<O>(
	val obj: O,
	var status: Status = Status.Pending,
	var error: String = ""
) {
	fun success() {
		status = Status.Success
	}

	fun error(error: String) {
		status = Status.Error
		this.error = error
	}

	fun has(obj: O) =
		this.obj.hashCode() == obj.hashCode()
}
