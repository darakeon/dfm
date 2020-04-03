package com.darakeon.dfm.utils.activity

import android.content.Intent

fun Intent.getActivityName() =
	component?.className?.split(".")?.last()
