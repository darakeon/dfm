package com.darakeon.dfm.lib.utils

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.testutils.api.CallMock

class CallMock : CallMock<Body<String>> {
	constructor(): super({ Body(it, null, null) })
	constructor(result: String): super({ Body(it, null, null) }, result)
	constructor(error: Exception): super({ Body(it, null, null) }, error)
}
