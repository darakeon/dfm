package com.darakeon.dfm.testutils.context

import android.content.Intent

fun Intent.getCalledName() =
	component?.className?.split(".")?.last()
