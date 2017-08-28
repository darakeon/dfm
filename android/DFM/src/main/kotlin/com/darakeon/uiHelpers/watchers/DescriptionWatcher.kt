package com.dontflymoney.watchers

import com.dontflymoney.entities.Move
import com.dontflymoney.viewhelper.AfterTextWatcher

class DescriptionWatcher(private val move: Move) : AfterTextWatcher() {

    override fun textChanged(text: String) {
        move.Description = text
    }
}
