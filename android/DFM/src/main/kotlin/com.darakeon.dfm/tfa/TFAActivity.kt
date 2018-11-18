package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.api.old.DELETE
import com.darakeon.dfm.api.old.InternalRequest
import com.darakeon.dfm.auth.auth
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.extensions.redirect
import kotlinx.android.synthetic.main.tfa.code

class TFAActivity : BaseActivity<DELETE>(DELETE) {
	override val contentView = R.layout.tfa
	override val hasTitle: Boolean get() = false

	fun verify(@Suppress("UNUSED_PARAMETER") view: View) {
		api.validateTFA(auth, code.text.toString()) {
			redirect<AccountsActivity>()
		}
	}
}
