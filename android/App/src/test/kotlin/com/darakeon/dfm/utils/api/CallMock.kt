package com.darakeon.dfm.utils.api

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.testutils.api.CallMock

class CallMock : CallMock<Body<String>> {
	constructor(): super({ Body(it, null, null, null) })
	constructor(result: String): super({ Body(it, null, null, null) }, result)
	constructor(error: Exception): super({ Body(it, null, null, null) }, error)
}
