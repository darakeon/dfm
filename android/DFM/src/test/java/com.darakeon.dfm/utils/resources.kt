package com.darakeon.dfm.utils

import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths

fun readFromFile(filename: String): String {
	val resource = ClassLoader.getSystemResource(filename)
	val path = Paths.get(resource.toURI())
	val bytes = Files.readAllBytes(path)
	val charset = Charset.forName("UTF-8")
	return String(bytes, charset)
}
