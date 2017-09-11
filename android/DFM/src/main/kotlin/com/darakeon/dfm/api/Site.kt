package com.darakeon.dfm.api

object Site {
	private val publicDomain = "dontflymoney.com"
	internal val Domain = publicDomain

	internal fun GetProtocol(): String = if (isLocal) "http" else "https"

	private val isLocal: Boolean get() = Domain != publicDomain


}
