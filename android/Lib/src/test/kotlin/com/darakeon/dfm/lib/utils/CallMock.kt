package com.darakeon.dfm.lib.utils

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.status.StatusResponse
import com.darakeon.dfm.testutils.api.CallMock


class CallMock {
	class ForString : CallMock<String> {
		constructor(): super({ Body(it, null, null) })
		constructor(result: String): super({ Body(it, null, null) }, result)
		constructor(error: Exception): super({ Body(it, null, null) }, error)
		constructor(statusCode: Int): super({ Body(it, null, null) }, statusCode)
	}

	class ForStatusResponse : CallMock<StatusResponse>(
		{ Body(it, null, null) }
	)
}
