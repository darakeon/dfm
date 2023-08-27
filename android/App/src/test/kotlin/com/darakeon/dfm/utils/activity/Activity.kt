package com.darakeon.dfm.utils.activity

import android.app.DatePickerDialog
import androidx.viewbinding.ViewBinding
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.WelcomeBinding
import com.darakeon.dfm.testutils.context.MockContext
import org.robolectric.shadows.ShadowDialog.getShownDialogs

fun getLastDatePicker() =
	getShownDialogs()
		.filterIsInstance<DatePickerDialog>()
		.last { it.isShowing }

fun mockContext() =
	MockContext(TestBaseActivity::class)

open class TestBaseActivity: BaseActivity<ViewBinding>() {
	override fun inflateBinding(): ViewBinding {
		return WelcomeBinding.inflate(layoutInflater)
	}
}
