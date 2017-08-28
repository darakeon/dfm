package com.dontflymoney.watchers

import com.dontflymoney.entities.Move
import com.dontflymoney.viewhelper.AfterTextWatcher

class ValueWatcher(private val move: Move) : AfterTextWatcher() {

    override fun textChanged(text: String) {
        try {
            move.Value = java.lang.Double.parseDouble(text)
        } catch (ignored: NumberFormatException) {
        }

    }
}
