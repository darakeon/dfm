package com.darakeon.dfm.lib

import android.os.Build
import android.util.Log
import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

object Log {
	fun record(text: Any?) {
		toConsole(text)
		toFile(text)
	}

	private fun toConsole(text: Any?) {
		try {
			Log.e("DFM", text.toString())
		} catch(e: Exception) {
			println(text)
		}
	}

	private fun toFile(text: Any?) {
		if (Build.VERSION.SDK_INT < Build.VERSION_CODES.O)
			return

		val charset = Charset.forName("UTF-8")

		var record = text.toString()

		val path = Paths.get("log", "${start}.log")

		if (Files.exists(path)) {
			val bytes = Files.readAllBytes(path)
			val current = String(bytes, charset)
			record = "$current\n$record"
		}

		Files.write(path, record.toByteArray(charset))
	}

	private val start = {
		val dir = Paths.get("log")
		if (!Files.exists(dir))
			Files.createDirectory(dir)

		now("yyyyMMdd_HHmmss_SSS")
	}()

	fun now(pattern: String): String {
		val calendar = Calendar.getInstance()
		val formatter = SimpleDateFormat(pattern, Locale.getDefault())
		return formatter.format(calendar.time)
	}
}
