package com.darakeon.dfm.utils.activity

import android.app.DatePickerDialog
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.testutils.context.MockContext
import org.robolectric.shadows.ShadowDialog.getShownDialogs

fun getLastDatePicker() =
	getShownDialogs()
		.filterIsInstance<DatePickerDialog>()
		.last { it.isShowing }

fun mockContext() =
	MockContext(BaseActivity::class)
