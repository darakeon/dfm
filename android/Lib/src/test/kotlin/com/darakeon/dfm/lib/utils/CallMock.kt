package com.darakeon.dfm.lib.utils

import com.darakeon.dfm.lib.api.entities.Body
import com.darakeon.dfm.lib.api.entities.status.StatusResponse
import com.darakeon.dfm.testutils.api.CallMock


class CallMock {
	class ForString : CallMock<String> {
		constructor(): super({ Body(it, null, null) })
		constructor(result: String): super({ Body(it, null, null) }, result)
		constructor(error: Exception): super({ Body(it, null, null) }, error)
	}
	class ForStatusResponse : CallMock<StatusResponse> {
		constructor(): super({ Body(it, null, null) })
		constructor(result: StatusResponse): super({ Body(it, null, null) }, result)
		constructor(error: Exception): super({ Body(it, null, null) }, error)
	}
}
