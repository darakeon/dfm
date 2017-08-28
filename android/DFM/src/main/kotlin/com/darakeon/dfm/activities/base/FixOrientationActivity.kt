package com.darakeon.dfm.activities.base

import android.app.Activity
import android.content.pm.ActivityInfo
import android.os.Bundle

open class FixOrientationActivity : Activity() {
    protected var rotated: Boolean = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        rotated = oldConfigInt and ActivityInfo.CONFIG_ORIENTATION == ActivityInfo.CONFIG_ORIENTATION
    }

    override fun onDestroy() {
        super.onDestroy()
        oldConfigInt = changingConfigurations
    }

    override fun onResume() {
        super.onResume()
        oldConfigInt = 0
    }

    companion object {
        private var oldConfigInt: Int = 0
    }

}
