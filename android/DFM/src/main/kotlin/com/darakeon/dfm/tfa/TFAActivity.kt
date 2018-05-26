package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.auth.getAuth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.redirect
import kotlinx.android.synthetic.main.tfa.code

class TFAActivity : BaseActivity<TFAStatic>(TFAStatic) {
	override val contentView = R.layout.tfa
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
