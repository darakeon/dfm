package com.darakeon.dfm.api.entities.extract

data class Extract(
	var title: String = "",
	var total: Double = 0.0,
	var canCheck: Boolean = false,
	val moveList: List<Move> = emptyList()
)
