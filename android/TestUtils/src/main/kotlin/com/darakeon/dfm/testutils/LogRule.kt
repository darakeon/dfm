package com.darakeon.dfm.testutils

import android.os.Build
import androidx.annotation.RequiresApi
import org.junit.rules.TestRule
import org.junit.runner.Description
import org.junit.runners.model.Statement
import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

class LogRule : TestRule {
	override fun apply(statement: Statement, description: Description): Statement {
		return processInStatement {
			log(description.className, description.methodName)
			statement.evaluate()
		}
	}

	private fun processInStatement(process: () -> Unit): Statement {
		return object : Statement() {
			override fun evaluate() {
				process()
			}
		}
	}

	private fun log(parent: Any, message: Any) {
		val now = now("HH:mm:ss.SSS")
		val log = "[${now}] $parent.$message"

		println(log)

		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
			logToFile(log)
		}
	}

	@RequiresApi(Build.VERSION_CODES.O)
	private fun logToFile(content: String) {
		val charset = Charset.forName("UTF-8")

		var record = content

		val path = Paths.get("log", "$start.log")

		if (Files.exists(path)) {
			val bytes = Files.readAllBytes(path)
			val current = String(bytes, charset)
			record = "$current\n$record"
		}

		Files.write(path, record.toByteArray(charset))
	}

	companion object {
		private val start = {
			val dir = Paths.get("log")
			if (!Files.exists(dir))
				Files.createDirectory(dir)

			now("yyyyMMdd_HHmmss_SSS")
		}()

		private fun now(pattern: String): String {
			val calendar = Calendar.getInstance()
			val formatter = SimpleDateFormat(pattern, Locale.getDefault())
			return formatter.format(calendar.time)
		}
	}
}
