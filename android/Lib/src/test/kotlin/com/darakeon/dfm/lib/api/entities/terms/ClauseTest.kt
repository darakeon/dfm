package com.darakeon.dfm.lib.api.entities.terms

import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Test

class ClauseTest {

	private val testClause = Clause(
		"main",
		listOf(
			Clause(
				"clause 1",
				listOf(
					Clause("clause 1a"),
					Clause("clause 1b"),
					Clause("clause 1c"),
				)
			),
			Clause("clause 2"),
			Clause(
				"clause 3",
				listOf(
					Clause(
						"clause 3a",
						listOf(
							Clause("clause 3aI"),
							Clause("clause 3aII"),
						)
					),
					Clause("clause 3b"),
				)
			),
		)
	)

	@Test
	fun format() {
		assertThat(
			testClause.format(),
			`is`(
				listOf(
					"main",
					"1. clause 1",
					"1.a. clause 1a",
					"1.b. clause 1b",
					"1.c. clause 1c",
					"2. clause 2",
					"3. clause 3",
					"3.a. clause 3a",
					"3.a.I. clause 3aI",
					"3.a.II. clause 3aII",
					"3.b. clause 3b",
				)
			)
		)
	}

	@Test
	fun letter() {
		val clause = Clause()

		assertThat(clause.letter(1), `is`("a"))
		assertThat(clause.letter(2), `is`("b"))
		assertThat(clause.letter(26), `is`("z"))
		assertThat(clause.letter(27), `is`("aa"))
		assertThat(clause.letter(28), `is`("ab"))
		assertThat(clause.letter(52), `is`("az"))
		assertThat(clause.letter(53), `is`("ba"))
		assertThat(clause.letter(54), `is`("bb"))
		assertThat(clause.letter(78), `is`("bz"))
		assertThat(clause.letter(677), `is`("za"))
		assertThat(clause.letter(678), `is`("zb"))
		assertThat(clause.letter(702), `is`("zz"))
		assertThat(clause.letter(703), `is`("aaa"))
	}

	@Test
	fun roman() {
		val clause = Clause()

		assertThat(clause.roman(1), `is`("I"))
		assertThat(clause.roman(2), `is`("II"))
		assertThat(clause.roman(3), `is`("III"))
		assertThat(clause.roman(4), `is`("IV"))
		assertThat(clause.roman(5), `is`("V"))
		assertThat(clause.roman(6), `is`("VI"))
		assertThat(clause.roman(7), `is`("VII"))
		assertThat(clause.roman(8), `is`("VIII"))
		assertThat(clause.roman(9), `is`("IX"))

		assertThat(clause.roman(10), `is`("X"))
		assertThat(clause.roman(11), `is`("XI"))
		assertThat(clause.roman(15), `is`("XV"))
		assertThat(clause.roman(16), `is`("XVI"))
		assertThat(clause.roman(20), `is`("XX"))
		assertThat(clause.roman(21), `is`("XXI"))
		assertThat(clause.roman(25), `is`("XXV"))
		assertThat(clause.roman(26), `is`("XXVI"))
		assertThat(clause.roman(30), `is`("XXX"))
		assertThat(clause.roman(31), `is`("XXXI"))
		assertThat(clause.roman(35), `is`("XXXV"))
		assertThat(clause.roman(36), `is`("XXXVI"))
		assertThat(clause.roman(40), `is`("XL"))
		assertThat(clause.roman(41), `is`("XLI"))
		assertThat(clause.roman(45), `is`("XLV"))
		assertThat(clause.roman(46), `is`("XLVI"))
		assertThat(clause.roman(50), `is`("L"))
		assertThat(clause.roman(51), `is`("LI"))
		assertThat(clause.roman(55), `is`("LV"))
		assertThat(clause.roman(56), `is`("LVI"))
		assertThat(clause.roman(60), `is`("LX"))
		assertThat(clause.roman(61), `is`("LXI"))
		assertThat(clause.roman(65), `is`("LXV"))
		assertThat(clause.roman(66), `is`("LXVI"))
		assertThat(clause.roman(70), `is`("LXX"))
		assertThat(clause.roman(71), `is`("LXXI"))
		assertThat(clause.roman(75), `is`("LXXV"))
		assertThat(clause.roman(76), `is`("LXXVI"))
		assertThat(clause.roman(80), `is`("LXXX"))
		assertThat(clause.roman(81), `is`("LXXXI"))
		assertThat(clause.roman(85), `is`("LXXXV"))
		assertThat(clause.roman(86), `is`("LXXXVI"))
		assertThat(clause.roman(90), `is`("XC"))
		assertThat(clause.roman(91), `is`("XCI"))
		assertThat(clause.roman(95), `is`("XCV"))
		assertThat(clause.roman(96), `is`("XCVI"))

		assertThat(clause.roman(100), `is`("C"))
		assertThat(clause.roman(101), `is`("CI"))
		assertThat(clause.roman(110), `is`("CX"))
		assertThat(clause.roman(111), `is`("CXI"))

		assertThat(clause.roman(200), `is`("CC"))
		assertThat(clause.roman(201), `is`("CCI"))
		assertThat(clause.roman(210), `is`("CCX"))
		assertThat(clause.roman(211), `is`("CCXI"))

		assertThat(clause.roman(300), `is`("CCC"))
		assertThat(clause.roman(301), `is`("CCCI"))
		assertThat(clause.roman(310), `is`("CCCX"))
		assertThat(clause.roman(311), `is`("CCCXI"))

		assertThat(clause.roman(400), `is`("CD"))
		assertThat(clause.roman(401), `is`("CDI"))
		assertThat(clause.roman(410), `is`("CDX"))
		assertThat(clause.roman(411), `is`("CDXI"))

		assertThat(clause.roman(500), `is`("D"))
		assertThat(clause.roman(501), `is`("DI"))
		assertThat(clause.roman(510), `is`("DX"))
		assertThat(clause.roman(511), `is`("DXI"))

		assertThat(clause.roman(600), `is`("DC"))
		assertThat(clause.roman(601), `is`("DCI"))
		assertThat(clause.roman(610), `is`("DCX"))
		assertThat(clause.roman(611), `is`("DCXI"))

		assertThat(clause.roman(700), `is`("DCC"))
		assertThat(clause.roman(701), `is`("DCCI"))
		assertThat(clause.roman(710), `is`("DCCX"))
		assertThat(clause.roman(711), `is`("DCCXI"))

		assertThat(clause.roman(800), `is`("DCCC"))
		assertThat(clause.roman(801), `is`("DCCCI"))
		assertThat(clause.roman(810), `is`("DCCCX"))
		assertThat(clause.roman(811), `is`("DCCCXI"))

		assertThat(clause.roman(900), `is`("CM"))
		assertThat(clause.roman(901), `is`("CMI"))
		assertThat(clause.roman(910), `is`("CMX"))
		assertThat(clause.roman(911), `is`("CMXI"))

		assertThat(clause.roman(1000), `is`("M"))
		assertThat(clause.roman(1001), `is`("MI"))
		assertThat(clause.roman(1010), `is`("MX"))
		assertThat(clause.roman(1011), `is`("MXI"))

		assertThat(clause.roman(1100), `is`("MC"))
		assertThat(clause.roman(1101), `is`("MCI"))
		assertThat(clause.roman(1110), `is`("MCX"))
		assertThat(clause.roman(1111), `is`("MCXI"))

		assertThat(clause.roman(1200), `is`("MCC"))
		assertThat(clause.roman(1201), `is`("MCCI"))
		assertThat(clause.roman(1210), `is`("MCCX"))
		assertThat(clause.roman(1211), `is`("MCCXI"))

		assertThat(clause.roman(1300), `is`("MCCC"))
		assertThat(clause.roman(1301), `is`("MCCCI"))
		assertThat(clause.roman(1310), `is`("MCCCX"))
		assertThat(clause.roman(1311), `is`("MCCCXI"))

		assertThat(clause.roman(1400), `is`("MCD"))
		assertThat(clause.roman(1401), `is`("MCDI"))
		assertThat(clause.roman(1410), `is`("MCDX"))
		assertThat(clause.roman(1411), `is`("MCDXI"))

		assertThat(clause.roman(1500), `is`("MD"))
		assertThat(clause.roman(1501), `is`("MDI"))
		assertThat(clause.roman(1510), `is`("MDX"))
		assertThat(clause.roman(1511), `is`("MDXI"))

		assertThat(clause.roman(1600), `is`("MDC"))
		assertThat(clause.roman(1601), `is`("MDCI"))
		assertThat(clause.roman(1610), `is`("MDCX"))
		assertThat(clause.roman(1611), `is`("MDCXI"))

		assertThat(clause.roman(1700), `is`("MDCC"))
		assertThat(clause.roman(1701), `is`("MDCCI"))
		assertThat(clause.roman(1710), `is`("MDCCX"))
		assertThat(clause.roman(1711), `is`("MDCCXI"))

		assertThat(clause.roman(1800), `is`("MDCCC"))
		assertThat(clause.roman(1801), `is`("MDCCCI"))
		assertThat(clause.roman(1810), `is`("MDCCCX"))
		assertThat(clause.roman(1811), `is`("MDCCCXI"))

		assertThat(clause.roman(1900), `is`("MCM"))
		assertThat(clause.roman(1901), `is`("MCMI"))
		assertThat(clause.roman(1910), `is`("MCMX"))
		assertThat(clause.roman(1911), `is`("MCMXI"))

		assertThat(clause.roman(2000), `is`("MM"))
		assertThat(clause.roman(2001), `is`("MMI"))
		assertThat(clause.roman(2010), `is`("MMX"))
		assertThat(clause.roman(2011), `is`("MMXI"))

		assertThat(clause.roman(2100), `is`("MMC"))
		assertThat(clause.roman(2101), `is`("MMCI"))
		assertThat(clause.roman(2110), `is`("MMCX"))
		assertThat(clause.roman(2111), `is`("MMCXI"))

		assertThat(clause.roman(3000), `is`("MMM"))
		assertThat(clause.roman(3001), `is`("MMMI"))
		assertThat(clause.roman(3010), `is`("MMMX"))
		assertThat(clause.roman(3011), `is`("MMMXI"))
		assertThat(clause.roman(3100), `is`("MMMC"))
		assertThat(clause.roman(3101), `is`("MMMCI"))
		assertThat(clause.roman(3110), `is`("MMMCX"))
		assertThat(clause.roman(3111), `is`("MMMCXI"))

		assertThat(clause.roman(3999), `is`("MMMCMXCIX"))
	}

	@Test(expected = NotImplementedError::class)
	fun romanOut() {
		val clause = Clause()
		clause.roman(4000)
	}
}
