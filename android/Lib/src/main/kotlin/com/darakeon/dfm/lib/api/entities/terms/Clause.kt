package com.darakeon.dfm.lib.api.entities.terms

data class Clause (
	val text: String = "",
	val items: List<Clause> = emptyList()
) {
	fun format(): List<String> {
		return format("", 0)
	}

	private fun format(parentPrefix: String, level: Int): List<String> {
		val result = mutableListOf("$parentPrefix $text".trim())

		items.forEachIndexed {
				index, clause ->
			val prefix = digitFormat(level, index+1)
			result += clause.format("$parentPrefix${prefix}.", level+1)
		}

		return result
	}

	private fun digitFormat(level: Int, digit: Int) = listOf(
		::arab, ::letter, ::roman, ::error
	)[level](digit)

	private fun arab(number: Int): String {
		return number.toString()
	}

	fun letter(number: Int): String {
		if (number < 27)
			return (number + 96).toChar().toString()

		return letter((number-1) / 26) + letter((number-1) % 26 + 1)
	}

	fun roman(number: Int): String {
		if (number >= 4000)
			throw NotImplementedError()

		val finalResult = listOf(
			number / 1000,
			number / 100 % 10,
			number / 10 % 10,
			number % 10,
		).mapIndexed(::romanDigit)

		return finalResult.joinToString("")
	}

	private val romanDigits = listOf(
		listOf("M", null),
		listOf("C", "D"),
		listOf("X", "L"),
		listOf("I", "V"),
	)

	private fun romanDigit(order: Int, digit: Int): String {
		when (digit) {
			0 -> return ""
			4 -> return romanDigits[order][0] + romanDigits[order][1]
			9 -> return romanDigits[order][0] + romanDigits[order - 1][0]
		}

		var partialResult = ""
		var original = digit

		if (digit >= 5) {
			partialResult += romanDigits[order][1]
			original -= 5
		}

		while (original > 0) {
			partialResult += romanDigits[order][0]
			original--
		}

		return partialResult
	}

	private fun error(index: Int): String {
		throw NotImplementedError()
	}
}
