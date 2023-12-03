package com.darakeon.dfm.lib

import android.os.Build
import android.util.Log
import androidx.annotation.RequiresApi
import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

object Log {
	fun record(text: Any?) {
		toConsole(text)

		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
			if (Files.isWritable(Paths.get(""))) {
				toFile(text)
			}
		}
	}

	private fun toConsole(text: Any?) {
		try {
			Log.e("DFM", text.toString())
		} catch(e: Exception) {
			println(text)
		}
	}

	@RequiresApi(Build.VERSION_CODES.O)
	private fun toFile(text: Any?) {
		val charset = Charset.forName("UTF-8")

		var record = text.toString()

		val path = Paths.get("..", "..", "outputs", "logs", "android", "${BuildConfig.BUILD_TYPE}_${start}.log")

		if (Files.exists(path)) {
			val bytes = Files.readAllBytes(path)
			val current = String(bytes, charset)
			record = "$current\n$record"
		}

		Files.write(path, record.toByteArray(charset))
	}

	private var path: String = ""
	private val start:String
		@RequiresApi(Build.VERSION_CODES.O)
		get() {
			if (path == "") {
				path = now("yyyyMMdd_HHmmss_SSS")
			}
			return path
		}

	fun now(pattern: String): String {
		val calendar = Calendar.getInstance()
		val formatter = SimpleDateFormat(pattern, Locale.getDefault())
		return formatter.format(calendar.time)
	}
}
