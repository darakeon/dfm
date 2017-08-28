package com.dontflymoney.io

enum class FileNames private constructor(name: String) {
    Ticket("ticket"),
    Language("language");

    internal var name: String

    init {
        this.name = name + File.getExtension()
    }
}