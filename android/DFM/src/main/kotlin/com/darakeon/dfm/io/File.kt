package com.darakeon.dfm.io

import android.content.Context
import com.darakeon.dfm.R
import java.io.*

/*
TODO: use kotlin IO
class File(private val context: Context, name: FileNames) {
private val name: String

private var status: String? = null


init {
this.name = name.name
}


fun WriteToFile(data: String?) {
var data = data
try {
if (data == null)
data = ""

val fileOutput = context.openFileOutput(name, Context.MODE_PRIVATE)

val writer = OutputStreamWriter(fileOutput)

writer.write(data)
writer.close()
} catch (e: IOException) {
status = context.getString(R.string.error_file_write) + ": " + e.toString()
}

}


fun ReadFromFile(): String {
var result = ""

try {
val inputStream = context.openFileInput(name)

if (inputStream != null) {
val inputStreamReader = InputStreamReader(inputStream)
val bufferedReader = BufferedReader(inputStreamReader)

var line: String
val allContent = StringBuilder()

while ((line = bufferedReader.readLine()) != null) {
    allContent.append(line)
}

inputStream.close()

result = allContent.toString()
}
} catch (e: FileNotFoundException) {
status = context.getString(R.string.error_file_not_found) + ": " + e.toString()
} catch (e: IOException) {
status = context.getString(R.string.error_file_read) + ": " + e.toString()
}

return result
}

companion object {
internal val extension = ".dfm"
}


}
*/