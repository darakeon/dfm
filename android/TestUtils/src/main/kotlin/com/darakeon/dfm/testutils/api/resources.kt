package com.darakeon.dfm.testutils.api

import android.content.res.Resources
import android.os.Build
import androidx.annotation.RequiresApi
import java.nio.charset.Charset
import java.nio.file.Files
import java.nio.file.Paths
import java.util.UUID

@RequiresApi(Build.VERSION_CODES.O)
fun readBundle(jsonName: String) =
	readFromFile("bundles", "$jsonName.json")

@RequiresApi(Build.VERSION_CODES.O)
fun readResponse(jsonName: String) =
	readFromFile("responses", "$jsonName.json")

@RequiresApi(Build.VERSION_CODES.O)
private fun readFromFile(folder: String, filename: String): String {
	val curDir = System.getProperty("user.dir")
		?: throw Resources.NotFoundException()

	val path = Paths.get(
		curDir, "..", "TestUtils",
		"src", "main", "assets",
		folder, filename
	)

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

const val internetSlow = "There is a problem with " +
	"the connection to the site"

val guid: UUID = UUID.fromString("01234567-89AB-CDEF-FEDC-BA9876543210")
