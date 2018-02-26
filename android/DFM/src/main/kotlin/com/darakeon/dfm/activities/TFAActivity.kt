package com.darakeon.dfm.activities

import android.os.Bundle
import android.view.View
import android.view.Window
import com.darakeon.dfm.R
import com.darakeon.dfm.activities.base.SmartActivity
import com.darakeon.dfm.activities.base.redirect
import com.darakeon.dfm.activities.objects.TFAStatic
import com.darakeon.dfm.api.InternalRequest
import com.darakeon.dfm.user.getAuth
import kotlinx.android.synthetic.main.tfa.*

class TFAActivity : SmartActivity<TFAStatic>(TFAStatic) {
	override fun contentView(): Int = R.layout.tfa
	override val hasTitle: Boolean get() = false

	fun verify(@Suppress("UNUSED_PARAMETER") view: View) {
		val request = InternalRequest(
			this, "Users/TFA", { redirect<AccountsActivity>() }
		)
		request.addParameter("ticket", getAuth())
		request.addParameter("Code", code.text)
		request.post()
	}
}
