package com.darakeon.dfm.utils

import android.content.Intent

fun Intent.getActivityName() =
	component?.className?.split(".")?.last()
