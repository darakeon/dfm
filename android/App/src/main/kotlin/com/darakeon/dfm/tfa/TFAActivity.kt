package com.darakeon.dfm.tfa

import android.view.View
import com.darakeon.dfm.R
import com.darakeon.dfm.accounts.AccountsActivity
import com.darakeon.dfm.base.BaseActivity
import com.darakeon.dfm.databinding.TfaBinding
import com.darakeon.dfm.lib.extensions.redirect
import kotlinx.android.synthetic.main.tfa.code

class TFAActivity : BaseActivity<TfaBinding>() {
	override val contentViewId = R.layout.tfa

	fun verify(@Suppress("UNUSED_PARAMETER") view: View) {
		callApi {
			it.validateTFA(code.text.toString()) {
				redirect<AccountsActivity>()
			}
		}
	}
}
