package com.darakeon.dfm.testutils

import org.mockito.stubbing.OngoingStubbing

fun<T> OngoingStubbing<T>.execute(
	call: (answer: Array<Any>) -> Unit
): OngoingStubbing<T> {
	return then { call(it.arguments) }
}
