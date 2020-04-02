package com.darakeon.dfm.api.entities.summary

data class Summary(
	val title: String = "",
	val total: Double = 0.0,
	val monthList: List<Month> = emptyList()
)
