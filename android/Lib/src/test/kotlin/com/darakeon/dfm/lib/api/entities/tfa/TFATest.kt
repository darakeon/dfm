package com.darakeon.dfm.lib.api.entities.tfa

import com.darakeon.dfm.testutils.BaseTest
import org.hamcrest.CoreMatchers.`is`
import org.hamcrest.MatcherAssert.assertThat
import org.junit.Assert.assertNull
import org.junit.Test

class TFATest: BaseTest() {
	@Test
	fun operationEnum() {
		val tfa1 = TFA(OperationEnum(0))
		assertThat(tfa1.operationEnum, `is`(Operation.Validate))

		val tfa2 = TFA(OperationEnum(1))
		assertThat(tfa2.operationEnum, `is`(Operation.AskRemove))

		val tfa3 = TFA(OperationEnum(1))
		tfa3.operationEnum = Operation.Validate
		assertThat(tfa3.operation.code, `is`(0))

		val tfa4 = TFA(OperationEnum(0))
		tfa4.operationEnum = Operation.AskRemove
		assertThat(tfa4.operation.code, `is`(1))
	}

	@Test
	fun forValidate() {
		val tfa = TFA.forValidate("code")

		assertThat(tfa.operationEnum, `is`(Operation.Validate))
		assertThat(tfa.code, `is`("code"))
		assertNull(tfa.password)
	}

	@Test
	fun forRemove() {
		val tfa = TFA.forRemove("password")

		assertThat(tfa.operationEnum, `is`(Operation.AskRemove))
		assertNull(tfa.code)
		assertThat(tfa.password, `is`("password"))
	}
}
