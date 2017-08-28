package com.darakeon.dfm.user

import android.content.Context
import android.provider.Settings.Secure

object Unique {
    fun GetKey(context: Context): String {
        return Secure.getString(context.contentResolver, Secure.ANDROID_ID)
    }
}
