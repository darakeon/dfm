package com.darakeon.dfm.api

object Site {
	private val publicDomain = "dontflymoney.com"
	internal val Domain = publicDomain

	internal fun GetProtocol(): String {
		return if (IsLocal()) "http" else "https"
	}

	fun IsLocal(): Boolean {
		return Domain != publicDomain
	}


}
