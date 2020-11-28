package com.darakeon.dfm.testutils

import com.darakeon.dfm.lib.Log
import org.junit.rules.TestRule
import org.junit.runner.Description
import org.junit.runners.model.Statement

class LogRule : TestRule {
	override fun apply(statement: Statement, description: Description): Statement {
		return processInStatement {
			log(description.className, description.methodName, "START")
			statement.evaluate()
			log(description.className, description.methodName, "END")
		}
	}

	private fun processInStatement(process: () -> Unit): Statement {
		return object : Statement() {
			override fun evaluate() {
				process()
			}
		}
	}

	fun log(parent: Any, message: Any, stage: String) {
		val now = Log.now("HH:mm:ss.SSS")
		val log = "[${now}] $parent.$message: $stage"
		Log.record(log)
	}
}
