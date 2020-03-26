package com.darakeon.dfm.utils.api

import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths

fun readBundle(jsonName: String) =
	readFromFile("bundles/$jsonName.json")

fun readResponse(jsonName: String) =
	readFromFile("responses/$jsonName.json")

fun readFromFile(filename: String): String {
	val resource = ClassLoader.getSystemResource(filename)
	val path = Paths.get(resource.toURI())
	val bytes = Files.readAllBytes(path)
	val charset = Charset.forName("UTF-8")
	return String(bytes, charset)
}

const val internetError = "Error on contacting the Pig. " +
	"It's either a internet problem or a system issue. " +
	"If it's the second option, " +
	"the error is recorded and we will solve it as soon as possible."

const val noBody = "Error on contacting the Pig. " +
	"The error is recorded and we will solve it as soon as possible."

const val internetSlow = "Problems while connecting to website, " +
	"please check your internet connection"
